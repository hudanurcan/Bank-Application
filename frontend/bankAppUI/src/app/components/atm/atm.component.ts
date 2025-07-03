import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserServiceService } from '../../user.service.service';
import { AccountService } from '../../account.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-atm',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './atm.component.html',
  styleUrl: './atm.component.css'
})
export class AtmComponent implements OnInit {

  balance: number = 0;  // Bakiye değişkeni
  userName: string = '';  // Kullanıcı adını tutacak değişken
  userId: string | null = localStorage.getItem('userId');  // Kullanıcı ID'sini localStorage'dan alıyoruz
  userSurname: string = '';

  constructor(
    private userService: UserServiceService,
    private accountService: AccountService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getBalance();  // Bakiye bilgisini alıyoruz

    if (this.userId) {
      this.getUserName();  // Kullanıcı adını alıyoruz
    } else {
      this.router.navigate(['/login']); // Kullanıcı ID'si yoksa login sayfasına yönlendiriyoruz
    }
  }

  // Kullanıcı adını almak için UserService kullanıyoruz
  getUserName(): void {
    this.userService.getUserProfile().subscribe(
      (profile) => {
        this.userName = profile.name;  // Profildeki adı alıyoruz
        this.userSurname = profile.surname;
      },
      (error) => {
        console.error('Kullanıcı profil bilgisi alınırken hata oluştu:', error);
      }
    );
  }

  // Bakiye bilgisini almak için AccountService kullanıyoruz
  getBalance(): void {
    this.accountService.getBalance().subscribe(
      (balance) => {
        this.balance = balance;  // Bakiye bilgisini güncelliyoruz
      },
      (error) => {
        console.error('Bakiye alınırken hata oluştu:', error);
      }
    );
  }
}
