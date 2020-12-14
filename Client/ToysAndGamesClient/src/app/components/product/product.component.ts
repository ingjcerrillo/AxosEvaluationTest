import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription } from 'rxjs';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';
import { ProductEditComponent } from '../product-edit/product-edit.component';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.sass']
})
export class ProductComponent implements OnInit, OnDestroy {

  displayedColumns:string[] = ['select', 'id', 'name', 'description', 'ageRestriction', 'company', 'price', 'actions'];
  dataSource = new MatTableDataSource<Product>([]);
  selection = new SelectionModel<Product>(true, []);
  currentDialog!: MatDialogRef<ProductEditComponent>;
  private dataSourceSubscription! : Subscription;

  constructor(private dialog: MatDialog, private productService:ProductService) {
  }

  ngOnDestroy(): void {
    if(this.dataSourceSubscription){
      this.dataSourceSubscription.unsubscribe();
    }
  }

  private loadProductTable():void {
    this.dataSourceSubscription = this.productService.getAll().subscribe(e => {
      this.dataSource.data = e;
    }, error => {}, () => {
      this.dataSourceSubscription.unsubscribe();
    });
  }

  ngOnInit(): void {
    this.loadProductTable();
  }

  openModal(row?: Product):void {
    this.currentDialog = this.dialog.open(ProductEditComponent, {
      data: row,
      disableClose: true
    });

    this.currentDialog.beforeClosed().subscribe(e => {
      if(e){
        this.loadProductTable();
      }
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.dataSource.data.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: Product): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.id + 1}`;
  }

}
