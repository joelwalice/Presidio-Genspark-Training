
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ContactUs } from '../models/contact-us.model';

@Injectable({
  providedIn: 'root'
})
export class ContactUsService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5067/api/ContactUs';

  constructor(){
    this.Headers();
  }

  Headers(){
    return {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('authToken')}`
    };
  }

  send(message: ContactUs) {
    return this.http.post(this.baseUrl, message, { headers: this.Headers() });
  }
}
