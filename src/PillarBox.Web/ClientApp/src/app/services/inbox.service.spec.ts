import { TestBed, inject } from '@angular/core/testing';

import { InboxService } from './inbox.service';

describe('InboxService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InboxService]
    });
  });

  it('should be created', inject([InboxService], (service: InboxService) => {
    expect(service).toBeTruthy();
  }));
});
