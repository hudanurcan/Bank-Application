import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable,forkJoin } from 'rxjs';
import { mergeMap, map } from 'rxjs/operators';  // Burada mergeMap ve map'i import ettik

@Injectable({
  providedIn: 'root'
})
export class ExchangeRateService {
  private baseUrl: string = 'https://v6.exchangerate-api.com/v6/adb3711a26e680b4e1286464/latest/'; // Bu URL'yi kullanarak USD ve EUR'yu aynı anda alabiliriz.
  constructor(private http: HttpClient) {}

  // Döviz kurunu almak için fonksiyon
  getExchangeRates(): Observable<any> {
    const usdUrl = `${this.baseUrl}USD`;  // USD/TL almak için URL
    const eurUrl = `${this.baseUrl}EUR`;  // EUR/TL almak için URL

    // İki döviz kuru için paralel istek yapıyoruz
    return this.http.get<any>(usdUrl).pipe(
      mergeMap((usdData) => {
        return this.http.get<any>(eurUrl).pipe(
          map((eurData) => {
            return {
              usdToTry: usdData.conversion_rates.TRY,
              eurToTry: eurData.conversion_rates.TRY,
            };
          })
        );
      })
    );
  }
}
