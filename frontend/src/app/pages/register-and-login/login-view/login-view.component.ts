import {Component, inject, OnInit} from '@angular/core';
import {AccountService} from "../../../services/account.service";
import { FormBuilder, Validators, ReactiveFormsModule } from "@angular/forms";
import {StatusService} from "../../../services/status.service";
import {NgClass} from "@angular/common";

@Component({
    selector: 'app-login-view',
    templateUrl: './login-view.component.html',
    styleUrls: ['./login-view.component.css'],
    standalone: true,
  imports: [ReactiveFormsModule, NgClass]
})
export class LoginViewComponent implements OnInit {
  private readonly accountService: AccountService = inject(AccountService);
  private readonly fb: FormBuilder = inject(FormBuilder);
  private readonly statusService: StatusService = inject(StatusService);

  loginForm = this.fb.group({
    username: [null, Validators.required],
    password: [null, [Validators.required, Validators.minLength(4)]]
  });

  constructor() {
  }
  async login() {
    try {
      await this.accountService.login(this.loginForm.value.username!, this.loginForm.value.password!);
    } catch (e) {
      return;
    }
  }

  async ngOnInit(){
    await this.statusService.isRunning();
  }
}
