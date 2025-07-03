import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
import { tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserServiceService {
  private apiUrl = 'http://localhost:5121/api/user/register';  
  private baseUrl = 'http://localhost:5121/api/user/profile/'; 

  constructor(private http: HttpClient) {}

  // Kullanıcı kaydı işlemi
  register(user: any): Observable<any> {
    return this.http.post(this.apiUrl, user, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json' // JSON formatında veri gönderiyoruz
      })
    });
  }

  // Profil bilgilerini almak için
  getUserProfile(): Observable<any> {
    // localStorage'ın tarayıcıda olup olmadığını kontrol ediyoruz
    if (typeof window === 'undefined' || !window.localStorage) {
      console.error("localStorage tarayıcıda tanımlı değil.");
      return throwError(() => new Error('localStorage is not available.'));
    }

    const token = localStorage.getItem('token');  // Token'ı localStorage'dan alıyoruz
    const userId = localStorage.getItem('userId');  // Kullanıcı ID'sini localStorage'dan alıyoruz

    console.log("Gönderilen Token:", token);  // Token'ı logluyoruz
    console.log("Frontend'den alınan Kullanıcı ID:", userId);  // Kullanıcı ID'sini logluyoruz

    // Eğer token veya userId eksikse hata veriyoruz
    if (!token || !userId) {
      console.error("Token veya userId eksik, yetkilendirme yapılamaz.");
      return throwError(() => new Error('Token veya userId bulunamadı.'));
    }

    // API'ye başlıkları ekleyerek istek yapıyoruz
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    // URL'yi dinamik olarak oluşturuyoruz
    const url = `${this.baseUrl}${userId}`;

    return this.http.get<any>(url, { headers }).pipe(
      tap(profile => console.log("Backend’den Alınan Profil Bilgisi:", profile)), // Profil bilgisi loglanıyor
      catchError(error => {
        console.error("Profil bilgisi alınırken hata oluştu:", error);
        return throwError(() => new Error('Profil bilgisi alınamadı.'));
      })
    );
  }

}
