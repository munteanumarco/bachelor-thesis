import { EmergencyType } from './EmergencyType';
import { Severity } from './Severity';
import { Status } from './Status';

export interface EmergencyEventMarkerDto {
  id: string;
  severity: Severity;
  type: EmergencyType;
  status: Status;
  latitude: number;
  longitude: number;
  updatedAt: Date;
}
