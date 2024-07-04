import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { EmergencyEventsRoutes } from '../constants/emergency-events-routes';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CreateEmergencyEventRequest } from '../interfaces/emergency/CreateEmergencyEventRequest';
import { Observable } from 'rxjs';
import { CreateEmergencyEventResponse } from '../interfaces/emergency/CreateEmergencyEventResponse';
import { PaginatedResponse } from '../interfaces/paginatedResponse';
import { EmergencyEventDto } from '../interfaces/emergency/EmergencyEventDto';
import { EmergencyEventMarkersResponse } from '../interfaces/emergency/EmergencyEventMarkersResponse';
import { GetEmergencyEventResponse } from '../interfaces/emergency/GetEmergencyEventResponse';
import { BaseResponse } from '../interfaces/BaseResponse';
import { ApiGatewayServices } from '../constants/api-gateway-services';
import { GetEmergencyDetailsResponse } from '../interfaces/emergency/GetEmergencyDetailsResponse';

@Injectable({
  providedIn: 'root',
})
export class EmergencyEventService {
  private baseUrl = `${environment.apiGateway}/${ApiGatewayServices.ENGINE}/${EmergencyEventsRoutes.BASE}`;

  constructor(private readonly http: HttpClient) {}

  reportEmergency(
    data: CreateEmergencyEventRequest
  ): Observable<CreateEmergencyEventResponse> {
    return this.http.post<CreateEmergencyEventResponse>(
      `${this.baseUrl}`,
      data
    );
  }

  getEmergencyEventsMarkers(): Observable<EmergencyEventMarkersResponse> {
    return this.http.get<EmergencyEventMarkersResponse>(
      `${this.baseUrl}/${EmergencyEventsRoutes.MARKERS}`
    );
  }

  getEmergencyEventDetails(
    eventId: string
  ): Observable<GetEmergencyDetailsResponse> {
    return this.http.get<GetEmergencyDetailsResponse>(
      `${this.baseUrl}/${eventId}`
    );
  }

  getEmergencyEvents(
    pageSize: number,
    pageNumber: number,
    userId?: string
  ): Observable<PaginatedResponse<EmergencyEventDto>> {
    let params = new HttpParams()
      .set('pageSize', pageSize.toString())
      .set('pageNumber', pageNumber.toString());
    if (userId) {
      params = params.set('userId', userId);
    }
    return this.http.get<PaginatedResponse<EmergencyEventDto>>(
      `${this.baseUrl}`,
      { params }
    );
  }

  getParticipatedEvents() {
    return this.http.get<EmergencyEventDto[]>(
      `${this.baseUrl}/${EmergencyEventsRoutes.PARTICIPATED}`
    );
  }

  addParticipant(eventId: string): Observable<BaseResponse> {
    return this.http.post<BaseResponse>(
      `${this.baseUrl}/${eventId}/${EmergencyEventsRoutes.PARTICIPANTS}`,
      {}
    );
  }
}
