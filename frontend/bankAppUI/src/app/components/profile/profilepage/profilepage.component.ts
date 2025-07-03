import { Component, OnInit } from '@angular/core';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { UserServiceService } from '../../../user.service.service';
import { Router } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { ToastComponent } from '../../toast/toast.component';
import { BankAccountService } from '../../../bank-account.service';

@Component({
  selector: 'app-profilepage',
  standalone: true,
  imports: [SidebarComponent, DatePipe, ToastComponent, CommonModule],
  templateUrl: './profilepage.component.html',
  styleUrl: './profilepage.component.css'
})
export class ProfilepageComponent implements OnInit{
  userProfile: any = {};  // Profil bilgilerini tutacak değişken
  userId: string | null = localStorage.getItem('userId'); // Kullanıcı ID'sini localStorage'dan alıyoruz
  toastMessage: string = '';
  toastVisible: boolean = false;
  isModalVisible: boolean = false; // Modal'ı kontrol etmek için flag
  toastType: 'success' | 'error' = 'success';  // Başlangıç tipi
  toastClass: string = '';  // Bu satırı ekleyin!

  constructor(private userService: UserServiceService, private router: Router, private bankAccountService: BankAccountService) {}

  ngOnInit(): void {
    if (this.userId) {
      this.userService.getUserProfile().subscribe(
        (data) => {
          this.userProfile = data; // Profil bilgilerini alıyoruz
        },
        (error) => {
          console.error('Profil bilgisi alınamadı:', error);
          this.router.navigate(['/login']); // Giriş yapılmadıysa login sayfasına yönlendirilir
        }
      );
    } else {
      this.router.navigate(['/login']); // Kullanıcı ID'si yoksa login sayfasına yönlendirilir
    }
  }

openModal(): void {
  this.isModalVisible = true;
}


confirmDeactivateAccount(): void {
  console.log("Hesap kapatma işlemi başlatıldı...");

  this.bankAccountService.deactivateAccount(this.userId).subscribe(
    (response) => {
      console.log("Hesap kapatma başarılı:", response);

      if (response && response.message === "Hesabınız başarıyla kapatıldı.") {
        // İlk olarak toast mesajını gösteriyoruz
        this.showToast('Hesabınız başarıyla kapatıldı!', 'success', true);  // Sağ altta göster

        this.closeModal(); 

        // 4 saniye sonra yönlendirmeyi yapıyoruz
        setTimeout(() => {
          console.log("Yönlendirme yapılıyor...");
          this.router.navigate(['/homepage']);  // 4 saniye sonra homepage'e yönlendiriyoruz
        }, 4000);  // 4 saniye sonra yönlendirme yapılacak
      } else {
        this.showToast('Hesap kapama işlemi sırasında bir hata oluştu.', 'error', true);  // Sağ altta hata mesajı göster
      }
    },
    (error) => {
      console.error('Hesap kapama hatası:', error);
      this.showToast(error?.error?.message || 'Hesap kapama işlemi sırasında bir hata oluştu.', 'error', true);  // Sağ altta hata mesajı göster
    }
  );
}


closeModal(): void {
  console.log("Modal kapanıyor...");
  this.isModalVisible = false; // Modal'ı kapatıyoruz
}
showToast(message: string, type: 'success' | 'error', isAccountDeactivation: boolean = false) {
  console.log("Toast mesajı gösteriliyor...");
  this.toastMessage = message;  // Mesajı güncelliyoruz
  this.toastVisible = true;  // Toast'ı görünür yapıyoruz
  this.toastType = type;  // Type'ı belirliyoruz (success ya da error)

  // Eğer hesap kapama işlemi ise, sağ alt köşede çıkmasını sağlıyoruz
  if (isAccountDeactivation) {
    this.toastClass = 'toast-position-bottom-right';
  } else {
    this.toastClass = '';  // Normalde ortada çıkması için
  }

  // 4 saniye sonra toast mesajını kaybetmek için
  setTimeout(() => {
    this.toastVisible = false;  // Toast mesajını gizle
  }, 4000); 
}

}
