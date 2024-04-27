import { AfterViewInit, Component, OnInit } from '@angular/core';
import L from 'leaflet';
import { GeolocationService } from '../../services/geolocation.service';
import { LoaderService } from '../../services/loader.service';
import { retryOperation } from '../../utils/retry-promise';
import { EmergencyEventService } from '../../services/emergency-event.service';
import { getIconForSeverity } from '../../utils/marker-icons/get-icon-for-severity';
import { severityNames } from '../../interfaces/emergency/Severity';
import { emergencyTypeNames } from '../../interfaces/emergency/EmergencyType';
import { statusNames } from '../../interfaces/emergency/Status';
import { RouterModule } from '@angular/router';

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
    private readonly emergencyEventService: EmergencyEventService
  ) {}

  ngOnInit(): void {
    this.loaderService.setManualLoading(true);
    this.emergencyEventService.getEmergencyEvents().subscribe((result) => {
      result.markers.forEach((event) => {
        const marker = L.marker([event.latitude, event.longitude], {
          icon: getIconForSeverity(event.severity),
        });

        const popupHtml = this.createPopupContent(
          emergencyTypeNames[event.type],
          severityNames[event.severity],
          statusNames[event.status],
          'emergency-details?eventId=' + event.id
        );
        marker.bindPopup(popupHtml);
        this.markers.push(marker);
      });
    });
  }

  private initMap(location: GeolocationPosition): void {
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
    this.markers.forEach((marker) => marker.addTo(this.map));
  }

  private createPopupContent(
    eventType: string,
    severity: string,
    status: string,
    link: string
  ): string {
    return `
      <div class="p-2 rounded">
        <div class="font-bold text-lg">Event: ${eventType}</div>
        <div class="text-gray-600">Severity: ${severity}</div>
        <div class="text-gray-600">Status: ${status}</div>
        <a href="${link}"><button class="mt-2 bg-black text-white px-4 py-2 rounded hover:bg-gray-700 transition-all duration-300">
          More Info
        </button></a>
      </div>
    `;
  }

  ngAfterViewInit(): void {
    retryOperation(this.geolocationService.getCurrentLocation, 5, 1)
      .then((location: GeolocationPosition) => {
        this.initMap(location);
        this.loaderService.setManualLoading(false);
      })
      .catch(() => {
        console.error('Getting location failed, retrying ...');
      });
  }
}
