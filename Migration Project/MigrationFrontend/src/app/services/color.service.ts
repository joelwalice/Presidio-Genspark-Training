
import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Color, ColorCreate, ColorUpdate } from '../models/color.model';
import { toObservable } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ColorService {
  private colors = signal<Color[]>([]);
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5067/api/Colors';

  colors$ = toObservable(this.colors);

  constructor() {
    this.loadColors();
    this.Headers();
  }
  Headers(){
    return {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    };
  }

  loadColors() {
    this.http.get<Color[]>(this.baseUrl, {headers : this.Headers()}).subscribe(data => this.colors.set(data));
  }

  create(colorData: ColorCreate): Observable<Color> {
    return this.http.post<Color>(this.baseUrl, colorData, { headers: this.Headers() });
  }

  update(id: number, colorData: ColorUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, colorData, { headers: this.Headers() });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { headers: this.Headers() });
  }
}
