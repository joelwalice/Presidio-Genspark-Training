import { Component, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ProductService } from '../../services/product.service';
import { CategoryService } from '../../services/category.service';
import { OrderService } from '../../services/order.service';
import { NewsService } from '../../services/news.service';
import { ColorService } from '../../services/color.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  imports: [ReactiveFormsModule, FormsModule]
})
export class LoginComponent {
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  productService = inject(ProductService);
  categoryService = inject(CategoryService);
  orderService = inject(OrderService);
  newsService = inject(NewsService);
  colorService = inject(ColorService);
  router = inject(Router);

  loginForm = this.fb.group({
    username: '',
    password: ''
  });

  onSubmit() {
    this.authService.login(this.loginForm.value).subscribe(response => {
      this.authService.token.set(response.token);
      this.loginForm.reset();

      this.productService.loadProducts();
      this.categoryService.loadCategories();
      this.orderService.loadOrders();
      this.newsService.loadNews();
      this.colorService.loadColors();

      this.router.navigate(['/']);
    });
  }
}