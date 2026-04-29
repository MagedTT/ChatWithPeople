import { AfterViewChecked, Component, effect, ElementRef, OnDestroy, OnInit, signal, ViewChild } from '@angular/core';
import { ReceivedMessageComponent } from '../../components/received-message/received-message.component';
import { SentMessageComponent } from '../../components/sent-message/sent-message.component';
import { AuthService } from '../../../../core/services/auth.service';
import { ConversationDto, ConversationService } from '../../services/conversation.service';
import { ActivatedRoute } from '@angular/router';
import { MessageSignalRService } from '../../../../core/services/message-signal-r.service';
import { MessageService } from '../../../../core/services/message.service';
import { Subscription } from 'rxjs';
import { MessageDto } from '../../../../shared/models/MessageDto';
import { UserService } from '../../../../core/services/user.service';
import { UserDto } from '../../../../shared/models/UserDto';

@Component({
  selector: 'app-conversation',
  imports: [ReceivedMessageComponent, SentMessageComponent],
  templateUrl: './conversation.component.html',
  styleUrl: './conversation.component.css'
})
export class ConversationComponent implements OnInit, AfterViewChecked, OnDestroy {
  friendId: string = '';
  currentUserId: string | null = null;
  conversation: ConversationDto | null = null;
  messages = signal<MessageDto[]>([]);
  friend: UserDto | null = null;

  private subs = new Subscription();

  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;

  // This runs every time the view updates (e.g., a new message is added)
  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    if (this.scrollContainer) {
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    }
  }

  constructor(
    private conversationService: ConversationService,
    private authService: AuthService,
    private activeRoute: ActivatedRoute,
    private messageSignalRService: MessageSignalRService,
    private userService: UserService,
    private messageService: MessageService) {


    effect(() => {
      this.messages();
      setTimeout(() => this.scrollToBottom(), 0);
    });

    this.activeRoute.params.subscribe(params => this.friendId = params['id']);
    this.currentUserId = this.authService.currentUserId();
  }

  async ngOnInit(): Promise<void> {
    this.userService.getUserById(this.friendId).subscribe(result => {
      this.friend = result;
      console.log("this.friend", this.friend);
    })

    this.conversationService.getOrCreateConversation(this.currentUserId, this.friendId).then(result => {
      this.conversation = result;
      this.loadMessages();
    });

    await this.messageSignalRService.startConnection();
    this.subs.add(
      this.messageSignalRService.messageReceived$.subscribe((messageDto: MessageDto) => {
        // console.log("Message received: ", messageDto);
        this.messages.update(prev =>
          [...prev, messageDto].sort((a, b) =>
            new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
          )
        );
      })
    );
  }

  loadMessages() {
    const messagesLocal = this.messages();

    if (!this.conversation) return;

    if (!messagesLocal || messagesLocal.length === 0) {
      this.messageService
        .getAllMessage(this.conversation.id, null, null)
        .subscribe(result => {
          this.messages.set(result.sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()));
        });
    } else {
      const oldestMessage = messagesLocal.reduce((oldest, current) =>
        new Date(current.createdAt) < new Date(oldest.createdAt)
          ? current
          : oldest
      );

      this.messageService
        .getAllMessage(
          this.conversation.id,
          oldestMessage.id,
          oldestMessage.createdAt
        )
        .subscribe(result => {
          this.messages.set(result.sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()));
        });
    }
  }

  getImageUrl(base64: string | null | undefined): string {
    if (!base64) return 'assets/default.png';
    return `data:image/png;base64,${base64}`;
  }

  async ngOnDestroy(): Promise<void> {
    if (this.subs)
      this.subs.unsubscribe();
    await this.messageSignalRService.stopConnection();
  }
}
