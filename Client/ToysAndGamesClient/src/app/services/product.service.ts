import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';

const baseURL = 'http://localhost:60868/api/Product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http:HttpClient) { }

  getAll() : Observable<Product[]> {
    return this.http.get<Product[]>(baseURL + '/GetAll');
  }

  getById(id:number) : Observable<Product> {
    return this.http.get<Product>(baseURL + `/GetById/${id}`);
  }

  updateProduct(product: Product) : Observable<boolean> {
    return this.http.put<boolean>(baseURL + `/Update`, product);
  }

  createProduct(product: Product) : Observable<boolean> {
    return this.http.post<boolean>(baseURL + `/Create`, product);
  }

  deleteProducts(products:Product[]) : void {
    let deleteTasks$ = [];
    products.forEach(product => {
      deleteTasks$.push(
        this.http.delete(baseURL + `/Delete/${product.id}`)
      );
    });
  }

}
