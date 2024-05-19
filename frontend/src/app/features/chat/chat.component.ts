import { Component, Input, OnInit } from '@angular/core';
import { ChatHeaderComponent } from './chat-header/chat-header.component';
import { ChatMessagesComponent } from './chat-messages/chat-messages.component';
import { ChatFooterComponent } from './chat-footer/chat-footer.component';
import { Message } from '../../interfaces/chat/message';
import { MessageType } from '../../interfaces/chat/messageType';
import { SignalRService } from '../../services/signalr.service';

const mockMessages: Message[] = [
  {
    id: '1',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '2',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '3',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john3',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '1',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '2',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '3',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john3',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '1',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '2',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '3',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john3',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '1',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '2',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '3',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john3',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '1',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '2',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '3',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john3',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '1',
    text: "You've been invited to a meeting at Filo's officeYou've been invited to a meeting at Filo's officeYou've been invited to a meeting at Filo's officeYou've been invited to a meeting at Filo's officeYou've been invited to a meeting at Filo's officeYou've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '2',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john2',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
  {
    id: '3',
    text: "You've been invited to a meeting at Filo's office",
    date: new Date(),
    type: MessageType.TEXT,
    user: {
      name: 'john3',
      avatar: 'https://i.gifer.com/no.gif',
    },
  },
];

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [ChatHeaderComponent, ChatMessagesComponent, ChatFooterComponent],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent implements OnInit {
  @Input() messages: Message[] = mockMessages;

  constructor(private readonly signalRService: SignalRService) {}

  ngOnInit(): void {
    this.signalRService.connect();
    this.signalRService.registerMessageReceived((message: any) => {
      this.messages.push(message);
    });
  }

  sendMessage(chatId: string, message: string): void {
    this.signalRService.sendMessage(message, chatId);
  }

  ngOnDestroy(): void {
    this.signalRService.disconnect();
  }
}
