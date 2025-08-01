
import { Component, inject } from '@angular/core';
import { CategoryService } from '../../services/category.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  imports: [AsyncPipe]
})
export class CategoryListComponent {
  categoryService = inject(CategoryService);
}
