import { TestBed } from '@angular/core/testing';

import { MessageSignalRService } from './message-signal-r.service';

describe('MessageSignalRService', () => {
  let service: MessageSignalRService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MessageSignalRService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
