import { Component, inject } from '@angular/core';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  imports: [CurrencyPipe, FormsModule]
})
export class ShoppingCartComponent {
  cartService = inject(ShoppingCartService);

  showCheckoutForm: boolean = false;
  customerEmail: string = '';
  customerPhone: string = '';
  customerAddress: string = '';

  updateQuantity(productId: number, event: Event) {
    const target = event.target as HTMLInputElement;
    const quantity = parseInt(target.value, 10);
    if (!isNaN(quantity)) {
      this.cartService.updateQuantity(productId, quantity);
    }
  }

  startCheckout() {
    this.showCheckoutForm = true;
  }

  async processCheckout() {
    try {
      const response = await this.cartService.checkout(this.customerEmail, this.customerPhone, this.customerAddress);
      alert(`Order placed successfully! Order ID: ${response.orderId}, Status: ${response.status}`);
      this.showCheckoutForm = false;
      this.customerEmail = '';
      this.customerPhone = '';
      this.customerAddress = '';
    } catch (error) {
      alert('Failed to place order. Please try again.');
    }
  }

  cancelCheckout() {
    this.showCheckoutForm = false;
  }
}
