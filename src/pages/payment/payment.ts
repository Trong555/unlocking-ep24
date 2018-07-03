import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { Platform } from 'ionic-angular';
import { HTTP } from '@ionic-native/http';
import { Order } from '../../app/models';

@IonicPage()
@Component({
  selector: 'page-payment',
  templateUrl: 'payment.html',
})
export class PaymentPage {

  public Host: string = "https://mixcoffee-dev.azurewebsites.net";
  public Username: string = "somsor-academy@outlook.co.th";

  public CheckoutPrice: number;
  public ReferenceCode: string;
  public OrderedProducts: string[];

  public errorMsg: string;

  constructor(public navCtrl: NavController, public platform: Platform, private http: HTTP, public navParams: NavParams) {
    this.platform.ready().then((readySource) => {
      this.ReferenceCode = navParams.data.ReferenceCode;

      if (!this.platform.is('core') && !this.platform.is('mobileweb')) {
        this.http.get(this.Host + "/api/Order/GetByReferenceCode/" + this.ReferenceCode, {}, {})
          .then(data => {
            var order: Order = JSON.parse(data.data);
            this.CheckoutPrice = order.orderedProducts.map(it => it.price).reduce((sum, price) => sum + price, 0);
            this.OrderedProducts = order.orderedProducts.map(it => it.name);
          })
          .catch(error => {
            this.errorMsg = "Error: [" + error + "][" + error.error + "][" + error.status + "][" + error.headers + "]";
          });
      }
      else {
        this.CheckoutPrice = navParams.data.CheckoutPrice;
        this.OrderedProducts = navParams.data.OrderedProducts;
      }
    });
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad PaymentPage');
  }
}



