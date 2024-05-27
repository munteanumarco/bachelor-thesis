import { Component, OnInit } from '@angular/core';
import { ChatFooterComponent } from '../chat/chat-footer/chat-footer.component';
import { ChatMessagesComponent } from '../chat/chat-messages/chat-messages.component';
import { ChatHeaderComponent } from '../chat/chat-header/chat-header.component';
import { ChatDetailsDto } from '../../interfaces/chat/ChatDetailsDto';
import { Message } from '../../interfaces/chat/message';
import { StorageService } from '../../services/storage.service';
import { AssistantService } from '../../services/assistant.service';
import { LoaderService } from '../../services/loader.service';
import { EnhancedMessage } from '../../interfaces/chat/EnhancedMessage';

@Component({
  selector: 'app-assistant',
  standalone: true,
  imports: [ChatHeaderComponent, ChatMessagesComponent, ChatFooterComponent],
  templateUrl: './assistant.component.html',
  styleUrl: './assistant.component.scss',
})
export class AssistantComponent implements OnInit {
  chatDetails: ChatDetailsDto = {
    id: 'mock-id',
    name: 'Assistant Chat',
    participantsCount: 0,
  };

  isLoading: boolean = false;

  messages!: EnhancedMessage[];

  constructor(
    private storageService: StorageService,
    private assistantService: AssistantService
  ) {}

  ngOnInit(): void {
    const assistantId = this.storageService.getAssistantId();
    const threadId = this.storageService.getThreadId();
    if (assistantId && threadId) {
      this.messages = [];
      this.assistantService
        .getThreadMessages(threadId)
        .subscribe((response) => {
          response.data.forEach((message) => {
            console.log(message);
            this.messages.push({
              id: message.id,
              text: message.content[0].text.value,
              date: new Date(message.created_at * 1000),
              username:
                message.role === 'assistant'
                  ? 'Assistant'
                  : this.storageService.getUsername() || '',
              created_at: message.created_at,
            });
          });
          this.sortMessages();
        });
    } else {
      this.assistantService.createAssistant().subscribe((response) => {
        this.storageService.saveAssistantId(response.id);
        this.assistantService.createThread().subscribe((response) => {
          this.storageService.saveThreadId(response.id);
        });
      });
    }
  }

  sortMessages() {
    this.messages.sort((a, b) => a.created_at - b.created_at);
  }

  handleSendMessage(message: string) {
    this.isLoading = true;
    this.assistantService.createMessage(message).subscribe((response) => {
      this.messages = [];
      response.data.forEach((message) => {
        this.messages.push({
          id: message.id,
          text: message.content[0].text.value,
          date: new Date(message.created_at * 1000),
          username:
            message.role === 'assistant'
              ? 'Assistant'
              : this.storageService.getUsername() || '',
          created_at: message.created_at,
        });
      });
      this.sortMessages();
      this.isLoading = false;
    });
  }
}
