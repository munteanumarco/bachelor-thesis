// src/app/web-socket.service.ts
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { StorageService } from './storage.service';
import { environment } from '../../environments/environment';
import { ApiGatewayServices } from '../constants/api-gateway-services';
import { AnalysisRoutes } from '../constants/analysis-routes';

@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  private socket!: WebSocket;
  private listener: Observable<any>;


  constructor(private storageService: StorageService) { 
    const userId = this.storageService.getUserId();
    const url = `${environment.apiGateway}/${ApiGatewayServices.ANALYSIS}/${AnalysisRoutes.BASE}/ws/${userId}`;
    this.listener = new Observable((subscriber) => {
      this.socket = new WebSocket(url);

      this.socket.onmessage = (event) => {
        subscriber.next(event.data);
      };

      // this.socket.onerror = (event) => {
      //   subscriber.error(event);
      // };

      // this.socket.onclose = (event) => {
      //   subscriber.complete();
      // };

      // return () => {
      //   this.socket.close();
      // };
    });
  }

  public get messages(): Observable<any> {
    return this.listener;
  }

  public send(msg: string): void {
    if (this.socket.readyState === WebSocket.OPEN) {
      this.socket.send(msg);
    } else {
      console.error("WebSocket is not open.");
    }
  }
}
