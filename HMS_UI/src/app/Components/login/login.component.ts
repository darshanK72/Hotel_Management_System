import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  
  loginForm: FormGroup;
  serverError = false;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  loginUser(): void {
    if (this.loginForm.valid) {
      console.log('Login details:', this.loginForm.value);
      const { username, password } = this.loginForm.value;
      this.authService.loginUser(username, password).subscribe(
        () => {
          console.log('Login successful'); // Handle successful login
          // Navigate to home component after successful login
          this.router.navigate(['/home']); // Adjust '/home' to your actual route
        },
        error => {
          // console.error('Login failed', error); // Handle login error
          this.serverError = true;
        }
      );
    } else {
      // Mark all fields as touched to display validation messages
      this.loginForm.markAllAsTouched();
    }
  }

  logoutUser(): void {
    this.authService.logoutUser();
    console.log('Logged out'); // Handle logout actions
    // Optionally navigate to another page upon logout
  }
}
