import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatService } from './services/chat';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class AppComponent {

  userMessage = '';
  chatHistory: { user: string; bot: string }[] = [];
  isLoading = false;

  constructor(private chatService: ChatService) {}

  sendMessage() {
    if (!this.userMessage.trim()) return;

    // show user message immediately
    this.chatHistory.push({
      user: this.userMessage,
      bot: 'Typing...'
    });

    this.isLoading = true;

    this.chatService.sendMessage(this.userMessage)
      .subscribe({
        next: (res) => {
          this.chatHistory[this.chatHistory.length - 1].bot = res.reply;
          this.isLoading = false;
          console.log(res.reply);
        },
        error: () => {
          this.chatHistory[this.chatHistory.length - 1].bot = 'Error occurred';
          this.isLoading = false;
        }
      });

    this.userMessage = '';
  }
}
