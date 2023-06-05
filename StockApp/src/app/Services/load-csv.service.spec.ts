import { TestBed } from '@angular/core/testing';

import { LoadCSVService } from './load-csv.service';

describe('LoadCSVService', () => {
  let service: LoadCSVService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LoadCSVService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
