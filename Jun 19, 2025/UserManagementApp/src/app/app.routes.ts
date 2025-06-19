import { Routes } from '@angular/router';
import { UserListComponent } from './user-list/user-list';
import { UserFormComponent } from './user-form/user-form';

export const routes: Routes = [
    {path : 'user', component : UserListComponent},
    {path : 'adduser', component : UserFormComponent}
];
