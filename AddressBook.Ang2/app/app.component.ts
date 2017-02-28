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
  templateUrl: "./app/app.component.html"
})
export class AppComponent {
  items: Item[] =
  [
    { purchase: "Хлеб", done: false, price: 15.9 },
    { purchase: "Масло", done: false, price: 60 },
    { purchase: "Картофель", done: true, price: 22.6 },
    { purchase: "Сыр", done: false, price: 310 }
  ];

  addItem(purchaseModel: NgModel, priceInputElement: HTMLInputElement): void {

    const purchase = purchaseModel.value;
    // this.purchase is also available

    const price = priceInputElement.value;

    if (purchase == null || purchase.trim() === "")
      return;
    if (price == null)
      return;

    this.items.push(new Item(purchase, Number(price)));
    purchaseModel.reset();
    priceInputElement.value = null;
  }}