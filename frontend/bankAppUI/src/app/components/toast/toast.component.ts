import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css'
})
export class ToastComponent {
  @Input() message: string = '';  // Bildirim mesajı
  @Input() show: boolean = false;  // Toast'ın görünür olup olmadığını kontrol eder
  @Input() type: 'success' | 'error' = 'success'; // default olarak success
  @Input() toastClass: string = '';  // Dinamik olarak sınıfı alacak

}
