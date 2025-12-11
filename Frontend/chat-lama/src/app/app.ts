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
  chatHistory: { user: string, bot: string }[] = [];

  constructor(private chatService: ChatService) {}

  ngOnInit() {
    this.loadHistory();

    // OPTIONAL: auto refresh every 2 seconds
    setInterval(() => this.loadHistory(), 2000);
  }

  loadHistory() {
    this.chatService.getChatHistory().subscribe(res => {
      this.chatHistory = res;
    });
  }



  sendMessage() {
          if (!this.userMessage.trim()) return;

            // Push user message to UI
            this.chatHistory.push({
              user: this.userMessage,
              bot: '...'   // temporary placeholder
            });

            // Index of the last message we just pushed
            const index = this.chatHistory.length - 1;

            // Call API
            this.chatService.sendMessage(this.userMessage).subscribe({
              next: (res) => {
                // Update the bot reply in the same chat item
                this.chatHistory[index].bot = res.reply;

                // OPTIONAL: load full history from backend instead of local UI
                this.loadHistory();
              },
              error: () => {
                this.chatHistory[index].bot = 'Error: API not responding';
              }
            });

            this.userMessage = '';

  }


  




  

}
