import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OwnerService {
  private apiUrl = 'https://localhost:44359/api/owner';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/user`);
  }

  getUser(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/user/${id}`);
  }

  createUser(userPayload: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/user/register`, userPayload);
  }

  updateUser(id: number, userPayload: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/user/update/${id}`, userPayload);
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/user/delete/${id}`);
  }

  getDepartments(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/departments`);
  }

  getDepartment(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/department/${id}`);
  }

  createDepartment(department: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/department`, department);
  }

  updateDepartment(id: number, department: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/department/${id}`, department);
  }

  deleteDepartment(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/department/${id}`);
  }

  getReport(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/report`);
  }
}