import { Injectable } from '@angular/core';
import L from 'leaflet';

@Injectable({
  providedIn: 'root'
})
export class MarkerService {
  private markers: L.Marker[] = [];

  addMarker(marker: L.Marker): void {
    this.markers.push(marker);
  }

  getMarkers(): L.Marker[] {
    return this.markers;
  }

  clearMarkers(): void {
    this.markers.forEach(marker => marker.remove());
    this.markers = [];
  }
}
