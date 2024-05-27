import { Injectable } from '@angular/core';
import { LocalStorageKeys } from '../constants/local-storage-keys';
import { BehaviorSubject } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class StorageService {
  private isLoggedIn = new BehaviorSubject<boolean>(this.currentStatus());
  public isLoggedIn$ = this.isLoggedIn.asObservable();

  constructor() {}

  public logout(): void {
    localStorage.removeItem(LocalStorageKeys.USER_TOKEN);
    this.isLoggedIn.next(false);
  }

  public saveUser(userToken: any): void {
    localStorage.removeItem(LocalStorageKeys.USER_TOKEN);
    localStorage.setItem(LocalStorageKeys.USER_TOKEN, userToken);
    this.isLoggedIn.next(true);
  }

  public currentStatus(): boolean {
    const userToken = localStorage.getItem(LocalStorageKeys.USER_TOKEN);
    return userToken ? true : false;
  }

  public getUserToken(): string | null {
    return localStorage.getItem(LocalStorageKeys.USER_TOKEN);
  }

  public saveUserToken(token: string): void {
    localStorage.setItem(LocalStorageKeys.USER_TOKEN, token);
  }

  public saveAssistantId(assistantId: string): void {
    localStorage.setItem(LocalStorageKeys.ASSISTANT_ID, assistantId);
  }

  public getAssistantId(): string | null {
    return localStorage.getItem(LocalStorageKeys.ASSISTANT_ID);
  }

  public saveThreadId(threadId: string): void {
    localStorage.setItem(LocalStorageKeys.THREAD_ID, threadId);
  }

  public getThreadId(): string | null {
    return localStorage.getItem(LocalStorageKeys.THREAD_ID);
  }

  public getUsername(): string | null {
    const token = this.getUserToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token); // Decode the JWT token
      // Replace the following URL with the claim's URL or key you are interested in
      return decoded[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
      ];
    } catch (error) {
      console.error('Failed to decode token', error);
      return null;
    }
  }
}
