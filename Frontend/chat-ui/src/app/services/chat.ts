import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private apiUrl = 'https://localhost:7101/Chat'; // <-- your .NET API base URL

  constructor(private http: HttpClient) { }

  // Call POST /Chat
  sendMessage(message: string): Observable<any> {
    return this.http.post(this.apiUrl, JSON.stringify(message), {
  headers: { 'Content-Type': 'application/json' }
});



  }



  getChatHistory() {
  return this.http.get<any[]>(`${this.apiUrl}`);
}
}
