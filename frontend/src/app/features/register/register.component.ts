import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { passwordValidator } from '../../validators/password_validator';
import { confirmPasswordValidator } from '../../validators/confirm_password_validator';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { LoginGoogleCredentials } from '../../interfaces/auth/LoginGoogleCredentials';
import { CredentialResponse } from 'google-one-tap';
import { environment } from '../../../environments/environment';
import { StorageService } from '../../services/storage.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  errorMessages: string[] = [];

  constructor(
    private readonly authService: AuthService,
    private formBuilder: FormBuilder,
    private messageService: MessageService,
    private storageService: StorageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required, Validators.minLength(5)]],
      password: ['', [Validators.required, passwordValidator()]],
      confirmPassword: [
        '',
        [Validators.required, confirmPasswordValidator('password')],
      ],
    });

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
      { theme: 'outline', size: 'large', class: 'w-full' }
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

  getPasswordErrorMessage() {
    return this.registerForm.get('password')?.hasError('passwordInvalid')
      ? this.registerForm.get('password')?.getError('passwordInvalid').value
      : '';
  }

  onSubmit(): void {
    this.authService
      .registerUser(
        this.registerForm.get('username')?.value,
        this.registerForm.get('email')?.value,
        this.registerForm.get('password')?.value
      )
      .subscribe({
        next: (data) => {
          if (data.isSuccess) {
            this.showSuccessRegisterMessage();
            this.router.navigate(['/check-email']);
          }
        },
        error: (error) => {
          this.errorMessages = error.error.errorMessages;
          console.log(this.errorMessages);
          this.showErrorMessage();
        },
      });
  }

  showSuccessRegisterMessage(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'success',
      summary: 'Creation Successful',
      detail: 'You will receive an email to confirm your account.',
    });
  }

  showErrorMessage(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'error',
      summary: 'Creation Failed',
      detail: this.errorMessages.join('\n'),
    });
  }
}
