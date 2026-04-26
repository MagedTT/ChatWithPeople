import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { AuthService } from '../../../core/services/auth.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup;
  constructor(private authService: AuthService, private router: Router, private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  navigateToRegisterPage() {
    this.router.navigateByUrl('auth/register');
  }

  onSubmit() {
    this.authService.login({ userName: this.loginForm.value.userName, password: this.loginForm.value.password }).subscribe(result => {
      // console.log(`Access Token: ${result.accessToken}`);
      // console.log(`Refresh Token: ${result.refreshToken}`);
    });
  }
}
