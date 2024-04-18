import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm!: FormGroup;

  constructor(
    private readonly authService: AuthService,
    private readonly formBuilder: FormBuilder,
    private readonly messageService: MessageService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.forgotPasswordForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit(): void {
    this.authService
      .forgotPassword(this.forgotPasswordForm.get('email')?.value)
      .subscribe({
        next: () => {
          this.showMessage();
          this.router.navigate(['/check-email']);
        },
        error: () => {
          this.showMessage();
          this.router.navigate(['/check-email']);
        },
      });
  }

  showMessage(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'success',
      summary: 'Password Reset',
      detail:
        'If there is an account associated with this email, a password reset link has been sent.',
    });
  }
}
