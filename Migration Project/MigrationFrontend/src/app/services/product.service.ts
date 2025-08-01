import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product.model';
import { toObservable } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  products = signal<Product[]>([]);
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5067/api/Products';

  Headers() {
    return {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    };
  }

  products$ = toObservable(this.products);

  constructor() {
    this.loadProducts();
  }

  loadProducts() {
    this.http.get<Product[]>(this.baseUrl, { headers: this.Headers() }).subscribe(data => {
      const ids = new Set();
      for (const product of data) {
        if (ids.has(product.productId)) {
          console.warn(`ProductService: Duplicate product ID detected from API: ${product.productId}`);
        }
        ids.add(product.productId);
      }
      this.products.set(data);
    });
  }

  getProduct(id: number) {
    return this.http.get<Product>(`${this.baseUrl}/${id}`, { headers: this.Headers() });
  }
}