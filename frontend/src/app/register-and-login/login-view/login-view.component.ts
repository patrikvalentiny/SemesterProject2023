import {Component, inject} from '@angular/core';
import {AccountService} from "../account.service";
import {FormBuilder, Validators} from "@angular/forms";

@Component({
    selector: 'app-login-view',
    templateUrl: './login-view.component.html',
    styleUrls: ['./login-view.component.css']
})
export class LoginViewComponent {
    private readonly accountService: AccountService = inject(AccountService);
    private readonly fb: FormBuilder = inject(FormBuilder);
    loginForm = this.fb.group({
        username: [null, Validators.required],
        password: [null, Validators.required]
    });


    async login() {
        await this.accountService.login(this.loginForm.value.username!, this.loginForm.value.password!);
    }
}
