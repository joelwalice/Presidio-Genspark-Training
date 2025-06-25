import { Component, Input } from '@angular/core';
import { RecipeModel } from '../models/recipe.model';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-recipe-card',
  standalone : true,
  imports: [CommonModule],
  templateUrl: './recipe-card.html',
  styleUrl: './recipe-card.css'
})
export class RecipeCard {
  @Input() recipe!: RecipeModel;
  constructor(private http : HttpClient){}
}
