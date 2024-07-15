import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { User } from '../Models/user.model';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:44359/api/auth';

  private loggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.hasToken());
  private loggedInUser: BehaviorSubject<any> = new BehaviorSubject<any>(this.getUserDetails());

  constructor(private http: HttpClient, private router: Router) {}

  get isLoggedIn$(): Observable<boolean> {
    return this.loggedIn.asObservable();
  }

  get loggedInUser$(): Observable<any> {
    return this.loggedInUser.asObservable();
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('token');
  }

  private getUserDetails(): any {
    const userData = localStorage.getItem('user');
    if (userData) {
      return JSON.parse(userData);
    }
    return null;
  }


  loginUser(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { username, password }).pipe(
      tap(response => {
        const token = response.token;
        if (token) {
          localStorage.setItem('token', token);
          this.loggedIn.next(true);
          const decodedToken = jwtDecode<DecodedToken>(token);
          const user = {
            name: decodedToken.name,
            role: decodedToken.role
          };
          this.loggedInUser.next(user);
          localStorage.setItem('user', JSON.stringify(user));
        }
      })
    );
  }

  registerUser(user: User): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, user);
  }

  logoutUser(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.loggedIn.next(false);
    this.loggedInUser.next(null);
    this.router.navigate(['/login']);
  }

  forgetPassword(email: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/forgetPassword`, { email });
  }
}

interface DecodedToken {
  name: string;
  role: string;
}
