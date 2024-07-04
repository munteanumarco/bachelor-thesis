import { BaseResponse } from '../BaseResponse';
import { EmergencyDetailsDto } from './EmergencyDetailsDto';

export interface GetEmergencyDetailsResponse extends BaseResponse {
  details: EmergencyDetailsDto;
}
