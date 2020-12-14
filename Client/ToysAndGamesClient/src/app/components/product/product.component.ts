import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subscription, pipe, forkJoin } from 'rxjs';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
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

  constructor(private dialog: MatDialog, private productService:ProductService, private snackbar:MatSnackBar) {
  }

  ngOnDestroy(): void {
    if(this.dataSourceSubscription){
      this.dataSourceSubscription.unsubscribe();
    }
  }

  /** Consumes productservice getall and fills product table */
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

  /** Opens a modal containing the product form */
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

  /**
   * Confirms user wanted to delete items, if so proceeds to delete them.
   */
  public confirmDelete():void {
    if(this.selection.hasValue()){
      let amountItems = this.selection.selected.length;
      this.dialog.open(ConfirmationDialogComponent, {
        data: { title: 'Confirm', message: `Delete (${amountItems}) items?` },
        disableClose: true
      })
      .afterClosed()
      .subscribe(acccepted => {
        if (acccepted){
          this.deleteSelected();
        }
      });
    }
  }

  /**
   * Gathers all items to delete and creates a request to the server for each one.
   */
  private deleteSelected() {
    let deleteRequests$: Array<Observable<boolean>> = new Array();
    this.selection.selected.forEach(p => {
      deleteRequests$.push(
        this.productService.deleteProduct(p.id)
      );
    });

    forkJoin(deleteRequests$).subscribe(e => {
      let deletedTotal = e.filter(b => b === true).length;
      this.snackbar.open(`A total of (${deletedTotal}) products were deleted`, 'Ok', {
        duration: 2000
      });
    }, err => {
      console.log(err);
    }, () => {
      this.loadProductTable();
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

  checkboxLabel(row?: Product): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.id + 1}`;
  }

}
