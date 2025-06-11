import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-first',
  imports: [
    FormsModule
  ],
  templateUrl: './first.html',
  styleUrl: './first.css'
})
export class First {
  value : string;
  isSelected : boolean;
  className : string;
  constructor(){
    this.value = "Hai!";
    this.isSelected = false;
    this.className = "bi bi-balloon"
  }
  onButtonClick(){
    if(this.value == "Hai!"){
      this.value = "Hello!";
    }
    else{
      this.value = "Hai!";
    }
  }
  onClickAlert(uname : string){
    alert(`Alerting the User ${uname}`);
    this.value = uname;
  }
  onToggle(){
    this.isSelected = !this.isSelected;
    if(this.isSelected){
      this.className = "bi bi-balloon-heart-fill";
    }
    else{
      this.className = "bi bi-balloon";
    }
  }
}
