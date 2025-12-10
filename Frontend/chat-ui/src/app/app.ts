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
  chatHistory: { sender: string, text: string }[] = [];

  constructor(private chatService: ChatService) {}

  sendMessage() {
    if (!this.userMessage.trim()) return;

    this.chatHistory.push({ sender: 'user', text: this.userMessage });

    this.chatService.sendMessage(this.userMessage).subscribe({
      next: res => {
        this.chatHistory.push({ sender: 'assistant', text: res.reply });
      },
      error: err => {
        this.chatHistory.push({ sender: 'assistant', text: 'Error: API not responding' });
      }
    });

    this.userMessage = '';
  }
}
