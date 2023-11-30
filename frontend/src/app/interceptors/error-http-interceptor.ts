import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {inject, Injectable} from "@angular/core";
import {catchError, Observable} from "rxjs";
import {HotToastService} from "@ngneat/hot-toast";

@Injectable()
export class ErrorHttpInterceptor implements HttpInterceptor {
    private readonly toastService = inject(HotToastService);

    intercept(req: HttpRequest<any>, next: HttpHandler):
        Observable<HttpEvent<any>> {
        return next.handle(req).pipe(catchError(async e => {
            if (e instanceof HttpErrorResponse) {
                this.showError(e.statusText);
            }
            throw e;
        }));
    }

    private async showError(message: string) {
        return this.toastService.error(message, {})
    }
}
