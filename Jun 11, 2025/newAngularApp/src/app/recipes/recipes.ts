import { Component, OnInit, signal } from '@angular/core';
import { RecipeModel } from '../models/recipe';
import { RecipeService } from '../services/recipe.service';

@Component({
  selector: 'app-recipes',
  standalone: true,
  templateUrl: './recipes.html',
  styleUrls: ['./recipes.css']
})
export class Recipes implements OnInit {
  recipes = signal<RecipeModel[] | undefined>(undefined);

  constructor(private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.recipeService.getRecipes().subscribe({
      next: (data: any) => {
        this.recipes.set(data.recipes);
        console.log(data);
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('Finished Execution...');
      }
    });
  }
}
