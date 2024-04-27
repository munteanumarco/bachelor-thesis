import { BaseResponse } from '../BaseResponse';
import { EmergencyEventMarkerDto } from './EmergencyEventMarkerDto';

export interface EmergencyEventMarkersResponse extends BaseResponse {
  markers: EmergencyEventMarkerDto[];
}
