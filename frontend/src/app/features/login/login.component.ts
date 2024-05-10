import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { StorageService } from '../../services/storage.service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { FormsModule } from '@angular/forms';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';
import { environment } from '../../../environments/environment';
import { LoginGoogleCredentials } from '../../interfaces/auth/LoginGoogleCredentials';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent implements OnInit {
  identifier!: string;
  password!: string;
  errorMessages: string[] = [];

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private router: Router,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.loadGoogleApi();
  }

  loadGoogleApi() {
    const script = document.createElement('script');
    script.src = 'https://accounts.google.com/gsi/client';
    script.onload = () => {
      this.initializeGoogleButton();
    };
    document.head.appendChild(script);
  }

  initializeGoogleButton() {
    google.accounts.id.initialize({
      client_id: environment.oAuthClientId,
      callback: this.handleCredentialResponse.bind(this),
      auto_select: false,
      cancel_on_tap_outside: true,
    });

    google.accounts.id.renderButton(
      // @ts-ignore
      document.getElementById('google-button'),
      { theme: 'outline', size: 'large' }
    );

    google.accounts.id.prompt((notification: any) => {
      // Handle the visibility of the prompt
    });
  }

  handleCredentialResponse(response: CredentialResponse) {
    const data: LoginGoogleCredentials = { credentials: response.credential };
    this.authService.loginWithGoogle(data).subscribe({
      next: (data) => {
        if (data.isSuccess && data.token) {
          this.storageService.saveUser(data.token);
          this.router.navigate(['/']).then(() => {
            window.location.reload();
          });
        }
      },
      error: (error) => {
        this.errorMessages = error.error.errorMessages;
        console.log(this.errorMessages);
        this.showErrorMessage();
      },
    });
  }

  onSubmit(): void {
    this.authService.loginUser(this.identifier, this.password).subscribe({
      next: (data) => {
        if (data.isSuccess && data.token) {
          this.storageService.saveUser(data.token);
          this.router.navigate(['/']);
        }
      },
      error: (error) => {
        this.errorMessages = error.error.errorMessages;
        console.log(this.errorMessages);
        this.showErrorMessage();
      },
    });
  }

  showErrorMessage(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'error',
      summary: 'Login Failed',
      detail: this.errorMessages.join('\n'),
    });
  }
}
