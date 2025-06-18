import { Component } from '@angular/core';
import { UserService } from '../services/UserService';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList {
  users: any[] = [];

  filteredUsers: any[] = [];
 
  selectedGender: string = '';

  roleSearch: string = '';

  nameSearch: string = '';
 
  constructor(private userService: UserService) {}
 
  ngOnInit(): void {

    this.userService.users$.subscribe((data) => {

      this.users = data;

      this.filteredUsers = data;

    });
 
    this.userService.fetchData();

  }
 
  filterUsers(): void {

    this.filteredUsers = this.users.filter(user => {

      const genderMatch = this.selectedGender === '' || user.gender === this.selectedGender;

      const roleMatch = user.company?.title?.toLowerCase().includes(this.roleSearch.toLowerCase());

      const nameMatch = (`${user.firstName} ${user.lastName}`).toLowerCase().includes(this.nameSearch.toLowerCase());
 
      return genderMatch && roleMatch && nameMatch;

    });

  }
 
  resetFilters(): void {

    this.selectedGender = '';

    this.roleSearch = '';

    this.nameSearch = '';

    this.filteredUsers = [...this.users];

  }

}
