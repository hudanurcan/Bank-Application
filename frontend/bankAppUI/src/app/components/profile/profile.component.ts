import { Component, OnInit } from '@angular/core';
import { SidebarComponent } from "./sidebar/sidebar.component";
import { UserServiceService } from '../../user.service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [SidebarComponent, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
 
  user: any; // Kullanıcı bilgilerini tutacak değişken

  constructor(private userService: UserServiceService) { }

  ngOnInit(): void {
    // // Kullanıcı bilgilerini al
    // this.userService.getUserProfile().subscribe(
    //   (data) => {
    //     this.user = data; // Kullanıcı bilgilerini alıyoruz
    //   },
    //   (error) => {
    //     console.error('Error fetching user profile:', error);
    //   }
    // );
  }
}
