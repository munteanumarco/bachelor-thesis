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
