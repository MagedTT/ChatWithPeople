import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit() {
    console.log('clicked');
    const username = this.loginForm.value.username;
    const password = this.loginForm.value.password;
    this.authService.login({ username, password }).subscribe((result) => {
      console.log(`Access Token: ${result.accessToken}`);
      console.log(`Refresh Token: ${result.refreshToken}`);
    });
  }
}
