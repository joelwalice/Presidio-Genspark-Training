import { Routes } from '@angular/router';
import { UserList } from './user-list/user-list';
import { UserForm } from './user-form/user-form';
import { Home } from './home/home';

export const routes: Routes = [
    {path : '', component : Home},
    {path : 'user', component : UserList},
    {path : 'adduser', component : UserForm}
];
