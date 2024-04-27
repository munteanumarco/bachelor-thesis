import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmergencyEventService } from '../../services/emergency-event.service';
import { EmergencyEventDto } from '../../interfaces/emergency/EmergencyEventDto';
import { AsyncPipe, DatePipe } from '@angular/common';
import { emergencyTypeNames } from '../../interfaces/emergency/EmergencyType';
import { statusNames } from '../../interfaces/emergency/Status';
import { severityNames } from '../../interfaces/emergency/Severity';

@Component({
  selector: 'app-emergency-details',
  standalone: true,
  imports: [AsyncPipe, DatePipe],
  templateUrl: './emergency-details.component.html',
  styleUrl: './emergency-details.component.scss',
})
export class EmergencyDetailsComponent {
  event!: EmergencyEventDto;
  emergencyTypeNames = emergencyTypeNames;
  statusNames = statusNames;
  severityNames = severityNames;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly emergencyEventService: EmergencyEventService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const eventId = params['eventId'];
      this.fetchEmergencyDetails(eventId);
    });
  }

  fetchEmergencyDetails(eventId: string): void {
    this.emergencyEventService
      .getEmergencyEvent(eventId)
      .subscribe((response) => {
        this.event = response.emergencyEvent;
      });
  }
}
