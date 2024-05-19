import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat-footer',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './chat-footer.component.html',
  styleUrl: './chat-footer.component.scss',
})
export class ChatFooterComponent {
  @Output() messageSend = new EventEmitter<string>();

  message: string = '';
  sendMessage() {
    if (this.message.trim()) {
      this.messageSend.emit(this.message);
      this.message = '';
    }
  }
}
