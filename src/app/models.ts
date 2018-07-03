import { DateTime } from "ionic-angular/umd";

export class Cart {
    ItemCount: number;
    TotalAmount: number;
    ReferenceCode: string;
}

export class Product {
    id: number;
    name: string;
    price: number;
    desc: string;
    thumbURL: string;
    hasStock: boolean;
    stock: number;
    isChecked: boolean;
}

export class Order {
    id: string;
    orderedProducts: Product[];
    username: string;
    orderDate: DateTime;
    paidDate: DateTime;
    referenceCode: string;
}

export class OrderProductResponse {
    referenceCode: string;
    message: string;
}
