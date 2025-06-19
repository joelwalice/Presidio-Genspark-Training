import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-user',
  imports: [RouterOutlet],
  templateUrl: './user.html',
  styleUrl: './user.css'
})
export class User implements OnInit {
  uname : string = "";
  router = inject(ActivatedRoute);
  ngOnInit():void{
    this.uname = this.router.snapshot.params["uname"] as string;
    console.log(this.uname);
  }
}
