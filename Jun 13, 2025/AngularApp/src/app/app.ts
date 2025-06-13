import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Products } from "./products/products";
import { Product } from "./product/product";
import { Menu } from "./menu/menu";
import { Login } from "./login/login";

@Component({
  selector: 'app-root',
  imports: [Menu, Login], //Products, Product
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'AngularApp';
}
