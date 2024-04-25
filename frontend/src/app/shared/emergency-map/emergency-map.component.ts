import { AfterViewInit, Component, OnInit } from '@angular/core';
import L from 'leaflet';
import { GeolocationService } from '../../services/geolocation.service';
import { LoaderService } from '../../services/loader.service';
import { retryOperation } from '../../utils/retry-promise';

@Component({
  selector: 'app-emergency-map',
  standalone: true,
  imports: [],
  templateUrl: './emergency-map.component.html',
  styleUrl: './emergency-map.component.scss',
})
export class EmergencyMapComponent implements OnInit, AfterViewInit {
  map!: L.Map;

  constructor(
    private readonly geolocationService: GeolocationService,
    private readonly loaderService: LoaderService
  ) {}

  ngOnInit(): void {
    this.loaderService.setLoading(true);
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
  }

  ngAfterViewInit(): void {
    retryOperation(this.geolocationService.getCurrentLocation, 5, 1)
      .then((location: GeolocationPosition) => {
        this.initMap(location);
        this.loaderService.setLoading(false);
      })
      .catch(() => {
        console.error('Getting location failed, retrying ...');
      });
  }
}
