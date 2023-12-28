import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { PrimeIcons } from 'primeng/api';
@Component({
  selector: 'app-home',
  standalone: true,
  providers: [MessageService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [
    InputTextModule,
    FormsModule,
    ToastModule,
    HomeComponent,
    ButtonModule,
  ],
})
export class HomeComponent {
  value!: string;
  loading: boolean = false;

  constructor(private messageService: MessageService) {}

  onClick(): void {
    this.loading = true;
    setTimeout(() => {
      this.loading = false;
      this.messageService.add({
        key: 'bc',
        severity: 'success',
        summary: 'This is a message',
        detail: this.value,
      });
    }, 1000);
  }
}
