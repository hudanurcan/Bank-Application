import { Component, OnInit } from '@angular/core';
import { SidebarComponent } from "../sidebar/sidebar.component";
import { BankAccountService } from '../../../bank-account.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { AccountService } from '../../../account.service';
import { UserServiceService } from '../../../user.service.service';

@Component({
  selector: 'app-cards',
  standalone: true,
  imports: [SidebarComponent, CommonModule],
  templateUrl: './cards.component.html',
  styleUrl: './cards.component.css'
})
export class CardsComponent implements OnInit {

  cards: any[] = []; // Kart verilerini saklamak için
  userName: string = ''; // Kullanıcı adı
  userSurname: string = ''; // Kullanıcı soyadı
  formattedExpiryDate: string = ''; // Başlangıçta boş string olarak başlatıldı

  constructor(
    private accountService: AccountService,
    private userService: UserServiceService,
    private datePipe: DatePipe  // DatePipe inject ediliyor
  ) {}

  ngOnInit(): void {
    const userId = localStorage.getItem('userId'); // Kullanıcı ID'sini localStorage'dan alıyoruz

    if (userId) {
      // Kullanıcı bilgilerini almak
      this.userService.getUserProfile().subscribe({
        next: (profile) => {
          this.userName = profile.name.toUpperCase();;
          this.userSurname = profile.surname.toUpperCase();;

          // Kart bilgilerini almak
          this.accountService.getBankAccounts(Number(userId)).subscribe({
            next: (cards) => {
              this.cards = cards; // Kartları cards dizisine atıyoruz

              // SKT'yi formatlamak ve sadece yıl-ay göstermek
              if (cards.length > 0 && cards[0].expiryDate) {  // Eğer expiryDate varsa
                const expiryDate = new Date(cards[0].expiryDate);  // İlk kartın expiryDate'ini alıyoruz
                this.formattedExpiryDate = this.datePipe.transform(expiryDate, 'MM/YY') || '';  // Sadece yıl ve ay
              }
            },
            error: (error) => {
              console.error('Kartlar yüklenemedi:', error); // Hata durumunda
            },
          });
        },
        error: (error) => {
          console.error('Kullanıcı bilgileri alınamadı:', error);
        },
      });
    } else {
      console.error('Kullanıcı ID’si bulunamadı.'); // Kullanıcı ID’si yoksa
    }
  }

  formatCardNumber(cardNumber: string): string {
    return cardNumber.replace(/(\d{4})(?=\d)/g, '$1 '); // Kart numarasını 4lü kümelere ayırma
  }
}
