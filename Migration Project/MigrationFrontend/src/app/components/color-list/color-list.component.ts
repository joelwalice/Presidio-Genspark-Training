
import { Component, inject, signal } from '@angular/core';
import { ColorService } from '../../services/color.service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Color, ColorCreate, ColorUpdate } from '../../models/color.model';

@Component({
  selector: 'app-color-list',
  templateUrl: './color-list.component.html',
  imports: [AsyncPipe, FormsModule, CommonModule]
})
export class ColorListComponent {
  colorService = inject(ColorService);
  showForm = signal(false);
  editingColor = signal<Color | null>(null);
  
  formData = {
    name: ''
  };

  showAddForm() {
    this.resetForm();
    this.editingColor.set(null);
    this.showForm.set(true);
  }

  editColor(color: Color) {
    this.editingColor.set(color);
    this.formData = {
      name: color.name
    };
    this.showForm.set(true);
  }

  saveColor() {
    if (!this.formData.name.trim()) {
      return;
    }

    const colorData = {
      name: this.formData.name.trim()
    };

    if (this.editingColor()) {
      this.colorService.update(this.editingColor()!.colorId, colorData as ColorUpdate).subscribe({
        next: () => {
          this.colorService.loadColors();
          this.cancelForm();
        },
        error: (error) => console.error('Error updating color:', error)
      });
    } else {
      this.colorService.create(colorData as ColorCreate).subscribe({
        next: () => {
          this.colorService.loadColors();
          this.cancelForm();
        },
        error: (error) => console.error('Error creating color:', error)
      });
    }
  }

  deleteColor(id: number) {
    if (confirm('Are you sure you want to delete this color?')) {
      this.colorService.delete(id).subscribe({
        next: () => {
          this.colorService.loadColors();
        },
        error: (error) => console.error('Error deleting color:', error)
      });
    }
  }

  cancelForm() {
    this.showForm.set(false);
    this.editingColor.set(null);
    this.resetForm();
  }

  resetForm() {
    this.formData = {
      name: ''
    };
  }
}
