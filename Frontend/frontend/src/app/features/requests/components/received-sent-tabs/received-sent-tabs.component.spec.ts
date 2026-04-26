import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceivedSentTabsComponent } from './received-sent-tabs.component';

describe('ReceivedSentTabsComponent', () => {
  let component: ReceivedSentTabsComponent;
  let fixture: ComponentFixture<ReceivedSentTabsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReceivedSentTabsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReceivedSentTabsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
