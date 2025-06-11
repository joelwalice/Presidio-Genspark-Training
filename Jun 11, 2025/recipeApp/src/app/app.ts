import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Recipe } from "./recipe/recipe";

@Component({
  selector: 'app-root',
  imports: [ Recipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'recipeApp';
}
