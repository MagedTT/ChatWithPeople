import { Component, OnInit } from '@angular/core';
import { PeopleFilterComponent } from '../../components/people-filter/people-filter.component';
import { UserCardComponent } from '../../components/user-card/user-card.component';
import { DiscoverService } from '../../services/discover.service';
import { UserForDiscover } from '../../models/UserForDiscover.model';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-discover-home',
  imports: [PeopleFilterComponent, UserCardComponent],
  templateUrl: './discover-home.component.html',
  styleUrl: './discover-home.component.css'
})
export class DiscoverHomeComponent implements OnInit {
  usersForDiscover?: UserForDiscover[];

  constructor(private authService: AuthService, private discoverService: DiscoverService) { }

  ngOnInit(): void {
    this.discoverService.getAllUsersForDiscover(this.authService.currentUserId()).subscribe(result => {
      this.usersForDiscover = result;
    });
  }

}
