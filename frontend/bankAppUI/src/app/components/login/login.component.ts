import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '../../auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent implements OnInit {
 // loginType: string = ''; // "atm" veya "profile" bilgisini tutmak için
  
 ngOnInit() {
  // URL'deki type parametresini alıp loginType değişkenine atar
    this.route.queryParams.subscribe(params => {
      if (params['type']) {
        this.loginType = params['type'];
      }
    });

    this.route.params.subscribe(params => {
      if (params['type']) {
        this.loginType = params['type'];
      }
    });

    console.log('Login Type:', this.loginType);  // Konsola yazdırarak değer kontrolü

}

onSubmit(form: any) {
  if (form.valid) {
    console.log('Login Type:', this.loginType);

    this.authService.login(this.tc, this.password).subscribe({
      next: (response) => {
        console.log("Login Response:", response);  // Gelen cevabı kontrol et
        console.log("response userId:", response.userId);

        if (response.token) {
          localStorage.setItem('token', response.token);
          console.log("Token saved:", response.token);
          localStorage.setItem('userId', response.userId.toString());
          console.log('LocalStorage ID:', response.userId); // Burada ID'yi kontrol edin

          // Login başarılıysa doğru sayfaya yönlendir
          if (this.loginType === 'atm') {
            this.router.navigate(['/atm']);
          } else if (this.loginType === 'bank') {
            this.router.navigate(['/mainpage']);
          } else {
            console.error('Geçersiz login türü!');
          }
        } else {
          console.error("Token bulunamadı, backend'den eksik geldi.");
          this.errorMessage = "Beklenmeyen bir hata oluştu, tekrar deneyin.";
        }
      },
      error: (error) => {
        console.error("Giriş başarısız:", error);

        // Backend 401 Unauthorized döndürüyorsa hata mesajını göster ama yönlendirme yapma
        if (error.status === 401) {
          this.errorMessage = "TC Kimlik Numarası veya şifre hatalı.";
        } else if (error.status === 400) { // Backend'den "Hesabınız kapalı" gibi bir mesaj alırsak
          this.errorMessage = error.error.message || "Hesabınız kapalı. Lütfen destek ile iletişime geçin.";
        } else {
          this.errorMessage = "Giriş yapılamadı, lütfen tekrar deneyin.";
        }
      }
    });
  } else {
    console.error('Form geçersiz!');
    this.errorMessage = "Lütfen tüm alanları doldurun.";
  }
}

loginType: string = ''; // "atm" veya "bank"
tc: string = '';
password: string = '';
errorMessage: string = '';

constructor(
  private route: ActivatedRoute,
  private router: Router,
  private authService: AuthService
) {}


login() {
  this.authService.login(this.tc, this.password).subscribe({
    next: (response) => {
      if (response.token) {
        localStorage.setItem('token', response.token); // Token'ı kaydet
        localStorage.setItem('userId', response.userId.toString()); // userId'yi kaydet
        console.log("Token saved:", response.token);
        console.log("User ID saved:", localStorage.getItem('userId')); // Burada userId'yi kontrol et

  
        // Login türüne göre yönlendirme yapıyoruz
        if (this.loginType === 'atm') {
          this.router.navigate(['/atm']);
        } else if (this.loginType === 'bank') {
          this.router.navigate(['/mainpage']);
        } else {
          console.error('Geçersiz login türü!');
        }
      } else {
        console.error('Token bulunamadı.');
      }
    },
    error: (error) => {
      if (error.status === 401) {
        this.errorMessage = "TC Kimlik Numarası veya şifre hatalı.";
      } else {
        this.errorMessage = "Giriş yapılamadı, lütfen tekrar deneyin.";
      }
    }
  });
  
}
}
