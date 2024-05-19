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

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = `${environment.apiGateway}/${ApiGatewayServices.ENGINE}/${UserRoutes.BASE}`;

  constructor(private http: HttpClient) {}

  loginUser(
    userIdentifier: string,
    password: string
  ): Observable<LoginResponse> {
    const user: LoginUserRequest = {
      userIdentifier,
      password,
    };
    return this.http.post<LoginResponse>(
      `${this.baseUrl}/${UserRoutes.LOGIN}`,
      user
    );
  }

  registerUser(
    username: string,
    email: string,
    password: string
  ): Observable<RegisterResponse> {
    const user: RegisterRequest = {
      username,
      email,
      password,
    };
    return this.http.post<RegisterResponse>(
      `${this.baseUrl}/${UserRoutes.REGISTER}`,
      user
    );
  }

  confirmEmail(data: ConfirmEmailRequest): Observable<BaseResponse> {
    return this.http.post<BaseResponse>(
      `${this.baseUrl}/${UserRoutes.CONFIRM_EMAIL}`,
      data
    );
  }

  forgotPassword(email: string): Observable<BaseResponse> {
    return this.http.post<BaseResponse>(
      `${this.baseUrl}/${UserRoutes.FORGOT_PASSWORD}`,
      { email }
    );
  }

  resetPassword(data: ResetPasswordRequest): Observable<BaseResponse> {
    return this.http.post<BaseResponse>(
      `${this.baseUrl}/${UserRoutes.RESET_PASSWORD}`,
      data
    );
  }

  loginWithGoogle(
    credentials: LoginGoogleCredentials
  ): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.baseUrl}/${UserRoutes.GOOGLE_LOGIN}`,
      credentials
    );
  }
}
