import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { HistoryPage } from '../history/history';
import { PaymentPage } from '../payment/payment';
import { Platform } from 'ionic-angular';
import { HTTP } from '@ionic-native/http';
import { Product, Cart, OrderProductResponse } from '../../app/models';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {

  public Host: string = "https://mixcoffee-dev.azurewebsites.net";
  public Username: string = "somsor-academy@outlook.co.th";

  public Products: Product[];
  public MyCart: Cart = {
    ItemCount: 0,
    TotalAmount: 0,
    ReferenceCode: ""
  };

  public errorMsg: string;

  constructor(public navCtrl: NavController, public platform: Platform, private http: HTTP) {
    this.platform.ready().then((readySource) => {
      if (!this.platform.is('core') && !this.platform.is('mobileweb')) {
        this.http.get(this.Host + "/Product/Get", {}, {})
          .then(data => {
            this.Products = JSON.parse(data.data);
          })
          .catch(error => {
            this.errorMsg = error;
          });
      }
      else {
        this.Products = this.mockProducts;
      }
    });
  }

  GoHistory() {
    this.navCtrl.push(HistoryPage);
  }

  GoPayment() {
    var orderedProducts = this.Products.filter(it => it.isChecked);
    if (orderedProducts.length == 0) {
      alert("กรุณาเลือกเมนูก่อนสั่งซื้อ");
      return;
    }

    if (!this.platform.is('core') && !this.platform.is('mobileweb')) {
      var body = {
        "orderedProducts": orderedProducts.map(it => {
          return { "key": it.id, "value": 1 }
        }),
        "username": this.Username
      };

      var headers = { 'Content-Type': 'application/json' };

      this.http.setDataSerializer('json');
      this.http.post(this.Host + "/api/Order/OrderProduct", body, headers)
        .then(data => {
          var response: OrderProductResponse = JSON.parse(data.data);
          alert(response.message);
          if (response.referenceCode != null && response.referenceCode != "") {
            this.navCtrl.push(PaymentPage, { ReferenceCode: response.referenceCode });
          }
        }).catch(error => {
          this.errorMsg = "Error: [" + error + "][" + error.error + "][" + error.status + "][" + error.headers + "]";
        });
    } else {
      this.navCtrl.push(PaymentPage, { CheckoutPrice: this.MyCart.TotalAmount, ReferenceCode: "A00001", OrderedProducts: orderedProducts.map(it => it.name) });
    }
  }

  SelectItem() {
    var selectedProducts = this.Products.filter(it => it.isChecked);
    var priceList = selectedProducts.map(it => it.price);

    this.MyCart.ItemCount = selectedProducts.length;
    this.MyCart.TotalAmount = priceList.reduce((sum, price) => sum + price, 0);
  }

  public mockProducts: Product[] = [{
    id: 1,
    name: "chocolate cake",
    price: 10,
    desc: "chocolate cake",
    thumbURL: this.Host + "/images/chocolate-cake.png",
    hasStock: true,
    stock: 10,
    isChecked: false
  }, {
    id: 2,
    name: "panna cotta",
    price: 20,
    desc: "panna cotta",
    thumbURL: this.Host + "/images/panna-cotta.png",
    hasStock: true,
    stock: 10,
    isChecked: false
  }, {
    id: 2,
    name: "cheesecake",
    price: 30,
    desc: "cheesecake",
    thumbURL: this.Host + "/images/cheese-cake.png",
    hasStock: true,
    stock: 10,
    isChecked: false
  }, {
    id: 2,
    name: "crepe cake",
    price: 40,
    desc: "crepe cake",
    thumbURL: this.Host + "/images/crepe-cake.png",
    hasStock: true,
    stock: 10,
    isChecked: false
  }, {
    id: 2,
    name: "hot coffee",
    price: 50,
    desc: "hot coffee",
    thumbURL: this.Host + "/images/hot-coffee.png",
    hasStock: true,
    stock: 10,
    isChecked: false
  }, {
    id: 2,
    name: "Cake",
    price: 60,
    desc: "ice coffee",
    thumbURL: this.Host + "/images/ice-coffee.png",
    hasStock: true,
    stock: 10,
    isChecked: false
  }];
}

