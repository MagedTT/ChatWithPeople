// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-user-card',
//   imports: [],
//   templateUrl: './user-card.component.html',
//   styleUrl: './user-card.component.css'
// })
// export class UserCardComponent {

// }


import {
  Component,
  input,
  output,
  signal,
  computed,
  ChangeDetectionStrategy,
} from '@angular/core';
import { CommonModule } from '@angular/common';
// import { User } from '../../shared/models/user.model';

@Component({
  selector: 'app-user-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-card.component.html',
  styleUrl: './user-card.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserCardComponent {
  // Inputs (Angular 17+ signal inputs)
  // user = input.required<User>();

  // Outputs
  // addFriend = output<User>();
  // message = output<User>();

  // Local UI state
  requestSent = signal(false);

  initials = computed(() => {
    // const u = this.user();
    // return (u.firstName[0] + u.lastName[0]).toUpperCase();
  });

  onAddFriend(): void {
    if (this.requestSent()) return;
    this.requestSent.set(true);
    // this.addFriend.emit(this.user());
  }

  onMessage(): void {
    // this.message.emit(this.user());
  }
}