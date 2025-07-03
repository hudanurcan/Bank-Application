import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AccountService } from '../../../account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-withdraw-money',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './withdraw-money.component.html',
  styleUrl: './withdraw-money.component.css'
})
export class WithdrawMoneyComponent implements OnInit{
  balance: number = 0; // Mevcut bakiye
  withdrawAmount: number = 0; // Çekilecek tutar
  showAlert: boolean = false; // Uyarı mesajı gösterimi
  alertMessage: string = ''; // Gösterilecek mesaj

  constructor(private accountService: AccountService, private router: Router) {}

  ngOnInit(): void {
    this.getBalance(); // Component açıldığında bakiye bilgilerini alıyoruz
  }

  // Bakiye bilgilerini almak
  getBalance(): void {
    const userId = localStorage.getItem('userId');
    console.log("Get Balance için alınan userId:", userId);  // userId'yi konsola yazdırıyoruz
    if (!userId) {
      throw new Error('Kullanıcı girişi yapılmamış');
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

  // Butonlara tıklandığında ilgili tutarı inputa ekliyoruz
  setAmount(amount: number): void {
    this.withdrawAmount = amount;
  }

  // Para çekme işlemi
  withdraw(): void {
    if (this.withdrawAmount <= 0) {  // Burada withdrawAmount kullanılacak
      this.showAlertMessage('Lütfen geçerli bir tutar girin.');
      return;
    }

    const userId = localStorage.getItem('userId');  // Kullanıcı ID'sini alıyoruz
    if (!userId) {
      this.showAlertMessage('Kullanıcı girişi yapılmamış.');
      return;
    }

    // withdrawAmount ve userId'yi payload'a ekliyoruz
    this.accountService.withdraw(Number(userId), this.withdrawAmount).subscribe({
      next: (newBalance) => {
        this.balance = newBalance;
        this.showAlertMessage(`Başarıyla ${this.withdrawAmount} ₺ çektiniz. Yeni bakiyeniz: ${this.balance} ₺`);
        this.withdrawAmount = 0;  // İşlem sonrası tutarı sıfırlıyoruz
      },
      error: (error) => {
        console.error('Bir hata oluştu:', error);
        this.showAlertMessage('Bir hata oluştu, lütfen tekrar deneyin.');
      }
    });
  }
  // Alert mesajı gösterimi
  showAlertMessage(message: string): void {
    this.alertMessage = message;
    this.showAlert = true;

    // 3 saniye sonra alert kaybolur
    setTimeout(() => {
      this.showAlert = false;
    }, 3000);
  }
  }

