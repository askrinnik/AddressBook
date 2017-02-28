import { Component } from "@angular/core";
import { NgModel } from "@angular/forms";

export class Item {
  purchase: string;
  done: boolean;
  price: number;

  constructor(purchase: string, price: number) {
    this.purchase = purchase;
    this.price = price;
    this.done = false;
  }
}

@Component({
  selector: "purchase-app",
  templateUrl: "./app/app.component.html",
  styles: [`
        input.ng-touched.ng-invalid {border:solid red 2px;}
        input.ng-touched.ng-valid {border:solid green 2px;}
    `]
})
export class AppComponent {
  items: Item[] =
  [
    { purchase: "Хлеб", done: false, price: 15.9 },
    { purchase: "Масло", done: false, price: 60 },
    { purchase: "Картофель", done: true, price: 22.6 },
    { purchase: "Сыр", done: false, price: 310 }
  ];

  addItem(purchaseModel: NgModel, priceModel: NgModel): void {

    const purchase = purchaseModel.value;
    // this.purchase is also available

    const price = priceModel.value;

    if (purchase == null || purchase.trim() === "")
      return;
    if (price == null)
      return;

    this.items.push(new Item(purchase, price));
    purchaseModel.reset();
    priceModel.reset();
  }}