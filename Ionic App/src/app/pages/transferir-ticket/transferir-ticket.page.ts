import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { ReservaTicketService } from '../../services/reserva-tickets.service';
import { LoadingController, ToastController } from '@ionic/angular';


@Component({
  selector: 'app-transferir-ticket',
  templateUrl: './transferir-ticket.page.html',
  styleUrls: ['./transferir-ticket.page.scss'],
})
export class TransferirTicketPage implements OnInit {

  transferirForm: FormGroup;
  @Input() cantidad: number; // componentProps
  @Input() id: number; // componentProps

  slideOpts = {
    allowSlidePrev: false,
    allowSlideNext: false
  };

  constructor(
    private fb: FormBuilder,
    private modalCtrl: ModalController,
    private reservaTicketService: ReservaTicketService,
    private toastCtrl: ToastController
  ) { }

  ngOnInit() {    
    this.transferirForm = this.fb.group({
      numeroDocumento: ['', [Validators.required, Validators.min(1000000000)]],
      cantidadTransferir: ['', [Validators.required, Validators.min(1), Validators.max(this.cantidad)]]
    });
  }

  async onSubmit() {
    console.log(this.transferirForm.get('numeroDocumento').value);
    console.log(this.transferirForm.get('cantidadTransferir').value);
    const ok = this.reservaTicketService.transferirTicket(this.id, String(this.transferirForm.get('numeroDocumento').value), this.transferirForm.get('cantidadTransferir').value);
    if (ok) {
      this.presentToast("Ticket transferido satisfactoriamente");
      this.modalCtrl.dismiss(true);
    } else {
      this.presentToast("ERROR: Ha ocurrido un error, intentelo de nuevo");
    }
  }

  cancelar() {
    this.modalCtrl.dismiss();
  }

  async presentToast(message: string) {
    const toast = await this.toastCtrl.create({
      message,
      position: 'bottom',
      duration: 3000
    });
    await toast.present();
  }

  get numeroDocumento() {
    return this.transferirForm.get('numeroDocumento');
  }

  get cantidadTransferir() {
    return this.transferirForm.get('cantidadTransferir');
  }
}
