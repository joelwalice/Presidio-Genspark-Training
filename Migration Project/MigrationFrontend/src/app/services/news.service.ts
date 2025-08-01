import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { News, NewsCreate, NewsUpdate } from '../models/news.model';
import { toObservable } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  private news = signal<News[]>([]);
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5067/api/News';

  Headers() {
    return {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    };
  }

  news$ = toObservable(this.news);

  constructor() {
    this.Headers();
    this.loadNews();
  }

  loadNews() {
    this.http.get<News[]>(this.baseUrl, { headers: this.Headers() }).subscribe(data => this.news.set(data));
  }

  create(newsData: NewsCreate): Observable<News> {
    return this.http.post<News>(this.baseUrl, newsData, { headers: this.Headers() });
  }

  update(id: number, newsData: NewsUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, newsData, { headers: this.Headers() });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { headers: this.Headers() });
  }

  exportCsv() {
    return this.http.get(`${this.baseUrl}/export/csv`, { headers: this.Headers(), responseType: 'blob' });
  }

  exportExcel() {
    return this.http.get(`${this.baseUrl}/export/excel`, {
      headers: this.Headers(),
      responseType: 'blob'
    });
  }
}