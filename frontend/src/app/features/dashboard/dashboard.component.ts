import { Component } from '@angular/core';
import { EmergencyMapComponent } from '../../shared/emergency-map/emergency-map.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [EmergencyMapComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {}
