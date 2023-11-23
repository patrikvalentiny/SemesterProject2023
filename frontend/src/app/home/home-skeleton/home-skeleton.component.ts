import {Component, inject, OnInit} from '@angular/core';
import {User} from "../../user";
import {AccountService} from "../../register-and-login/account.service";

@Component({
    selector: 'app-home-skeleton',
    templateUrl: './home-skeleton.component.html',
    styleUrls: ['./home-skeleton.component.css']
})
export class HomeSkeletonComponent implements OnInit {
    readonly accountService = inject(AccountService);
    public user: User | null = null;

    async ngOnInit() {
        this.accountService.tokenService.getToken();
    }

    async logout() {
        this.user = null;
        await this.accountService.logout();
    }
}
