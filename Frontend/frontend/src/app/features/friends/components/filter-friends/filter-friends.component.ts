import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filter-friends',
  imports: [FormsModule],
  templateUrl: './filter-friends.component.html',
  styleUrl: './filter-friends.component.css'
})
export class FilterFriendsComponent {
  @Output() sendSearchTerm = new EventEmitter<string>();
  searchTerm: string = '';
}
