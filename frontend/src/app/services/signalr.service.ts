import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private connection: signalR.HubConnection;

  constructor(private storageService: StorageService) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.chatUrl, {
        accessTokenFactory: () => this.getJwtToken(),
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

  private getJwtToken(): string {
    let token = this.storageService.getUserToken() || '';
    return token.replace(/^"|"$/g, '');
  }

  public async connect(): Promise<void> {
    try {
      await this.connection.start();
    } catch (err) {
      console.error('SignalR Connection Error: ', err);
    }
  }

  public async sendMessage(message: string, chatId: string): Promise<void> {
    try {
      await this.connection.invoke('SendMessageToChatGroup', chatId, message);
    } catch (err) {
      console.error('SignalR Send Message Error: ', err);
    }
  }

  public registerMessageReceived(callback: (message: any) => void): void {
    this.connection.on('ReceiveMessage', callback);
  }

  public unregisterMessageReceived(callback: (message: any) => void): void {
    this.connection.off('ReceiveMessage', callback);
  }

  public async disconnect(): Promise<void> {
    try {
      await this.connection.stop();
    } catch (err) {
      console.error('SignalR Disconnect Error: ', err);
    }
  }
}
