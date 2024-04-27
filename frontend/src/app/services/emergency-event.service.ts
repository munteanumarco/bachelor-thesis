import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { EmergencyEventsRoutes } from '../constants/emergency-events-routes';
import { HttpClient } from '@angular/common/http';
import { CreateEmergencyEventRequest } from '../interfaces/emergency/CreateEmergencyEventRequest';
import { Observable } from 'rxjs';
import { CreateEmergencyEventResponse } from '../interfaces/emergency/CreateEmergencyEventResponse';
import { PaginatedResponse } from '../interfaces/paginatedResponse';
import { EmergencyEventDto } from '../interfaces/emergency/EmergencyEventDto';
import { EmergencyEventMarkersResponse } from '../interfaces/emergency/EmergencyEventMarkersResponse';
import { GetEmergencyEventResponse } from '../interfaces/emergency/GetEmergencyEventResponse';

@Injectable({
  providedIn: 'root',
})
export class EmergencyEventService {
  private baseUrl = `${environment.baseUrl}/${EmergencyEventsRoutes.BASE}`;

  constructor(private readonly http: HttpClient) {}

  reportEmergency(
    data: CreateEmergencyEventRequest
  ): Observable<CreateEmergencyEventResponse> {
    return this.http.post<CreateEmergencyEventResponse>(
      `${this.baseUrl}`,
      data
    );
  }

  getEmergencyEvents(): Observable<EmergencyEventMarkersResponse> {
    return this.http.get<EmergencyEventMarkersResponse>(
      `${this.baseUrl}/${EmergencyEventsRoutes.MARKERS}`
    );
  }

  getEmergencyEvent(eventId: string): Observable<GetEmergencyEventResponse> {
    return this.http.get<GetEmergencyEventResponse>(
      `${this.baseUrl}/${eventId}`
    );
  }
}
