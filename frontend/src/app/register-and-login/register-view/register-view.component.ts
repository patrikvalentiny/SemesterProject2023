import {Component, inject, OnInit} from '@angular/core';
import {FormBuilder, FormsModule, Validators} from "@angular/forms";
import {AccountService} from "../account.service";

@Component({
  selector: 'app-register-view',
  templateUrl: './register-view.component.html',
  styleUrls: ['./register-view.component.css']
})
export class RegisterViewComponent implements OnInit{
  readonly accountService:AccountService = inject(AccountService);
  readonly fb: FormBuilder = inject(FormBuilder);

  registerForm = this.fb.group({
    username:[null, Validators.required],
    password:[null, Validators.required],
    confirmPassword:[null, Validators.required],
    email:[null, Validators.email],
    firstName:[null],
    lastName:[null],
  });

  passwordMatching:boolean = true;
  register() {
    if (this.registerForm.value.confirmPassword !== this.registerForm.value.password){
      this.passwordMatching = false;
      this.registerForm.controls.confirmPassword.reset();
      return
    }

    this.accountService.register(this.registerForm);

  }

  ngOnInit(): void {

  }
}
