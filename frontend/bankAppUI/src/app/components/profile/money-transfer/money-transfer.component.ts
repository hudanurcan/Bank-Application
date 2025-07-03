import { Component, OnInit } from '@angular/core';
import { SidebarComponent } from "../sidebar/sidebar.component";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MoneyTransferService } from '../../../money-transfer.service';
import { Router } from '@angular/router';
import { UserServiceService } from '../../../user.service.service';
import { ToastComponent } from '../../toast/toast.component';

@Component({
  selector: 'app-money-transfer',
  standalone: true,
  imports: [SidebarComponent, CommonModule, FormsModule, ToastComponent],
  templateUrl: './money-transfer.component.html',
  styleUrl: './money-transfer.component.css'
})
export class MoneyTransferComponent implements OnInit {

  receiverIban: string = '';
  amount: number = 0;
  description: string = '';
  userId: string | null = '';
  toastMessage: string = '';
  showToast: boolean = false;
  currentBalance: number | null = null;
  toastType: 'success' | 'error' = 'success';  // Başlangıç tipi

  constructor(
    private moneyTransferService: MoneyTransferService,
    private userService: UserServiceService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Kullanıcı bilgisini alıyoruz
    this.userService.getUserProfile().subscribe(
      user => {
        if (user && user.userId) {
          this.userId = user.userId;
          console.log('Kullanıcı ID:', this.userId);
        } else {
          console.error('Kullanıcı bilgisi alınamadı');
        }
      },
      error => {
        console.error('Kullanıcı bilgisi alınamadı:', error);
      }
    );
  }

  onSubmit(): void {
    if (!this.userId) {
      console.error('Kullanıcı ID bulunamadı');
      return;
    }

    // Transfer verilerini oluşturuyoruz
    const transferData = {
      senderUserId: this.userId,
      receiverIban: this.receiverIban,
      amount: this.amount,
      description: this.description
    };

    console.log('Transfer Data:', transferData);

    // Para transferi işlemini ayrı bir fonksiyonla çağırıyoruz
    this.transferMoney(transferData);
  }

  private transferMoney(transferData: any): void {
    this.moneyTransferService.transferMoney(transferData).subscribe(
      (response) => {
        console.log('Transfer başarılı:', response);
        
        // Toast mesajı göster
        this.toastMessage = response.message + ` Güncel bakiye: ₺${response.updatedBalance}`;
        this.toastType = 'success'; 
        this.showToast = true;
  
        // 5 saniye sonra toast'ı gizle
        setTimeout(() => this.showToast = false, 5000);
  
        // İstersen güncel bakiyeyi ekranda bir yere yazdır
        this.currentBalance = response.updatedBalance;
  
      },
      (error) => {
        console.error('Transfer hatası:', error);
        if (error.status === 400 && error.error.message) {
          this.toastMessage = error.error.message;
          this.toastType = 'error'; 
        } else {
          this.toastMessage = "Bilinmeyen bir hata oluştu.";
          this.toastType = 'error'; 

        }
        this.showToast = true;
        setTimeout(() => this.showToast = false, 5000);
      }
    );
  }
  
}
