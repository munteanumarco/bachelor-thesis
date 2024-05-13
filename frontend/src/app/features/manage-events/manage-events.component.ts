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
import { Table } from 'primeng/table';

const mockEmergencyEvents: EmergencyEventDto[] = [
  {
    id: '1',
    description: 'Fire in the building',
    location: 'Building 1',
    latitude: 1.2345,
    longitude: 1.2345,
    severity: Severity.HIGH,
    status: Status.NEW,
    type: EmergencyType.FIRE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'John Doe',
  },
  {
    id: '2',
    description: 'Flood in the building',
    location: 'Building 2',
    latitude: 1.2345,
    longitude: 1.2345,
    severity: Severity.LOW,
    status: Status.IN_PROGRESS,
    type: EmergencyType.FLOOD,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'Jane Doe',
  },
  {
    id: '3',
    description: 'Earthquake in the building',
    location: 'Building 3',
    latitude: 1.2345,
    longitude: 1.2345,
    severity: Severity.MEDIUM,
    status: Status.RESOLVED,
    type: EmergencyType.EARTHQUAKE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'John Doe',
  },
  {
    id: '4',
    description: 'Fire in the building',
    location: 'Building 1',
    latitude: 1.2345,
    longitude: 1.2345,
    severity: Severity.CRITICAL,
    status: Status.NEW,
    type: EmergencyType.FIRE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'John Doe',
  },
];

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

  constructor(private readonly emergencyEventService: EmergencyEventService) {}

  ngOnInit() {
    this.emergencyEvents = mockEmergencyEvents;
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

  clear(table: Table) {
    table.clear();
    this.searchValue = '';
  }
}
