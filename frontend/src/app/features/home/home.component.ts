import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { HomeService } from './home.service';
@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [InputTextModule, FormsModule, ButtonModule],
})
export class HomeComponent implements OnInit {
  value!: string;
  loading: boolean = false;

  constructor(
    private messageService: MessageService,
    private homeService: HomeService
  ) {}

  ngOnInit(): void {
    console.log('HomeComponent.ngOnInit()');
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
