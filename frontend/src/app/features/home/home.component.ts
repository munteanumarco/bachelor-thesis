import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { PrimeIcons } from 'primeng/api';
import { HomeService } from './home.service';
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
export class HomeComponent implements OnInit {
  value!: string;
  loading: boolean = false;

  constructor(
    private messageService: MessageService,
    private homeService: HomeService
  ) {}

  ngOnInit(): void {
    this.homeService.getUsers().subscribe((users) => {
      console.log(users);
    });
  }

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
