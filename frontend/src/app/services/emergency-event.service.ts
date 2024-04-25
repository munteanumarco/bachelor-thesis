import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { EmergencyEventsRoutes } from '../constants/emergency-events-routes';
import { HttpClient } from '@angular/common/http';
import { CreateEmergencyEventRequest } from '../interfaces/emergency/CreateEmergencyEventRequest';
import { Observable } from 'rxjs';
import { CreateEmergencyEventResponse } from '../interfaces/emergency/CreateEmergencyEventResponse';

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
}
