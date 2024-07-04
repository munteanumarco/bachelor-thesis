import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ApiGatewayServices } from '../constants/api-gateway-services';
import { Observable } from 'rxjs';
import { AnalysisRoutes } from '../constants/analysis-routes';
import { LandCoverAnalysisDto } from '../interfaces/analysis/LandcoverAnalysisDto';
import { StartAnalysisResponse } from '../interfaces/analysis/StartAnalysisResponse';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root',
})
export class AnalysisService {
  private baseUrl = `${environment.apiGateway}/${ApiGatewayServices.ANALYSIS}/${AnalysisRoutes.BASE}`;

  constructor(private http: HttpClient, private storageService: StorageService) {}

  getUp() {
    return this.http.get(`${this.baseUrl}/${AnalysisRoutes.UP}`);
  }

  getAnalysis(emergencyEventId: string): Observable<LandCoverAnalysisDto> {
    return this.http.get<LandCoverAnalysisDto>(
      `${this.baseUrl}/${emergencyEventId}`
    );
  }

  startAnalysis(
    emergencyEventId: string,
    latitude: number,
    longitude: number
  ): Observable<StartAnalysisResponse> {
    const body = {
      latitude: latitude,
      longitude: longitude,
      client_id: this.storageService.getUserId(), 
    };
    return this.http.post<StartAnalysisResponse>(
      `${this.baseUrl}/${emergencyEventId}`,
      body
    );
  }
}
