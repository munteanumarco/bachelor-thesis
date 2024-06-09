import {
  AfterViewChecked,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ChatHeaderComponent } from './chat-header/chat-header.component';
import { ChatMessagesComponent } from './chat-messages/chat-messages.component';
import { ChatFooterComponent } from './chat-footer/chat-footer.component';
import { Message } from '../../interfaces/chat/message';
import { SignalRService } from '../../services/signalr.service';
import { ChatService } from '../../services/chat.service';
import { ActivatedRoute } from '@angular/router';
import { ChatDetailsDto } from '../../interfaces/chat/ChatDetailsDto';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [ChatHeaderComponent, ChatMessagesComponent, ChatFooterComponent],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss',
})
export class ChatComponent implements OnInit, OnDestroy {
  messages!: Message[];
  eventId!: string;
  chatDetails!: ChatDetailsDto;

  constructor(
    private readonly signalRService: SignalRService,
    private readonly chatService: ChatService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.signalRService.connect();

    this.route.queryParams.subscribe((params) => {
      this.eventId = params['eventId'];
    });
    this.chatService.getChatDetails(this.eventId).subscribe((response) => {
      this.chatDetails = response.data;
      this.chatService
        .getChatMessages(this.chatDetails.id)
        .subscribe((response) => {
          this.messages = response.chatMessages;
        });
    });

    this.signalRService.registerMessageReceived(
      this.onMessageReceived.bind(this)
    );
  }

  handleSendMessage(message: string) {
    this.signalRService.sendMessage(message, this.chatDetails.id);
  }

  onMessageReceived(message: Message): void {
    console.log('Message received:', message);
    this.messages.push(message);
  }

  ngOnDestroy(): void {
    this.signalRService.unregisterMessageReceived(
      this.onMessageReceived.bind(this)
    );
    this.signalRService.disconnect();
  }
}
