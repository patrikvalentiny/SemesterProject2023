import {Component, inject, OnInit} from '@angular/core';

import {AccountService} from "../../services/account.service";
import {NavigationEnd, Router} from "@angular/router";
import {User} from "../../dtos/user";

@Component({
    selector: 'app-home-skeleton',
    templateUrl: './home-skeleton.component.html',
    styleUrls: ['./home-skeleton.component.css']
})
export class HomeSkeletonComponent implements OnInit {
    readonly accountService = inject(AccountService);
    readonly router = inject(Router);
    public user: User | null = null;
    public isHidden= false;

    async ngOnInit() {
        this.accountService.tokenService.getToken();
        this.router.events.subscribe((value) => {
          if (value instanceof NavigationEnd) {
            this.isHidden = value.url.includes("login") || value.url.includes("register") || value.url.includes("onboarding");
          }
          }
        );

    }

    async logout() {
        this.user = null;
        await this.accountService.logout();
    }
}
