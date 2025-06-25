import { Injectable } from '@angular/core';
import { RecipeModel } from '../models/recipe.model';
import { map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private apiUrl = 'https://dummyjson.com/recipes';

  constructor(private http: HttpClient) {}

  getRecipes(): Observable<{ recipes: RecipeModel[] }> {
    return this.http.get<{ recipes: RecipeModel[] }>(this.apiUrl).pipe(
      map((data: { recipes: RecipeModel[] }) => {
        return {
          recipes: data.recipes.map((recipe) => ({
            ...recipe,
            
            ingredients: recipe.ingredients, 
          })),
        };
      })
    );
  }
}
