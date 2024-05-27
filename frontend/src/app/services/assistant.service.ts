import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ApiGatewayServices } from '../constants/api-gateway-services';
import { AssistantRoutes } from '../constants/assistant-routes';
import { StorageService } from './storage.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateAssistantResponse } from '../interfaces/assistant/CreateAssistantResponse';
import { CreateThreadResponse } from '../interfaces/assistant/CreateThreadResponse';
import { GetThreadMessagesResponse } from '../interfaces/assistant/GetThreadMessagesResponse';

@Injectable({
  providedIn: 'root',
})
export class AssistantService {
  private baseUrl = `${environment.apiGateway}/${ApiGatewayServices.ASSISTANT}`;

  constructor(
    private http: HttpClient,
    private storageService: StorageService
  ) {}

  createAssistant(): Observable<CreateAssistantResponse> {
    return this.http.post<CreateAssistantResponse>(
      `${this.baseUrl}/${AssistantRoutes.ASSISTANTS}`,
      {}
    );
  }

  createThread(): Observable<CreateThreadResponse> {
    return this.http.post<CreateThreadResponse>(
      `${this.baseUrl}/${AssistantRoutes.THREADS}`,
      {}
    );
  }

  getThreadMessages(threadId: string): Observable<GetThreadMessagesResponse> {
    return this.http.get<GetThreadMessagesResponse>(
      `${this.baseUrl}/threads/${threadId}/messages`
    );
  }

  createMessage(message: string): Observable<GetThreadMessagesResponse> {
    const assistantId = this.storageService.getAssistantId();
    const threadId = this.storageService.getThreadId();
    if (!assistantId || !threadId) {
      throw new Error('Assistant ID or Thread ID is missing');
    }
    const body = {
      userQuestion: message,
    };
    return this.http.post<GetThreadMessagesResponse>(
      `${this.baseUrl}/assistants/${assistantId}/threads/${threadId}`,
      body
    );
  }

  deleteThread(threadId: string) {
    return this.http.delete(`${this.baseUrl}/threads/${threadId}`);
  }

  deleteAssistant(assistantId: string) {
    return this.http.delete(`${this.baseUrl}/assistants/${assistantId}`);
  }
}
