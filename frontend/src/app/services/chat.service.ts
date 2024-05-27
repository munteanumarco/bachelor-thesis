import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiGatewayServices } from '../constants/api-gateway-services';
import { CommunicationRoutes } from '../constants/communication-routes';
import { GetChatDetailsResponse } from '../interfaces/chat/GetChatDetailsResponse';
import { GetChatMessagesResponse } from '../interfaces/chat/GetChatMessagesResponse';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private baseUrl = `${environment.apiGateway}/${ApiGatewayServices.COMMUNICATION}/${CommunicationRoutes.BASE}`;

  constructor(private http: HttpClient) {}

  getChatDetails(eventId: string): Observable<GetChatDetailsResponse> {
    return this.http.get<GetChatDetailsResponse>(
      `${this.baseUrl}/${eventId}/details`
    );
  }

  getChatMessages(chatId: string): Observable<GetChatMessagesResponse> {
    return this.http.get<GetChatMessagesResponse>(
      `${this.baseUrl}/${chatId}/messages`
    );
  }
}
