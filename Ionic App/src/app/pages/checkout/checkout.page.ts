import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { CustomerInfo } from '../../interfaces/customerInfo.interface';
import { CheckoutService } from '../../services/checkout.service';
import { ModalController } from '@ionic/angular';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.page.html',
  styleUrls: ['./checkout.page.scss'],
})
export class CheckoutPage implements OnInit {

  form1: FormGroup;
  @Input() amount: number; // componentProps

  slideOpts = {
    allowSlidePrev: false,
    allowSlideNext: false
  };

  constructor(
    private fb: FormBuilder,
    private checkoutService: CheckoutService,
    private modalCtrl: ModalController
  ) { }

  ngOnInit() {
    this.form1 = this.fb.group({
      cardNumber: ['', [Validators.required, Validators.minLength(16), Validators.maxLength(16)]],
      expireDate: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(5)]],
      cvc: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(3)]]
    });
  }

  async onSubmit() {
    const { cardNumber, cvc, expireDate } = this.form1.value;
    const customerInfo: CustomerInfo = {
      cardNumber,
      cvc,
      amount: 4200,
      monthExpiration: expireDate.split('/')[0],
      yearExpiration: '20' + expireDate.split('/')[1]
    };
    const resultPay = await this.checkoutService.payment(customerInfo);
    this.modalCtrl.dismiss(resultPay);
  }

  cancelar() {
    this.modalCtrl.dismiss();
  }

  get cvc() {
    return this.form1.get('cvc');
  }

  get cardNumber() {
    return this.form1.get('cardNumber');
  }

  get expireDate() {
    return this.form1.get('expireDate');
  }

}
