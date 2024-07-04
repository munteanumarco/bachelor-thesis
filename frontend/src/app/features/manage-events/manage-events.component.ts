import { Component, OnInit } from '@angular/core';
import { ImportsModule } from './imports';
import { EmergencyEventDto } from '../../interfaces/emergency/EmergencyEventDto';
import { EmergencyEventService } from '../../services/emergency-event.service';
import { getSeverityName, Severity } from '../../interfaces/emergency/Severity';
import { Status, getStatusName } from '../../interfaces/emergency/Status';
import {
  EmergencyType,
  getEmergencyTypeName,
} from '../../interfaces/emergency/EmergencyType';
import { StorageService } from '../../services/storage.service';

@Component({
  selector: 'app-manage-events',
  standalone: true,
  imports: [ImportsModule],
  providers: [],
  templateUrl: './manage-events.component.html',
  styleUrl: './manage-events.component.scss',
})
export class ManageEventsComponent implements OnInit {
  emergencyEvents!: EmergencyEventDto[];
  loading = false;
  searchValue: string | undefined;

  constructor(
    private readonly emergencyEventService: EmergencyEventService,
    private readonly storageService: StorageService
  ) {}

  ngOnInit() {
    this.emergencyEventService.getParticipatedEvents().subscribe((response) => {
      this.emergencyEvents = response;
    });
  }

  getSeverityPrimeNg(severity: Severity) {
    switch (severity) {
      case Severity.LOW:
        return 'success';

      case Severity.MEDIUM:
        return 'info';

      case Severity.HIGH:
        return 'warning';

      case Severity.CRITICAL:
        return 'danger';

      default:
        return 'info';
    }
  }

  getStatusPrimeNg(status: Status) {
    switch (status) {
      case Status.NEW:
        return 'success';

      case Status.IN_PROGRESS:
        return 'warning';

      case Status.RESOLVED:
        return 'contrast';

      default:
        return 'info';
    }
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
