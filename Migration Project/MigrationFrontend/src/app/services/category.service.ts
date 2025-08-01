
import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models/category.model';
import { toObservable } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  categories = signal<Category[]>([]);
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5067/api/Category';

  categories$ = toObservable(this.categories);

  constructor() {
    this.Headers();
    this.loadCategories();
  }
  Headers(){
    return {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    };
  }

  loadCategories() {
    this.http.get<Category[]>(this.baseUrl, { headers: this.Headers() }).subscribe(data => this.categories.set(data));
  }
}
