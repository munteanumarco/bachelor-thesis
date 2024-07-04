import { AfterViewInit, Component, OnInit } from '@angular/core';
import L from 'leaflet';
import { GeolocationService } from '../../services/geolocation.service';
import { LoaderService } from '../../services/loader.service';
import { retryOperation } from '../../utils/retry-promise';
import { EmergencyEventService } from '../../services/emergency-event.service';
import { getIconForSeverity } from '../../utils/marker-icons/get-icon-for-severity';
import { getSeverityName } from '../../interfaces/emergency/Severity';
import { getEmergencyTypeName } from '../../interfaces/emergency/EmergencyType';
import { getStatusName } from '../../interfaces/emergency/Status';
import { RouterModule } from '@angular/router';
import { formatDate } from '../../utils/date_formatter';
import { signalUpdateFn } from '@angular/core/primitives/signals';
import { MarkerService } from '../../services/marker-service.service';

@Component({
  selector: 'app-emergency-map',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './emergency-map.component.html',
  styleUrl: './emergency-map.component.scss',
})
export class EmergencyMapComponent implements OnInit, AfterViewInit {
  map!: L.Map;
  markers: L.Marker[] = [];
  constructor(
    private readonly geolocationService: GeolocationService,
    private readonly loaderService: LoaderService,
    private readonly emergencyEventService: EmergencyEventService,
    private markerService: MarkerService
  ) {}

  ngOnInit(): void {
    console.log("this is hit");
    this.loaderService.setManualLoading(true);
    this.emergencyEventService
      .getEmergencyEventsMarkers()
      .subscribe((result) => {
        result.markers.forEach((event) => {
          const marker = L.marker([event.latitude, event.longitude], {
            icon: getIconForSeverity(event.severity),
          });

          const popupHtml = this.createPopupContent(
            getEmergencyTypeName(event.type),
            getSeverityName(event.severity),
            getStatusName(event.status),
            'emergency-details?eventId=' + event.id,
            formatDate(new Date(event.updatedAt))
          );
          marker.bindPopup(popupHtml);
          this.markers.push(marker);
        });
        this.loadMarkers();
        this.markers.forEach((marker) => marker.addTo(this.map));
      });
  }

  private initMap(location: GeolocationPosition): void {

    if (!this.map){
    this.map = L.map('map', {
      center: [location.coords.latitude, location.coords.longitude],
      zoom: 10,
    });

    const tiles = L.tileLayer(
      'https://tiles.stadiamaps.com/tiles/outdoors/{z}/{x}/{y}{r}.png',
      {
        maxZoom: 20,
        attribution:
          '&copy; <a href="https://stadiamaps.com/" target="_blank">Stadia Maps</a> &copy; <a href="https://openmaptiles.org/" target="_blank">OpenMapTiles</a> &copy; <a href="https://www.openstreetmap.org/copyright" target="_blank">OpenStreetMap</a>',
      }
    );

    tiles.addTo(this.map);
    this.markers.forEach((marker) => marker.addTo(this.map));}
  }

  private createPopupContent(
    eventType: string,
    severity: string,
    status: string,
    link: string,
    updatedAt: string
  ): string {
    return `
      <div class="p-2 rounded">
        <div class="font-bold text-lg">Event: <b>${eventType}</b></div>
        <div class="text-gray-600">Severity: <b>${severity}</b></div>
        <div class="text-gray-600">Status: <b>${status}</b></div>
        <div class="text-gray-600">Last Updated: <b>${updatedAt}</b></div>
        <a href="${link}"><button class="mt-2 bg-black text-white px-4 py-2 rounded hover:bg-gray-700 transition-all duration-300">
          More Info
        </button></a>
      </div>
    `;
  }

  loadMarkers() {
    retryOperation(this.geolocationService.getCurrentLocation, 5, 1) 
        .then((location: GeolocationPosition) => {
          this.initMap(location);
          this.loaderService.setManualLoading(false);
        })
        .catch(() => {
          console.error('Getting location failed, retrying ...');
        });
  }

  ngAfterViewInit(): void {
    this.loadMarkers();
  }
}
