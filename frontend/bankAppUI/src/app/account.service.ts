import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {


  private apiUrl = 'http://localhost:5121/api/Account';  // API URL

  constructor(private http: HttpClient) {}

  // Kullanıcı ID'sine göre bakiye sorgulama
  getBalance(): Observable<number> {
    const userId = localStorage.getItem('userId');
    if (!userId) {
      throw new Error('Kullanıcı girişi yapılmamış');
    }
    return this.http.get<number>(`${this.apiUrl}/get-balance/${userId}`);
  }

  // Para yatırma işlemi dinamik olarak
  // deposit(userId: number, amount: number): Observable<number> {
  //   return this.http.post<number>(`${this.apiUrl}/deposit/${userId}`, { amount });
  deposit(userId: number, amount: number): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}/deposit/${userId}`, { amount, userId });
  }

  // Para çekme işlemi dinamik olarak
  withdraw(userId: number, amount: number): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}/withdraw/${userId}`, { amount, userId });
  }

    // // Kullanıcıya ait banka hesaplarını al
    getBankAccounts(userId: number): Observable<any> {
      return this.http.get(`${this.apiUrl}/get-cards/${userId}`);
   }

   
   // İşlem geçmişini çekmek için metod
   getTransactionHistory(userId: number): Observable<any[]> {
    const token = localStorage.getItem('token');  // Token'ı localStorage'dan al
    if (!token) {
      console.error("Token eksik");
      return new Observable<any[]>(); // Token yoksa boş bir observable döndür
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    // Transaction endpoint'ini kullanarak işlem geçmişini alıyoruz
    return this.http.get<any[]>(`http://localhost:5121/api/Transaction/history/${userId}`, { headers });
  }
}



