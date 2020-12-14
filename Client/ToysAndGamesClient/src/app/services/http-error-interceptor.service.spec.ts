import { TestBed } from '@angular/core/testing';
import { RequestErrorInterceptorService } from './request-error-interceptor.service';

describe('HttpErrorInterceptorService', () => {
  let service: RequestErrorInterceptorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RequestErrorInterceptorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
