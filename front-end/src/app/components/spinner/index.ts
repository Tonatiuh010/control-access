import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { HttpInterceptorSpinner } from "./http-interceptor";

export const httpInterceptProviders = [
  {provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorSpinner, multi: true}
];
