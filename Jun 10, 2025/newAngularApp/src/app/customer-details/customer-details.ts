import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-customer-details',
  imports: [FormsModule],
  templateUrl: './customer-details.html',
  styleUrl: './customer-details.css'
})

export class CustomerDetails {
  name: string;
  like: number = 0;  
  LikeClassName : string;
  DislikeClassName : string;
  dislike: number = 0;
  constructor() {
    this.name = "";
    this.LikeClassName = "bi bi-hand-thumbs-up";
    this.DislikeClassName = "bi bi-hand-thumbs-down";
  }
  clickLike() {
    this.LikeClassName = "bi bi-hand-thumbs-up-fill"
    this.like++;
  }
  clickDislike() {
    this.DislikeClassName = "bi bi-hand-thumbs-down-fill";
    this.dislike++;
  }
  reset() {
    this.like = 0;
    this.dislike = 0;
  }
  submitName(uname : string){
    this.name = uname;
    console.log(this.name);
  }
}
