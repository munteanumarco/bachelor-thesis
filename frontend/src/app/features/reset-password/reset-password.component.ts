import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
import { passwordValidator } from '../../validators/password_validator';
import { confirmPasswordValidator } from '../../validators/confirm_password_validator';
import { ResetPasswordRequest } from '../../interfaces/auth/ResetPasswordRequest';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss',
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm!: FormGroup;
  token!: string | null;
  email!: string | null;

  constructor(
    private readonly authService: AuthService,
    private readonly formBuilder: FormBuilder,
    private readonly messageService: MessageService,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.token = params['token'];
      this.email = params['email'];
    });
    this.resetPasswordForm = this.formBuilder.group({
      password: ['', [Validators.required, passwordValidator()]],
      confirmPassword: [
        '',
        [Validators.required, confirmPasswordValidator('password')],
      ],
    });
  }

  onSubmit(): void {
    const data: ResetPasswordRequest = {
      email: this.email!,
      token: encodeURIComponent(this.token!),
      password: this.resetPasswordForm.get('password')?.value,
      confirmPassword: this.resetPasswordForm.get('confirmPassword')?.value,
    };

    this.authService.resetPassword(data).subscribe({
      next: () => {
        this.showSuccessMessage();
        this.router.navigate(['/login']);
      },
      error: () => {
        this.showErrorMessage();
      },
    });
  }

  getPasswordErrorMessage() {
    return this.resetPasswordForm.get('password')?.hasError('passwordInvalid')
      ? this.resetPasswordForm.get('password')?.getError('passwordInvalid')
          .value
      : '';
  }

  showSuccessMessage(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'success',
      summary: 'Password Reset Successfully',
      detail: 'You have successfully reset your password.',
    });
  }

  showErrorMessage(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'error',
      summary: 'Password Reset Failed',
      detail: 'There was an error. Please try again.',
    });
  }
}
