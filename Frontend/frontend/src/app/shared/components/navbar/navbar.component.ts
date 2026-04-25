import { Component, inject, signal, computed, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';
// import { AuthStore } from '../../shared/store/auth.store';
// import { NotificationStore } from '../../shared/store/notification.store';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  constructor(private authService: AuthService) { }
  // private authStore = inject(AuthStore);
  // private notificationStore = inject(NotificationStore);

  // Signals
  notifPanelOpen = signal(false);
  userMenuOpen = signal(false);

  // // Derived from stores
  // currentUser = this.authStore.currentUser;
  // unreadCount = this.notificationStore.unreadCount;
  // notifications = this.notificationStore.notifications;

  userInitials = computed(() => {
    // const user = this.currentUser();
    // if (!user) return '';
    // return (user.firstName[0] + user.lastName[0]).toUpperCase();
  });

  // hasUnread = computed(() => this.unreadCount() > 0);

  navLinks = [
    { label: 'Discover', path: '/discover' },
    { label: 'Friends', path: '/friends' },
    { label: 'Groups', path: '/groups' },
    { label: 'Requests', path: '/requests' },
  ];

  toggleNotifPanel(): void {
    this.notifPanelOpen.update(v => !v);
    this.userMenuOpen.set(false);
    if (this.notifPanelOpen()) {
      // this.notificationStore.markAllRead();
    }
  }

  toggleUserMenu(): void {
    this.userMenuOpen.update(v => !v);
    this.notifPanelOpen.set(false);
  }

  logout(): void {
    this.authService.logout();
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.notif-trigger') && !target.closest('.notif-panel')) {
      this.notifPanelOpen.set(false);
    }
    if (!target.closest('.user-menu-trigger') && !target.closest('.user-dropdown')) {
      this.userMenuOpen.set(false);
    }
  }
}