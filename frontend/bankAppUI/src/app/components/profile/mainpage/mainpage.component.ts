import { Component, OnDestroy, OnInit } from '@angular/core';
import { SidebarComponent } from "../sidebar/sidebar.component";
import { UserServiceService } from '../../../user.service.service';
import { AccountService } from '../../../account.service';
import { Router } from '@angular/router';
import { ExchangeRateService } from '../../../exchange-rate.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-mainpage',
  standalone: true,
  imports: [SidebarComponent, CommonModule],
  templateUrl: './mainpage.component.html',
  styleUrl: './mainpage.component.css'
})
export class MainpageComponent implements OnInit, OnDestroy{

  userName: string = 'Yükleniyor...';
  userSurname: string = '';
  balance: number = 0; 
  currentTime: string = '';
  usdToTry: number = 0;  // USD kuru
  eurToTry: number = 0;  // EUR kuru

  private timer: any;
  constructor(private userService: UserServiceService, 
              private accountService : AccountService, 
              private router: Router, 
              private exchangeRateService: ExchangeRateService) {}

  ngOnDestroy(): void {
    if (this.timer) {
      clearInterval(this.timer); // Component yok olduğunda interval durdurulur.
    }
  }

  private updateTime(): void {
    const now = new Date();
    this.currentTime = now.toLocaleTimeString(); // Saati alır ve formatlar.
  }
  getExchangeRates(): void {
    this.exchangeRateService.getExchangeRates().subscribe({
      next: (data) => {
        this.usdToTry = data.usdToTry;
        this.eurToTry = data.eurToTry;
        console.log('USD/TL: ', this.usdToTry, ' EUR/TL: ', this.eurToTry);
      },
      error: (error) => {
        console.error('Döviz kurları alınamadı:', error);
      }
    });
  }

  ngOnInit(): void {
    this.updateTime(); // saat
    this.timer = setInterval(() => this.updateTime(), 1000); // Her saniye saati günceller.
    this.getExchangeRates(); // Döviz kurlarını almak için servis çağrısı yapılır.


    this.getBalance(); //  bakiyeyi alır
    //  window nesnesi, tarayıcı penceresi ile ilgili bilgileri tutar ve web sayfası ile etkileşim kurmak için birçok metodu barındırır.
    if (typeof window !== 'undefined' && window.localStorage) {
      this.userService.getUserProfile().subscribe({
        next: (profile) => {
          console.log("Sidebar'da alınan kullanıcı Bilgisi:", profile);
          if (profile && profile.name && profile.surname) {
            this.userName = profile.name;
            this.userSurname = profile.surname;
            
            // Kullanıcı ID'sini localStorage'dan alıyoruz
            const userId = profile.userId;
            if (userId) {
              // Bakiye bilgisini alıyoruz
              this.accountService.getBalance().subscribe({
                next: (balance) => {
                  this.balance = balance;
                },
                error: (error) => {
                  console.error('Bakiye alınamadı:', error);
                }
              });
            }
          } else {
            console.warn('Profil bilgisi eksik veya yanlış formatta:', profile);
          }
        },
        error: (error) => {
          console.error('Profil alınamadı:', error);
        }
      });
    } else {
      console.error('Tarayıcı ortamında değil, localStorage kullanılamaz.');
    }
  }

  // Bakiye bilgilerini dinamik olarak alır
  getBalance(): void {
    const userId = localStorage.getItem('userId');  
    console.log("Alınan userId:", userId);  // userId'yi kontrol eder
    if (!userId) {
      this.router.navigate(['/login']);  // Kullanıcı girişi yapılmamışsa login sayfasına yönlendir
      console.error('Kullanıcı girişi yapılmamış!');
      return; // userId yoksa işlemi sonlandır
    }
  
    this.accountService.getBalance().subscribe({
      next: (balance) => {
        this.balance = balance; 
      },
      error: (error) => {
        console.error('Bakiye alınamadı:', error);
      }
    });
  }
}
