import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.sass']
})
export class ProductEditComponent implements OnInit {

  productDetail!: Product;

  constructor(private productService:ProductService,
    private snackbar: MatSnackBar,
    public dialogRef: MatDialogRef<ProductEditComponent>,
    @Inject(MAT_DIALOG_DATA) data: Product) {
      if(data){
        this.productDetail = { ...data };
      }else{
        this.productDetail = this.newProduct();
      }
  }

  /**
   * Decides whether the product should be updated or created based on its id.
   */
  public saveProduct(): void {
    if(this.productDetail.id > 0){
      this.updateProduct();
    }else{
      this.createProduct();
    }
  }

  private updateProduct() : void {
    this.productService.updateProduct(this.productDetail).subscribe(successful => {
      if(successful){
        this.snackbar.open('Product updated successfully', 'Ok', { duration: 2000, politeness: 'polite' });
        this.dialogRef.close(true);
      }
    });
  }

  private createProduct() : void {
    this.productService.createProduct(this.productDetail).subscribe(successful => {
      this.snackbar.open('Product created successfully', 'Ok', { duration: 2000, politeness: 'polite' });
      this.dialogRef.close(true);
    });
  }

  /** generates a new product */
  private newProduct(): Product{
    return {
      id: 0,
      name: '',
      description: '',
      ageRestriction: 0,
      company: '',
      price: 1
    };
  }

  ngOnInit(): void {
  }



}
