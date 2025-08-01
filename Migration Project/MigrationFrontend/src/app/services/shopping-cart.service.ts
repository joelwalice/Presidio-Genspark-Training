import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product.model';
import { CartItem } from '../models/cart-item.model';
import { OrderRequestDto } from '../models/order-request.model';
import { OrderResponseDto } from '../models/order-response.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {
  cart = signal<CartItem[]>([]);
  private http = inject(HttpClient);
  private authService = inject(AuthService); 
  private baseUrl = 'http://localhost:5067/api/ShoppingCart';

  constructor() {
    // Load cart from local storage if available
    const storedCart = localStorage.getItem('shoppingCart');
    if (storedCart) {
      this.cart.set(JSON.parse(storedCart));
    }
  }

  private saveCart() {
    localStorage.setItem('shoppingCart', JSON.stringify(this.cart()));
  }

  Headers(){
    return {
      'Content-Type': 'application/json',
      'Accept': 'application/json', 
      'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    };
  }

  addToCart(product: Product, quantity: number = 1) {
    this.cart.update(items => {
      const existingItem = items.find(item => item.product.productId === product.productId);
      if (existingItem) {
        existingItem.quantity += quantity;
      } else {
        items.push({ product, quantity });
      }
      this.saveCart();
      return [...items];
    });

    this.http.post(`${this.baseUrl}/add`, {
      productId: product.productId,
      quantity: quantity
    }, {headers : this.Headers()}).subscribe({
      next: () => console.log('Item added to cart on server'),
      error: (error) => console.error('Failed to notify server about cart addition:', error)
    });
  }

  removeFromCart(productId: number) {
    this.cart.update(items => {
      const updatedItems = items.filter(item => item.product.productId !== productId);
      this.saveCart();
      return updatedItems;
    });
  }

  updateQuantity(productId: number, quantity: number) {
    this.cart.update(items => {
      const existingItem = items.find(item => item.product.productId === productId);
      if (existingItem) {
        existingItem.quantity = quantity;
        if (existingItem.quantity <= 0) {
          return items.filter(item => item.product.productId !== productId);
        }
      }
      this.saveCart();
      return [...items];
    });
  }

  clearCart() {
    this.cart.set([]);
    this.saveCart();
  }

  getCartTotal(): number {
    return this.cart().reduce((total, item) => total + (item.product.price || 0) * item.quantity, 0);
  }

  async checkout(customerEmail: string, customerPhone: string, customerAddress: string): Promise<OrderResponseDto> {
    const userId = this.authService.currentUserId();
    console.log('Processing checkout with User Id:', userId); 
    if (!userId) {
      throw new Error('User not logged in.');
    }

    const orderItems = this.cart().map(item => ({
      productId: item.product.productId,
      quantity: item.quantity
    }));
    console.log('Processing checkout with Order Id request:', orderItems);


    const orderRequest: OrderRequestDto = {
      userId: userId,
      customerEmail: customerEmail,
      customerPhone: customerPhone,
      customerAddress: customerAddress,
      items: orderItems
    };

    console.log('Processing checkout with order request:', orderRequest);

    try {
      const response = await this.http.post<OrderResponseDto>(`${this.baseUrl}/checkout`, orderRequest).toPromise();
      this.clearCart(); 
      return response!;
    } catch (error) {
      console.error('Checkout failed:', error);
      throw error;
    }
  }
}
