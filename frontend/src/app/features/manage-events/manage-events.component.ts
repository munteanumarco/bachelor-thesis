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
    id: '4047779c-80d2-48f3-a201-9e55393f78d0',
    description: 'Fire in a residential area',
    location: 'Cluj-Napoca, Central Park',
    latitude: 46.769379,
    longitude: 23.5899542,
    severity: Severity.HIGH,
    status: Status.NEW,
    type: EmergencyType.FIRE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'John Doe',
  },
  {
    id: '2',
    description: 'Flood near a riverbank',
    location: 'Timisoara, Bega River',
    latitude: 45.7488716,
    longitude: 21.2086793,
    severity: Severity.LOW,
    status: Status.IN_PROGRESS,
    type: EmergencyType.FLOOD,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'Jane Doe',
  },
  {
    id: '3',
    description: 'Earthquake reported',
    location: 'Bucharest, Sector 1',
    latitude: 44.497841,
    longitude: 26.065451,
    severity: Severity.MEDIUM,
    status: Status.RESOLVED,
    type: EmergencyType.EARTHQUAKE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'John Doe',
  },
  {
    id: '4',
    description: 'Fire in an office building',
    location: 'Iasi, Palas Complex',
    latitude: 47.1584549,
    longitude: 27.6014418,
    severity: Severity.CRITICAL,
    status: Status.NEW,
    type: EmergencyType.FIRE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'John Doe',
  },
  {
    id: '5',
    description: 'Industrial accident with chemicals',
    location: 'Constanta, Industrial Area',
    latitude: 44.180737,
    longitude: 28.634302,
    severity: Severity.HIGH,
    status: Status.NEW,
    type: EmergencyType.FIRE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'Jane Doe',
  },
  {
    id: '6',
    description: 'Severe flood accident',
    location: 'Brasov, Strada Lunga',
    latitude: 45.64861,
    longitude: 25.60613,
    severity: Severity.HIGH,
    status: Status.IN_PROGRESS,
    type: EmergencyType.FLOOD,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'John Doe',
  },
  {
    id: '7',
    description: 'Building collapse',
    location: 'Sibiu, Piata Mare',
    latitude: 45.79833,
    longitude: 24.12558,
    severity: Severity.CRITICAL,
    status: Status.NEW,
    type: EmergencyType.EARTHQUAKE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'Jane Doe',
  },
  {
    id: '8',
    description: 'Forest fire near residential area',
    location: 'Piatra Neamt, Cozla Park',
    latitude: 46.92938,
    longitude: 26.37095,
    severity: Severity.CRITICAL,
    status: Status.NEW,
    type: EmergencyType.FIRE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'John Doe',
  },
  {
    id: '9',
    description: 'Gas leak in public building',
    location: 'Craiova, Electroputere Mall',
    latitude: 44.31406,
    longitude: 23.79488,
    severity: Severity.HIGH,
    status: Status.IN_PROGRESS,
    type: EmergencyType.FIRE,
    reportedAt: new Date(),
    updatedAt: new Date(),
    reportedBy: 'Jane Doe',
  },
  {
    id: '10',
    description: 'Chemical spill in factory',
    location: 'Arad, Industrial Park West',
    latitude: 46.16667,
    longitude: 21.31667,
    severity: Severity.HIGH,
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
