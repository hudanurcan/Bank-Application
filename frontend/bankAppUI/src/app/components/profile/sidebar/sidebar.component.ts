import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { UserServiceService } from '../../../user.service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {

  userName: string = 'Yükleniyor...';
  userSurname: string = '';

  constructor(private userService: UserServiceService) {}

  ngOnInit(): void {


 if (typeof window !== 'undefined' && window.localStorage) {
      this.userService.getUserProfile().subscribe({
        next: (profile) => {
          console.log("Sidebar'da Alınan Kullanıcı Bilgisi:", profile);
          if (profile && profile.name && profile.surname) {
            this.userName = profile.name;
            this.userSurname = profile.surname;
          } else {
            console.warn("Profil bilgisi eksik veya yanlış formatta:", profile);
          }
        },
        error: (error) => {
          console.error('Profil alınamadı:', error);
        }
      });
    } else {
      console.error("Tarayıcı ortamında değil, localStorage kullanılamaz.");
    }
  }
}

