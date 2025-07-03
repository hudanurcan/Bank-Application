import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5121/api/auth'; // Backend API URL

  constructor(private http: HttpClient) {}

  login(tc: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { tc, password });
  }
}
