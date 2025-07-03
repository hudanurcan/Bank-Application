import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-deposit-money',
  standalone: true,
  imports: [CommonModule, FormsModule ],
  templateUrl: './deposit-money.component.html',
  styleUrl: './deposit-money.component.css'
})
export class DepositMoneyComponent implements OnInit {
  balance: number = 0; // Kullanıcının mevcut bakiyesi
  depositAmount: number = 0; // Yatırılacak tutar
  showAlert: boolean = false; // Alert görünür mü?
  alertMessage: string = ''; // Gösterilecek mesaj

  constructor(private accountService: AccountService, private router: Router) {}

  ngOnInit(): void {
    this.getBalance(); // Component açıldığında bakiyeyi alıyoruz
  }

  // Bakiye bilgilerini almak
  getBalance(): void {
    const userId = localStorage.getItem('userId');  // localStorage'dan userId'yi alıyoruz
    console.log("Alınan userId:", userId);  // Burada userId'yi kontrol et
    if (!userId) {
      this.router.navigate(['/login']);  // Kullanıcı girişi yapılmamışsa login sayfasına yönlendir
      console.error('Kullanıcı girişi yapılmamış!');
      return; // Eğer userId yoksa işlemi sonlandır
    }
  
    this.accountService.getBalance().subscribe({
      next: (balance) => {
        this.balance = balance; // Bakiye bilgisini güncelliyoruz
      },
      error: (error) => {
        console.error('Bakiye alınamadı:', error);
      }
    });
  }

  // Butonlara basıldığında ilgili tutarı inputa ekler
  setAmount(amount: number): void {
    this.depositAmount = amount;
  }

  // Para yatırma işlemi
 deposit(): void {
  if (this.depositAmount <= 0) {
    this.showAlertMessage('Lütfen geçerli bir tutar girin.');
    return;
  }

  const userId = localStorage.getItem('userId'); // localStorage'dan kullanıcı ID'sini alıyoruz
  if (!userId) {
    this.showAlertMessage('Kullanıcı girişi yapılmamış.');
    return;
  }

  // Payload'a userId'yi de ekliyoruz
  this.accountService.deposit(Number(userId), this.depositAmount).subscribe({
    next: (newBalance) => {
      this.balance = newBalance;
      this.showAlertMessage(`Başarıyla ${this.depositAmount} ₺ yatırdınız. Yeni bakiyeniz: ${this.balance} ₺`);
      this.depositAmount = 0;
    },
    error: (error) => {
      console.error('Bir hata oluştu:', error);
      this.showAlertMessage('Bir hata oluştu, lütfen tekrar deneyin.');
    }
  });
}


  // Alert mesajını gösterir
  showAlertMessage(message: string): void {
    this.alertMessage = message;
    this.showAlert = true;

    // 3 saniye sonra alert kaybolur
    setTimeout(() => {
      this.showAlert = false;
    }, 3000);
  }
}