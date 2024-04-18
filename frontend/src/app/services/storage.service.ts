import { Injectable } from '@angular/core';
import { LocalStorageKeys } from '../constants/local-storage-keys';
import { BehaviorSubject } from 'rxjs';

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
}
