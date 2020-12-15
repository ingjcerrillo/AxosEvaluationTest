import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import HttpStatusCodes from './http-status-codes.enum';

@Injectable({
  providedIn: 'root'
})
export class RequestErrorInterceptorService implements HttpInterceptor {

  constructor(private notifications:MatSnackBar) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err:HttpErrorResponse) => {
        switch(err.status){
          case HttpStatusCodes.NOT_FOUND:
            this.NotificarError('We couldnt find the item you are looking for');
            break;
          case HttpStatusCodes.INTERNAL_SERVER_ERROR:
            this.NotificarError('An error ocurred in the server while processing your request. Operation was cancelled.');
            break;
          case HttpStatusCodes.BAD_REQUEST:
            if(err.error){
              if(err.error.errors){
                this.processServerValidationErrors(err.error.errors);
              }
            }
            break;
          case HttpStatusCodes.REQUEST_TIMEOUT:
            this.NotificarError('Execution time limit was reached. Check your connection or try again later');
            break;
          default:
            this.NotificarError('An unknown error occured. Please try again later')
            break;
        }

        return throwError(err.status);
      })
    );
  }

  private processServerValidationErrors(errors : any){
    let propertyNames = Object.getOwnPropertyNames(errors);
    let errorList :  any[];
    if(propertyNames){
      propertyNames.forEach(p => {
        let errorList = errors[p];
        errorList.forEach((e: string) => {
          this.NotificarError(e);
        });
      });
    }
  }

  private NotificarAdvertencia(mensaje:string) : void {
    this.notifications.open(mensaje, '', {
      duration: 2000,
      politeness: 'polite'
    });
  }

  private NotificarError(error:string) : void {
    this.notifications.open(error, '', {
      duration: 2000,
      politeness: 'polite'
    });
  }
}
