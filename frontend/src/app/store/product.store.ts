import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {Product} from "../model/product-model";
import Order = jasmine.Order;
import {ProductService} from "../services/product.service";

@Injectable({
  providedIn: 'root'
})
export class ProductStore {
  // @ts-ignore
  private _products: BehaviorSubject<Product[]> = new BehaviorSubject(Array<Order>());

  public readonly products$: Observable<Order[]> = this._products.asObservable();

  constructor(private productService: ProductService) {
    this.loadAllProducts();
  }

  public loadAllProducts(){
    this.productService.getAllProducts().subscribe(
      products => this._products.next(products),
      error => console.error('ProductStore' + error)
    )
  }

  public updateProducts(updateProducts: Product[]){

  }


}
