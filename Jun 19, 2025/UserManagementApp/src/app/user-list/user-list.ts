import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { UserService } from '../services/UserService';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css'],
  imports:[CommonModule]
})
export class UserListComponent implements OnInit {
  users$!: Observable<User[]>;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.users$ = this.userService.getUsers();
  }
}