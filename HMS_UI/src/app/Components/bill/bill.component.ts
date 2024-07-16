import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Bill } from 'src/app/Models/bill.model';
import { Payment } from 'src/app/Models/payment.model';
import { ReceptionistService } from 'src/app/Services/reception.service';

@Component({
  selector: 'app-bill',
  templateUrl: './bill.component.html',
  styleUrls: ['./bill.component.css']
})
export class BillComponent implements OnInit {
  bills: Bill[] = [];
  selectedBill: Bill | null = null;
  paymentForm: FormGroup;

  constructor(private receptionService: ReceptionistService, private fb: FormBuilder) {
    this.paymentForm = this.fb.group({
      totalAmount: [{ value: '', disabled: true }, Validators.required],
      paymentTime: ['', Validators.required],
      creditCardDetails: ['', [Validators.required, Validators.maxLength(100)]],
      guestId: ['', Validators.required],
      reservationId: ['', Validators.required],
      status:['']
    });
  }

  ngOnInit(): void {
    this.loadBills();
  }

  loadBills(): void {
    this.receptionService.getBills().subscribe((data: Bill[]) => {
      this.bills = data;
      console.log(this.bills);
    });
  }

  onPay(bill: Bill): void {
    this.selectedBill = bill;
    console.log(bill);
    this.paymentForm.setValue({
      totalAmount: bill.price + bill.taxes,
      paymentTime: '',
      creditCardDetails: '',
      guestId: bill.guestId,
      reservationId:bill.reservationId,
      status : bill.status
    });
  }

  onSubmitPayment(): void {
    if (this.paymentForm.valid) {
      const payment: Payment = {
        paymentId: 0,
        totalAmount: this.selectedBill!.price + this.selectedBill!.taxes,
        paymentTime: this.paymentForm.value.paymentTime,
        creditCardDetails: this.paymentForm.value.creditCardDetails,
        billId: this.selectedBill!.billId,
        guestId: this.selectedBill!.guestId ,
        reservationId: this.selectedBill!.reservationId
      };

      this.receptionService.completePayment(payment).subscribe(() => {
        this.loadBills();
        this.selectedBill = null;
        this.paymentForm.reset();
      });
    } else {
      this.paymentForm.markAllAsTouched();
    }
  }

  onCancel(): void {
    this.selectedBill = null;
    this.paymentForm.reset();
  }
}
