import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Menu } from "./menu/menu";
import { Product } from "./product/product";
import { Products } from "./products/products";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Menu, Product, Products],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'ProductsApp';
}
