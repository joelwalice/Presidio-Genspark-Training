import { Component, inject, signal, computed, effect } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { CategoryService } from '../../services/category.service';
import { Product } from '../../models/product.model';
import { Category } from '../../models/category.model';
import { list } from 'postcss';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  imports: [CurrencyPipe, FormsModule, CommonModule]
})
export class ProductListComponent {
  // Services
  productService = inject(ProductService);
  cartService = inject(ShoppingCartService);
  categoryService = inject(CategoryService);

  products = this.productService.products;
  categories = this.categoryService.categories;
  selectedCategory = signal<string | null>(null);

  filteredProducts = computed(() => {
    const categoryName = this.selectedCategory();
    const allProducts = this.products();

    console.log(`Filtering for category Name: ${categoryName}`);
    console.log(`All products count: ${allProducts.length}`);

    if (!Array.isArray(allProducts)) {
      console.error('ProductService.products() did not return an array!', allProducts);
      return []; // Return empty array to prevent errors
    }

    if (categoryName === null) {
      console.log('Returning all products.');
      return allProducts;
    }

    const filtered = allProducts.filter(p => p.categoryName === categoryName);
    console.log(`Filtered products count for category ${categoryName}: ${filtered.length}`);
    return filtered;
  });

  // Method to update the selected category signal when a user clicks.
  onSelectCategory(categoryName: string | null): void {
    this.selectedCategory.set(categoryName);
  }

  // Method to add items to cart with feedback
  addToCart(product: any): void {
    this.cartService.addToCart(product);
    // You could add a toast notification here or some other feedback
    console.log(`Added ${product.productName} to cart`);
  }

  constructor() {
    // Optional: Log changes to the filtered list for debugging
    effect(() => {
      console.log(`Filtered product count (effect): ${this.filteredProducts().length}`);
    });
  }
}