import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmergencyEventService } from '../../services/emergency-event.service';
import { EmergencyEventDto } from '../../interfaces/emergency/EmergencyEventDto';
import { AsyncPipe, DatePipe } from '@angular/common';
import {
  EmergencyType,
  getEmergencyTypeName,
} from '../../interfaces/emergency/EmergencyType';
import { getStatusName, Status } from '../../interfaces/emergency/Status';
import { getSeverityName, Severity } from '../../interfaces/emergency/Severity';
import { MessageService } from 'primeng/api';
import { EmergencyDetailsDto } from '../../interfaces/emergency/EmergencyDetailsDto';
import { StorageService } from '../../services/storage.service';

@Component({
  selector: 'app-emergency-details',
  standalone: true,
  imports: [AsyncPipe, DatePipe],
  templateUrl: './emergency-details.component.html',
  styleUrl: './emergency-details.component.scss',
})
export class EmergencyDetailsComponent {
  event!: EmergencyDetailsDto;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly emergencyEventService: EmergencyEventService,
    private readonly messageService: MessageService,
    private readonly storageService: StorageService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const eventId = params['eventId'];
      this.fetchEmergencyDetails(eventId);
    });
  }

  fetchEmergencyDetails(eventId: string): void {
    this.emergencyEventService
      .getEmergencyEventDetails(eventId)
      .subscribe((response) => {
        this.event = response.details;
      });
  }

  addParticipant(): void {
    this.emergencyEventService.addParticipant(this.event.id).subscribe(
      () => {
        this.fetchEmergencyDetails(this.event.id);
        this.showSuccessParticipation();
      },
      (err) => {
        this.showErrorMessages(err.error.errorMessages);
      }
    );
  }

  showSuccessParticipation(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'success',
      summary: 'Success',
      detail: 'You are now participating in this emergency event',
    });
  }

  showErrorMessages(message: string): void {
    this.messageService.add({
      key: 'bc',
      severity: 'error',
      summary: 'Error',
      detail: message,
    });
  }

  isCurrentUserParticipant(): boolean {
    return this.event.participantsUsernames.some(
      (participant) => participant === this.storageService.getUsername()
    );
  }

  getEmergencyTypeName(type: EmergencyType): string {
    return getEmergencyTypeName(type);
  }

  getStatusName(status: Status): string {
    return getStatusName(status);
  }

  getSeverityName(severity: Severity): string {
    return getSeverityName(severity);
  }
}
