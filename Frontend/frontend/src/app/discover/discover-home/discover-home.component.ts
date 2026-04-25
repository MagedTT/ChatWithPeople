import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserCardComponent } from '../user-card/user-card.component';
import { PeopleFilterComponent, FilterState } from '../people-filter/people-filter.component';
// import { DiscoverService } from '../discover.service';
// import { DiscoverStore } from '../discover.store';
// import { User } from '../../shared/models/user.model';

@Component({
  selector: 'app-discover-home',
  imports: [CommonModule, UserCardComponent, PeopleFilterComponent],
  templateUrl: './discover-home.component.html',
  styleUrl: './discover-home.component.css',
})
export class DiscoverHomeComponent implements OnInit {
  // private discoverStore = inject(DiscoverStore);

  // // From store
  // users = this.discoverStore.users;
  // isLoading = this.discoverStore.isLoading;
  // hasError = this.discoverStore.hasError;

  // Local filter state
  filters = signal<FilterState>({
    query: '',
    interest: '',
    sort: 'newest',
  });

  // Derived: filter + sort the user list locally
  filteredUsers = computed(() => {
    const { query, interest, sort } = this.filters();
    // let list = this.users();

    if (query.trim()) {
      const q = query.toLowerCase();
      // list = list.filter(u =>
      //   `${u.firstName} ${u.lastName}`.toLowerCase().includes(q) ||
      //   u.location?.toLowerCase().includes(q) ||
      //   u.interest?.toLowerCase().includes(q)
      // );
    }

    if (interest) {
      // list = list.filter(u => u.interest === interest);
    }

    if (sort === 'online') {
      // list = [...list].sort((a, b) => (b.isOnline ? 1 : 0) - (a.isOnline ? 1 : 0));
    }

    // return list;
  });

  // resultCount = computed(() => this.filteredUsers().length);
  resultCount = 10;

  ngOnInit(): void {
    // this.discoverStore.loadUsers();
  }

  onFiltersChange(f: FilterState): void {
    this.filters.set(f);
  }

  // onAddFriend(user: User): void {
  onAddFriend(user: any): void {
    // this.discoverStore.sendFriendRequest(user.id);
  }

  // onMessage(user: User): void {
  onMessage(user: any): void {
    // this.discoverStore.openChat(user);
  }

  // trackById(_: number, user: User): string {
  trackById(_: number, user: any): string {
    return user.id;
  }
}