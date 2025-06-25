import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipe } from '../recipe/recipe';
import { RecipeService } from '../services/recipe-service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('Recipe Component', () => {
  let component: Recipe;
  let fixture: ComponentFixture<Recipe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Recipe],
      imports: [HttpClientTestingModule],
      providers: [RecipeService]
    }).compileComponents();

    fixture = TestBed.createComponent(Recipe);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
