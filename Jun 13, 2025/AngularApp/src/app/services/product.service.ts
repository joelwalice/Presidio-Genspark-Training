import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable()
export class ProductService{
    private http = inject(HttpClient);

    getProduct(){
        return this.http.get('https://dummyjson.com/products')
    }
}