import { Routes } from '@angular/router';
import { DiscoverHomeComponent } from './discover/discover-home/discover-home.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { authGuard } from './shared/guards/auth.guard';
import { MainLayoutComponent } from './shared/components/layout/main-layout/main-layout.component';
import { AuthLayoutComponent } from './shared/components/layout/auth-layout/auth-layout.component';

export const routes: Routes = [
    {
        path: '',
        component: MainLayoutComponent,
        canActivate: [authGuard],
        children: [
            { path: 'discover-home', component: DiscoverHomeComponent }
        ]
    },
    {
        path: '',
        component: AuthLayoutComponent,
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'resiger', component: RegisterComponent },
        ]
    },
    // { path: 'discover-home', component: MainLayoutComponent, canActivate: [authGuard] },
    // { path: 'login', component: AuthLayoutComponent },
    // { path: 'register', component: AuthLayoutComponent },
    { path: 'auth/register', component: RegisterComponent },
];
