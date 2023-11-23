import {inject, Injectable} from '@angular/core';
import {Router} from "@angular/router";
import {TokenService} from "./token.service";

@Injectable({
    providedIn: 'root'
})
export class AuthGuardService {
    tokenService: TokenService = inject(TokenService);
    router = inject(Router);

    canActivate() {
        if (this.tokenService.getToken() === null) {
            this.router.navigate(["/login"]);
            return false;
        }
        return true;
    }

}
