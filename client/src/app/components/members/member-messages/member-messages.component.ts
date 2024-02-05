import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
  imports: [CommonModule, TimeagoModule, FormsModule]
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() messages: Message[] = [];
  @Input() username?: string;
  loading = false;
  messageContent = '';

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
  }

  sendMessage() {
    if (!this.username) return;
    this.loading = true;
    this.messageService.sendMessage(this.username, this.messageContent).subscribe({
      next: message => {
        this.messages.push(message);
        this.messageForm?.reset();
        this.loading = false;
      }
    })
  }
}
