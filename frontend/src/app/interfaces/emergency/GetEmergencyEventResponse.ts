import { BaseResponse } from '../BaseResponse';
import { EmergencyEventDto } from './EmergencyEventDto';

export interface GetEmergencyEventResponse extends BaseResponse {
  emergencyEvent: EmergencyEventDto;
}
