import { Component } from '@angular/core';
import { NavbarComponent } from '../../navbar/navbar.component';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { DiscoverHomeComponent } from '../../../../discover/discover-home/discover-home.component';

@Component({
  selector: 'app-main-layout',
  imports: [
    RouterOutlet,
    NavbarComponent,
    SidebarComponent,
  ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css'
})
export class MainLayoutComponent {

}
