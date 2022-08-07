import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, tap } from "rxjs";
import { SpinnerService } from "./spinner.service";

@Injectable()
export class HttpInterceptorSpinner implements HttpInterceptor{

  constructor(private spinService: SpinnerService) {

  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
    this.spinService.requestStarted()
    return this.handler(next, request)
  }

  handler(next: HttpHandler, request: HttpRequest<any>){
    return next.handle(request).pipe(
      tap(
        (event) => {
          if (event instanceof HttpResponse){
            console.log('me acabe XD')
            this.spinService.requestEnded()
          }
        (error: HttpErrorResponse) => {
          this.spinService.resetSpinner
          throw error;
         }
        }
      )
    )
  }
}