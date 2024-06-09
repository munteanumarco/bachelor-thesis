import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ApiGatewayServices } from '../constants/api-gateway-services';
import { Observable } from 'rxjs';
import { AnalysisRoutes } from '../constants/analysis-routes';

@Injectable({
  providedIn: 'root',
})
export class AnalysisService {
  private baseUrl = `${environment.apiGateway}/${ApiGatewayServices.ANALYSIS}/${AnalysisRoutes.BASE}`;

  constructor(private http: HttpClient) {}

  getUp() {
    return this.http.get(`${this.baseUrl}/${AnalysisRoutes.UP}`);
  }
}
