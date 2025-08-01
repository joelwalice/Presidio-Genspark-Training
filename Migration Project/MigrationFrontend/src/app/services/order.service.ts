
import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Order } from '../models/order.model';
import { toObservable } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private orders = signal<Order[]>([]);
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5067/api/Order';

  orders$ = toObservable(this.orders);

  Headers() {
    return {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    };
  }

  constructor() {
    this.loadOrders();
  }

  loadOrders() {
    this.http.get<Order[]>(this.baseUrl, { headers: this.Headers() }).subscribe(data => this.orders.set(data));
  }

  placeOrder(order: Order) {
    return this.http.post<Order>(this.baseUrl, order, { headers: this.Headers() });
  }
}
