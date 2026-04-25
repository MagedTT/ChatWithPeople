// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-people-filter',
//   imports: [],
//   templateUrl: './people-filter.component.html',
//   styleUrl: './people-filter.component.css'
// })
// export class PeopleFilterComponent {

// }
import {
  Component,
  output,
  signal,
  computed,
  effect,
  ChangeDetectionStrategy,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

export interface FilterState {
  query: string;
  interest: string;
  sort: 'newest' | 'online';
}

const INTERESTS = [
  'Photography', 'Design', 'Music', 'Travel',
  'Tech', 'Cooking', 'Gaming', 'Art',
];

const SORT_OPTIONS: { label: string; value: FilterState['sort'] }[] = [
  { label: 'Newest', value: 'newest' },
  { label: 'Online first', value: 'online' },
];

@Component({
  selector: 'app-people-filter',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './people-filter.component.html',
  styleUrl: './people-filter.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PeopleFilterComponent {
  filtersChange = output<FilterState>();

  // Internal state — each field is its own signal
  query = signal('');
  interest = signal('');
  sort = signal<FilterState['sort']>('newest');

  // Panel visibility
  filterPanelOpen = signal(false);

  readonly interests = INTERESTS;
  readonly sortOptions = SORT_OPTIONS;

  hasActiveFilters = computed(
    () => !!this.interest() || this.sort() !== 'newest'
  );

  activeFilterCount = computed(() => {
    let n = 0;
    if (this.interest()) n++;
    if (this.sort() !== 'newest') n++;
    return n;
  });

  constructor() {
    // Emit whenever any signal changes
    effect(() => {
      this.filtersChange.emit({
        query: this.query(),
        interest: this.interest(),
        sort: this.sort(),
      });
    });
  }

  setInterest(value: string): void {
    this.interest.set(this.interest() === value ? '' : value);
  }

  setSort(value: FilterState['sort']): void {
    this.sort.set(value);
  }

  clearAll(): void {
    this.query.set('');
    this.interest.set('');
    this.sort.set('newest');
    this.filterPanelOpen.set(false);
  }

  togglePanel(): void {
    this.filterPanelOpen.update(v => !v);
  }
}