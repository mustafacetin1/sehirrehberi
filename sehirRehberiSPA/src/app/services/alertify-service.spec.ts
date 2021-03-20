/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AlertifyServiceService } from './alertify-service.service';

describe('Service: AlertifyService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AlertifyServiceService]
    });
  });

  it('should ...', inject([AlertifyServiceService], (service: AlertifyServiceService) => {
    expect(service).toBeTruthy();
  }));
});
