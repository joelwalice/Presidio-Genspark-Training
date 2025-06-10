import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class Products {
  products: any[] = [
  {
    id: 1,
    name: 'Smartphone',
    price: 499,
    imageUrl: 'https://images.unsplash.com/photo-1603184017968-953f59cd2e37?q=80&w=2942&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
    count: 0
  },
  {
    id: 2,
    name: 'Headphones',
    price: 199,
    imageUrl: 'https://images.unsplash.com/photo-1546435770-a3e426bf472b?q=80&w=2930&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D',
    count: 0
  },
  {
    id: 3,
    name: 'Camera',
    price: 899,
    imageUrl: 'https://images.unsplash.com/photo-1510127034890-ba27508e9f1c?w=900&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NXx8Y2FtZXJhfGVufDB8fDB8fHww',
    count: 0
  }
];

  constructor() { }

  addToCart(product: any) {
    product.count++;
  }
}
