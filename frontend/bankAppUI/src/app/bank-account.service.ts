import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BankAccountService {
  private baseUrl = 'http://localhost:5121/api/Account';  // Backend API URL

  constructor(private http: HttpClient) {}

    // Hesap kapatma
    deactivateAccount(userId: string | null): Observable<any> {
      return this.http.post<any>(`${this.baseUrl}/deactivate/${userId}`, {});  // Deactivation API çağrısı
    }  
}
