import { EmergencyType } from './EmergencyType';
import { Severity } from './Severity';
import { Status } from './Status';

export interface EmergencyEventDto {
  id: string;
  description: string;
  location: string;
  latitude: number;
  longitude: number;
  severity: Severity;
  status: Status;
  type: EmergencyType;
  createdAt: Date;
  updatedAt: Date;
}
