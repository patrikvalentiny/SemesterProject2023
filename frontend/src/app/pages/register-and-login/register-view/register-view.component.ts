import {Component, inject, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {AccountService} from "../../../services/account.service";
import {HotToastService} from "@ngneat/hot-toast";

@Component({
  selector: 'app-register-view',
  templateUrl: './register-view.component.html',
  styleUrls: ['./register-view.component.css']
})
export class RegisterViewComponent implements OnInit {
  passwordMatching: boolean = true;
  private readonly accountService: AccountService = inject(AccountService);
  private readonly fb: FormBuilder = inject(FormBuilder);
  registerForm = this.fb.group({
    email: [null, Validators.email],
    username: [null, Validators.required],
    password: [null, Validators.required],
    confirmPassword: [null, Validators.required],

  });
  private readonly toastService = inject(HotToastService);

  async register() {
    if (this.registerForm.value.confirmPassword !== this.registerForm.value.password) {
      this.passwordMatching = false;
      this.registerForm.controls.confirmPassword.reset();
      this.toastService.error("Passwords do not match");
      return
    }

    await this.accountService.register(this.registerForm);

  }

  ngOnInit(): void {

  }
}
