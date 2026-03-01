import { Component , signal,effect} from '@angular/core';
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


  botReply=signal('')
  userMessage = '';
  chatHistory: { user: string; bot: string }[] = [];
  isLoading = false;

  constructor(private chatService: ChatService) {

    // effect(()=>{
    //   console.log(this.botReply)

    // })


  }

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
          this.botReply.set(res.reply);
          console.log(res.reply);
        },
        error: () => {
          this.chatHistory[this.chatHistory.length - 1].bot = 'Error occurred';
          this.isLoading = false;
        }
      });
      
    this.botReply.set('');


  }
}
