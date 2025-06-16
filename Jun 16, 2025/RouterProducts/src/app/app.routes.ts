import { Routes } from '@angular/router';
import { Products } from './products/products';
import { Hello } from './hello/hello';
import { About } from './about/about';

export const routes: Routes = [
    {path : "hello", component : Hello},
    {path : "products", component : Products},
    {path : "about", component : About}
];
