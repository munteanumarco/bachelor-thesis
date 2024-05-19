import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginUserRequest } from '../interfaces/auth/LoginUserRequest';
import { Observable } from 'rxjs';
import { LoginResponse } from '../interfaces/auth/LoginResponse';
import { environment } from '../../environments/environment';
import { RegisterRequest } from '../interfaces/auth/RegisterRequest';
import { RegisterResponse } from '../interfaces/auth/RegisterResponse';
import { ConfirmEmailRequest } from '../interfaces/auth/ConfirmEmailRequest';
import { BaseResponse } from '../interfaces/BaseResponse';
import { UserRoutes } from '../constants/user-routes';
import { ResetPasswordRequest } from '../interfaces/auth/ResetPasswordRequest';
import { LoginGoogleCredentials } from '../interfaces/auth/LoginGoogleCredentials';
import { ApiGatewayServices } from '../constants/api-gateway-services';
import { convertToObject } from 'typescript';
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
