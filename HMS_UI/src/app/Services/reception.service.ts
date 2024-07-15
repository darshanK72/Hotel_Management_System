import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReceptionistService {
  private apiUrl = 'https://localhost:44359/api/receptionist';

  constructor(private http: HttpClient) { }

  searchGuests(name?: string, email?: string, phoneNumber?: string, memberCode?: string): Observable<any[]> {
    let params = new HttpParams();
    if (name) params = params.append('name', name);
    if (email) params = params.append('email', email);
    if (phoneNumber) params = params.append('phoneNumber', phoneNumber);
    if (memberCode) params = params.append('memberCode', memberCode);

    return this.http.get<any[]>(`${this.apiUrl}/guest/search`, { params });
  }

  getGuest(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/guest/${id}`);
  }

  createGuest(guestDto: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/guest`, guestDto);
  }

  updateGuest(id: number, guestDto: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/guest/${id}`, guestDto);
  }

  deleteGuest(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/guest/${id}`);
  }

  getReservations(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/reservations`);
  }

  getReservation(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/reservation/${id}`);
  }

  createReservation(request: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/reservation/create`, request);
  }

  completePayment(request: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/reservation/complete-payment`, request);
  }

  updateReservation(id: number, reservation: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/reservation/${id}`, reservation);
  }

  deleteReservation(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/reservation/${id}`);
  }

  getRooms(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/rooms`);
  }
}