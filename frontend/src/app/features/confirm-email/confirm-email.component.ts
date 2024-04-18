import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MessageService } from 'primeng/api';
import { ConfirmEmailRequest } from '../../interfaces/auth/ConfirmEmailRequest';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss'],
})
export class ConfirmEmailComponent implements OnInit {
  token!: string | null;
  email!: string | null;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
    private readonly messageService: MessageService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.token = params['token'];
      this.email = params['email'];
    });
  }

  confirmEmail(): void {
    const data: ConfirmEmailRequest = {
      email: encodeURIComponent(this.email!),
      token: encodeURIComponent(this.token!),
    };
    this.authService.confirmEmail(data).subscribe({
      next: () => {
        this.showSuccessMessage();
        this.router.navigate(['/login']);
      },
      error: () => {
        this.showErrorMessages();
      },
    });
  }

  showSuccessMessage(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'success',
      summary: 'Email Confirmed',
      detail: 'You have successfully confirmed your email!',
    });
  }

  showErrorMessages(): void {
    this.messageService.add({
      key: 'bc',
      severity: 'error',
      summary: 'Email Confirmation Failed',
      detail: 'There was an error confirming your email. Please try again.',
    });
  }
}
