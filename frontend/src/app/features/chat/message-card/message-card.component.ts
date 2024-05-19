import { Component, Input, OnInit } from '@angular/core';
import { Message } from '../../../interfaces/chat/message';
import { StorageService } from '../../../services/storage.service';

@Component({
  selector: 'app-message-card',
  standalone: true,
  imports: [],
  templateUrl: './message-card.component.html',
  styleUrl: './message-card.component.scss',
})
export class MessageCardComponent implements OnInit {
  @Input() message!: Message;
  currentUser!: string;

  constructor(private readonly storageService: StorageService) {}
  ngOnInit(): void {
    this.currentUser = this.storageService.getUsername() || '';
  }

  formattedDate(date: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      hour: 'numeric',
      minute: 'numeric',
      hour12: true,
    }).format(new Date(date));
  }
}
