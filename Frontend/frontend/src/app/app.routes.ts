import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { DiscoverHomeComponent } from './features/discover/pages/discover-home/discover-home.component';
import { FriendsComponent } from './features/friends/pages/friends/friends.component';
import { FriendRequestsComponent } from './features/requests/pages/friend-requests/friend-requests.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path: '',
        component: MainLayoutComponent,
        canActivate: [authGuard],
        children: [
            { path: 'discover-home', component: DiscoverHomeComponent },
            { path: 'friends', component: FriendsComponent },
            { path: 'friend-requests', component: FriendRequestsComponent }
        ]
    },
    {
        path: 'auth',
        component: AuthLayoutComponent,
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'register', component: RegisterComponent }
        ]
    }
];
