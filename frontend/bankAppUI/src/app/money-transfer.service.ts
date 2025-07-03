import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MoneyTransferService {
 
  private apiUrl = 'http://localhost:5121/api/transaction/transfer'; // API url'inizi buraya ekleyin

  constructor(private http: HttpClient) {}

  // Para transferi yapmak için bir fonksiyon
  transferMoney(transaction: any): Observable<any> {
    // Token'ı localStorage'dan alıyoruz
    const token = localStorage.getItem('token');

    if (!token) {
      console.error("Token eksik.");
      return new Observable<any>(); // Token yoksa boş bir observable döndür
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`, // Token'ı Authorization başlığına ekliyoruz
      'Content-Type': 'application/json'  // JSON formatında veri gönderiyoruz
    });

    // POST isteğini gönderiyoruz
    return this.http.post(this.apiUrl, transaction, { headers: headers });
  }
}
