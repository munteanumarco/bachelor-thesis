import { Component, Input } from '@angular/core';
import { Message } from '../../../interfaces/chat/message';

@Component({
  selector: 'app-message-card',
  standalone: true,
  imports: [],
  templateUrl: './message-card.component.html',
  styleUrl: './message-card.component.scss',
})
export class MessageCardComponent {
  @Input() message!: Message;
  currentUser = 'john2';

  formattedDate(date: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      hour: 'numeric',
      minute: 'numeric',
      hour12: true,
    }).format(new Date(date));
  }
}
