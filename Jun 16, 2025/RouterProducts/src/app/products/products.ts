import { Component, ElementRef, HostListener, OnInit, ViewChild, viewChild } from '@angular/core';
import { ProductService } from '../services/ProductService';
import { ProductModel } from '../models/ProductModel';
import { Product } from "../product/product";
import { CartItem } from '../models/CardItem';
import { FormsModule } from '@angular/forms';
import { debounce, debounceTime, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-products',
  imports: [Product, FormsModule, CommonModule],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products implements OnInit {
  products:ProductModel[]=[];
  cartItems:CartItem[] =[];
  cartCount:number =0;
  searchString:string="";
  searchSubject = new Subject<string>();
  loading:boolean = false;
  limit=10;
  skip=0;
  total =0;
  constructor(private productService:ProductService){

  }
  handleSearchProducts(){
    // console.log(this.searchString)
    this.searchSubject.next(this.searchString);
  }

  handleAddToCart(event:Number)
  {
    console.log("Handling add to cart - "+event)
    let flag = false;
    for(let i=0;i<this.cartItems.length;i++)
    {
      if(this.cartItems[i].Id==event)
      {
         this.cartItems[i].Count++;
         flag=true;
      }
    }
    if(!flag)
      this.cartItems.push(new CartItem(event,1));
    this.cartCount++;
  }
  ngOnInit(): void {
    
    this.productService.getAllProducts().subscribe(
      {
        next:(data:any)=>{
         this.products = data.products as ProductModel[];
         console.log(document.body.scrollHeight, window.innerHeight);
        },
        error:(err)=>{},
        complete:()=>{}
      }
    )
    this.searchSubject.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      tap(()=>this.loading=true),
      switchMap(query=>this.productService.getProductSearchResult(query,this.limit,this.skip)),
       tap(()=>this.loading=false)).subscribe({
        next:(data:any)=>{
                    this.products = data.products as ProductModel[];
                    this.total = data.total;
                    console.log(this.total)
                    }
      });

  }
  @HostListener('window:scroll',[])
  onScroll():void
  {
    const scrollPosition = window.innerHeight + window.pageYOffset;
    const threshold = document.body.offsetHeight-100;
    if(scrollPosition>=threshold && this.products?.length<this.total)
    {
      console.log(scrollPosition);
      console.log(threshold)
      
      this.loadMore();
    }
  }
  loadMore(){
    this.loading = true;
    this.skip += this.limit;
    this.productService.getProductSearchResult(this.searchString,this.limit,this.skip)
        .subscribe({
          next:(data:any)=>{
            this.products = [...this.products, ...data.products];
            console.log(this.products);
            this.loading = false;
          }
        })
  }
  scrollToTop():void {
    window.scrollTo({
      top : 0,
      behavior : 'smooth'
    })
  }
}