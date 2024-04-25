import { EmergencyType } from './EmergencyType';
import { Severity } from './Severity';

export interface CreateEmergencyEventRequest {
  description?: string;
  location: string;
  latitude: number;
  longitude: number;
  severity: Severity;
  type: EmergencyType;
}
