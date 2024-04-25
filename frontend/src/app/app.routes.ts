import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/login/login.component';
import { RegisterComponent } from './features/register/register.component';
import { guestGuard } from './guards/guest.guard';
import { ConfirmEmailComponent } from './features/confirm-email/confirm-email.component';
import { ForgotPasswordComponent } from './features/forgot-password/forgot-password.component';
import { CheckEmailComponent } from './shared/check-email/check-email.component';
import { ResetPasswordComponent } from './features/reset-password/reset-password.component';
import { ReportEmergencyComponent } from './features/report-emergency/report-emergency.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { userGuard } from './guards/user.guard';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    title: 'Home',
  },
  {
    path: 'login',
    component: LoginComponent,
    title: 'Login',
    canActivate: [guestGuard],
  },
  {
    path: 'register',
    component: RegisterComponent,
    title: 'Register',
    canActivate: [guestGuard],
  },
  {
    path: 'confirm-email',
    component: ConfirmEmailComponent,
    title: 'Confirm Email',
    canActivate: [guestGuard],
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent,
    title: 'Forgot Password',
    canActivate: [guestGuard],
  },
  {
    path: 'check-email',
    component: CheckEmailComponent,
    title: 'Check Email',
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent,
    title: 'Reset Password',
    canActivate: [guestGuard],
  },
  {
    path: 'report-emergency',
    component: ReportEmergencyComponent,
    title: 'Report Emergency',
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    title: 'Dashboard',
    canActivate: [userGuard],
  },
];
