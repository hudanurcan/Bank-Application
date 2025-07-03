import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormGroup , FormBuilder, Validators   } from '@angular/forms';
import { UserServiceService } from '../../user.service.service';
import { CommonModule } from '@angular/common';
import { ToastComponent } from '../toast/toast.component';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [RouterModule, FormsModule, ReactiveFormsModule, CommonModule, ToastComponent],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent implements OnInit {

 
  signupform!: FormGroup;
  toastMessage: string = '';
  toastVisible: boolean = false;
  isKvkkAccepted: boolean = false; // KVKK onayı kontrolü

  user = {
    name: '',
    surname: '',
    tc: '',
    birthday: '',
    phone: '',
    email: '',
    address: '',
    password: '',
    confirmPassword: ''
  };

  errorMessage: string | null = null; // Hata mesajı için değişken

  constructor(private router: Router, private userService: UserServiceService, private fb: FormBuilder) {}

  ngOnInit(): void {
    // Formu başlatıyoruz
    this.signupform = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      surname: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      tc: ['', [Validators.required, Validators.pattern(/^\d{11}$/)]],
      phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      address: ['', Validators.required],
      birthday: ['', Validators.required],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&,.]{8,}$/), 
        ],
      ],
      confirmPassword: ['', Validators.required],
    }, { validators: this.passwordsMatch });
  }

  // Şifrelerin eşleşip eşleşmediğini kontrol eden fonksiyon
  passwordsMatch(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordsNotMatching: true };
  }

  onSubmit() {
    console.log(this.signupform.valid);  // Form geçerli mi?
    if (this.signupform.invalid || !this.isKvkkAccepted) {
      this.showToast('Lütfen tüm alanları doğru bir şekilde doldurunuz!');
      return;
    }

    const user = {
      name: this.user.name,
      surname: this.user.surname,
      email: this.user.email,
      password: this.user.password, 
      tc: this.user.tc,
      phone: this.user.phone,
      address: this.user.address,
      birthday: this.user.birthday
    };

    this.userService.register(user).subscribe(
      response => {
        console.log('Kullanıcı başarıyla kaydedildi!', response);
        this.showToast('Kayıt Başarılı')
        //  this.router.navigate(['/login']);
              // Toast'ın 3 saniye süreyle gösterilmesini bekleyip yönlendirmeyi başlatıyoruz
      setTimeout(() => {
        this.router.navigate(['/homepage']);
      }, 4000);  // 3 saniye sonra yönlendirme yapılır
      },
      error => {
        console.error('Kayıt hatası:', error);
      if (error.status === 400 && error.error.message) {
        this.errorMessage = error.error.message  // Örneğin: "18 yaşından küçük kullanıcılar kayıt olamaz."
      } else {
        this.showToast('Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyin.');
      }
      }
    );
  }

  showToast(message: string) {
    this.toastMessage = message;
    this.toastVisible = true;
  
    setTimeout(() => {
      this.toastVisible = false;
    }, 4000); // 3 saniyede kaybolur
  }

}