import { Component, inject, signal } from '@angular/core';
import { NewsService } from '../../services/news.service';
import { AsyncPipe, DatePipe, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { saveAs } from 'file-saver';
import { News, NewsCreate, NewsUpdate } from '../../models/news.model';

@Component({
  selector: 'app-news-list',
  templateUrl: './news-list.component.html',
  imports: [AsyncPipe, DatePipe, FormsModule, CommonModule]
})
export class NewsListComponent {
  newsService = inject(NewsService);
  showForm = signal(false);
  editingNews = signal<News | null>(null);
  
  formData: any = {
    title: '',
    shortDescription: '',
    content: '',
    image: '',
    status: true
  };

  isExportMenuOpen = signal(false);

  toggleExportMenu() {
    this.isExportMenuOpen.set(!this.isExportMenuOpen());
  }

  showAddForm() {
    this.resetForm();
    this.editingNews.set(null);
    this.showForm.set(true);
  }

  editNews(news: News) {
    this.editingNews.set(news);
    this.formData = {
      title: news.title,
      shortDescription: news.shortDescription,
      content: news.content,
      image: news.image || '',
      status: news.status === 1
    };
    this.showForm.set(true);
  }

  saveNews() {
    const newsData = {
      ...this.formData,
      userId: 1, // You might want to get this from auth service
      createdDate: new Date()
    };

    if (this.editingNews()) {
      const updateData: NewsUpdate = {
        newsId: this.editingNews()!.newsId,
        ...newsData
      };
      
      this.newsService.update(this.editingNews()!.newsId, updateData).subscribe({
        next: () => {
          this.newsService.loadNews();
          this.cancelForm();
        },
        error: (error) => console.error('Error updating news:', error)
      });
    } else {
      this.newsService.create(newsData as NewsCreate).subscribe({
        next: () => {
          this.newsService.loadNews();
          this.cancelForm();
        },
        error: (error) => console.error('Error creating news:', error)
      });
    }
  }

  deleteNews(id: number) {
    if (confirm('Are you sure you want to delete this news item?')) {
      this.newsService.delete(id).subscribe({
        next: () => {
          this.newsService.loadNews();
        },
        error: (error) => console.error('Error deleting news:', error)
      });
    }
  }

  cancelForm() {
    this.showForm.set(false);
    this.editingNews.set(null);
    this.resetForm();
  }

  resetForm() {
    this.formData = {
      title: '',
      shortDescription: '',
      content: '',
      image: '',
      status: true
    };
  }

  exportCsv() {
    this.newsService.exportCsv().subscribe(blob => {
      saveAs(blob, 'news.csv');
    });
  }

  exportExcel() {
    this.newsService.exportExcel().subscribe(blob => {
      saveAs(blob, 'news.xlsx');
    });
  }
}