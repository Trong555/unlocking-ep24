import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams, DateTime } from 'ionic-angular';
import { Platform } from 'ionic-angular';
import { HTTP } from '@ionic-native/http';
import { Order, Product } from '../../app/models';

/**
 * Generated class for the HistoryPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-history',
  templateUrl: 'history.html',
})
export class HistoryPage {

  public Host: string = "https://mixcoffee-dev.azurewebsites.net";
  public Username: string = "somsor-academy@outlook.co.th";

  public Orders: Order[];

  public errorMsg: string;

  constructor(public navCtrl: NavController, public platform: Platform, private http: HTTP, public navParams: NavParams) {
    this.platform.ready().then((readySource) => {
      if (!this.platform.is('core') && !this.platform.is('mobileweb')) {
        this.http.get(this.Host + "/api/Order/ListByUsername/" + this.Username, {}, {})
          .then(data => {
            this.Orders = JSON.parse(data.data);
          })
          .catch(error => {
            this.errorMsg = "Error: [" + error + "][" + error.error + "][" + error.status + "][" + error.headers + "]";
          });
      }
      else {
      }
    });
  }

  SumPrice(products: Product[]) {
    return products.map(it => it.price).reduce((sum, price) => sum + price, 0);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad HistoryPage');
  }
}
