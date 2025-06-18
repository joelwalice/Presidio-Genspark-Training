import { Routes } from '@angular/router';
import { First } from './first/first';
import { Home } from './home/home';
import { User } from './user/user';
import { Login } from './login/login';
import { Profile } from './profile/profile';
import { AuthGuard } from './auth-guard';

export const routes: Routes = [
    {path : "first", component : First},
    {path : "login", component : Login},
    {path : "user/:uname", component : User, children : [
        {path : "home", component : Home},
    ]},
    {path:'profile',component:Profile,canActivate:[AuthGuard]}
];
