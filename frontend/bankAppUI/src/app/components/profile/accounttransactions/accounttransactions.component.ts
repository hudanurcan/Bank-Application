import { Component, OnInit } from '@angular/core';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { MoneyTransferService } from '../../../money-transfer.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../../account.service';

@Component({
  selector: 'app-accounttransactions',
  standalone: true,
  imports: [SidebarComponent, CommonModule],
  templateUrl: './accounttransactions.component.html',
  styleUrl: './accounttransactions.component.css'
})
export class AccounttransactionsComponent implements OnInit {
  userTransactions: any[] = []; // Kullanıcının işlem geçmişi
  userId: number = 0; // Kullanıcı ID'si
  loading: boolean = false;
  currentPage: number = 1; // Geçerli sayfa
  itemsPerPage: number = 7; // Her sayfada gösterilecek işlem sayısı,

  constructor(
    private accountService: AccountService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Kullanıcı ID'sini localStorage'dan alıyoruz
    const storedUserId = localStorage.getItem('userId');
    if (!storedUserId) {
      this.router.navigate(['/login']);
      return;
    }
    this.userId = parseInt(storedUserId);

    this.getTransactionHistory(); // İşlem geçmişini çekiyoruz
  }

  // İşlem geçmişini almak için metod
  getTransactionHistory(): void {
    this.loading = true;

    // AccountService'den işlem geçmişini çekiyoruz
    this.accountService.getTransactionHistory(this.userId).subscribe(
      (data: any[]) => {
        this.userTransactions = data;
        this.loading = false;
      },
      (error) => {
        console.error('İşlem geçmişi alınırken hata oluştu:', error);
        this.loading = false;
      }
    );
  }

    // Sayfa numarasına göre işlem geçmişini al
    get pagedTransactions(): any[] {
      const startIndex = (this.currentPage - 1) * this.itemsPerPage;
      const endIndex = startIndex + this.itemsPerPage;
      return this.userTransactions.slice(startIndex, endIndex);
    }
  
    // Sayfa değiştirme fonksiyonu
    changePage(page: number): void {
      this.currentPage = page;
    }
  
    // Toplam sayfa sayısını hesapla
    get totalPages(): number {
      return Math.ceil(this.userTransactions.length / this.itemsPerPage);
    }
}
