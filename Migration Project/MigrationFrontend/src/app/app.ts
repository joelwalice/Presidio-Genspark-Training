
import { Component, inject, computed } from '@angular/core';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { ShoppingCartService } from './services/shopping-cart.service';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterLink
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private cartService = inject(ShoppingCartService);

  constructor(private router: Router) {
  }

  cartItemCount = computed(() => {
    return this.cartService.cart().reduce((total, item) => total + item.quantity, 0);
  });
  isDefaultRoute() {
    return this.router.url === '/' || this.router.url === '';
  }
}
