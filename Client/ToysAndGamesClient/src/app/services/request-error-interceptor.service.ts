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
            this.NotificarError('No se encontró el elemento que se deseaba modificar.');
            break;
          case HttpStatusCodes.INTERNAL_SERVER_ERROR:
            this.NotificarError('Ocurrió un problema en el servidor al intentar procesar la solicitud. Operación cancelada.');
            break;
          case HttpStatusCodes.BAD_REQUEST:
            if(err.error){
              if(err.error.errors){
                this.processServerValidationErrors(err.error.errors);
              }
            }
            break;
          case HttpStatusCodes.REQUEST_TIMEOUT:
            this.NotificarError('El tiempo de espera para procesar la solicitud expiró. Compruebe su conexión y vuelva a intentar más tarde.');
            break;
          default:
            this.NotificarError('Ocurió un error no identificado.')
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
