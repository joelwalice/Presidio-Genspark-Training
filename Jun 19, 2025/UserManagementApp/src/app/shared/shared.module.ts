import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { UserListComponent } from '../user-list/user-list';
import { UserSearchComponent } from '../user-search/user-search.component';
import { UserFormComponent } from '../user-form/user-form';

@NgModule({
  declarations: [
  ],
  imports: [
    UserListComponent,
    UserSearchComponent,
    UserFormComponent,
    CommonModule,
    ReactiveFormsModule,
  ],
  exports: [
    CommonModule,
    ReactiveFormsModule,
    UserListComponent,
    UserSearchComponent
  ]
})
export class SharedModule {}