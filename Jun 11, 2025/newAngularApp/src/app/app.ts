import { Component } from '@angular/core';
import { First } from "./first/first";
import { CustomerDetails } from "./customer-details/customer-details";
import { Products } from "./products/products";
import { Product } from "./product/product";
import { Recipes } from "./recipes/recipes";


@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Recipes]
})
export class App {
  protected title = 'newAngularApp';
}
