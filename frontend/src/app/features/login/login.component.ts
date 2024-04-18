import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { StorageService } from '../../services/storage.service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  identifier!: string;
  password!: string;
  errorMessages: string[] = [];

  constructor(
    private authService: AuthService,
    private storageService: StorageService,
    private router: Router,
    private messageService: MessageService
  ) {}

  onSubmit(): void {
    this.authService.loginUser(this.identifier, this.password).subscribe({
      next: (data) => {
        if (data.isSuccess && data.token) {
          this.storageService.saveUser(data.token);
          this.showSuccessLoginMessage();
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

  showSuccessLoginMessage(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'success',
      summary: 'Login Successful',
      detail: 'You have successfully logged in!',
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
