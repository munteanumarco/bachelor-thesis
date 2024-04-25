import { BaseResponse } from '../BaseResponse';
import { EmergencyEventDto } from './EmergencyEventDto';

export interface CreateEmergencyEventResponse extends BaseResponse {
  emergencyEvent: EmergencyEventDto;
}
