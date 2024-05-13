import { Component, Input } from '@angular/core';
import { Message } from '../../../interfaces/chat/message';
import { MessageCardComponent } from '../message-card/message-card.component';

@Component({
  selector: 'app-chat-messages',
  standalone: true,
  imports: [MessageCardComponent],
  templateUrl: './chat-messages.component.html',
  styleUrl: './chat-messages.component.scss',
})
export class ChatMessagesComponent {
  @Input() messages!: Message[];
}
