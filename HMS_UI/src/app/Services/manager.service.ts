import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ManagerService {

  private apiUrl = 'https://localhost:44359/api/manager';

  constructor(private http: HttpClient) { }

  getRooms(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/room`);
  }

  getRoom(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/room/${id}`);
  }

  createRoom(room: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/room`, room);
  }

  updateRoom(id: number, room: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/room/${id}`, room);
  }

  deleteRoom(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/room/${id}`);
  }

  getStaffs(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/staff`);
  }

  getStaff(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/staff/${id}`);
  }

  createStaff(staffDto: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/staff`, staffDto);
  }

  updateStaff(id: number, staffDto: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/staff/${id}`, staffDto);
  }

  deleteStaff(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/staff/${id}`);
  }
}
