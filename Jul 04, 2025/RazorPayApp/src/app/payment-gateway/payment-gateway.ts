import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { environment } from '../../../environments/environment';

declare var Razorpay: any;

@Component({
  selector: 'app-payment-gateway',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './payment-gateway.html',
  styleUrl: './payment-gateway.css',
})
export class PaymentGateway {
  paymentForm: FormGroup;
  isLoading: boolean = false;

  constructor() {
    this.paymentForm = new FormGroup({
      amount: new FormControl('', [Validators.required, Validators.min(1)]),
      customerName: new FormControl('', [Validators.required, Validators.minLength(3)]),
      email: new FormControl('', [Validators.required, Validators.email]),
      contactNumber: new FormControl('', [Validators.required, Validators.pattern('^[0-9]{10}$')]),
    });
  }

  public get amount() {
    return this.paymentForm.get('amount');
  }

  public get customerName() {
    return this.paymentForm.get('customerName');
  }

  public get email() {
    return this.paymentForm.get('email');
  }

  public get contactNumber() {
    return this.paymentForm.get('contactNumber');
  }

  onSubmit() {
    if (this.paymentForm.valid) {
      const paymentData = this.paymentForm.value;
      this.isLoading = true;

      const options = {
        key: environment.KEY_ID,
        amount: paymentData.amount * 100,
        currency: 'INR',
        name: paymentData.customerName,
        description: 'Test UPI Payment',
        prefill: {
          name: paymentData.customerName,
          email: paymentData.email,
          contact: paymentData.contactNumber,
        },
        handler: (response: any) => {
          this.isLoading = false;
          this.showSuccess(response.razorpay_payment_id);
        },
        modal: {
          ondismiss: () => {
            this.isLoading = false;
            this.showFailure();
          },
        },
        theme: {
          color: '#3399cc',
        },
      };

      const rzp = new Razorpay(options);
      rzp.open();
    } else {
      alert('Please fill out the form correctly.');
    }
  }

  showSuccess(razorpay_payment_id: string) {
    alert(`Payment successful! Razorpay Payment ID: ${razorpay_payment_id}`);
  }

  showFailure() {
    alert('Payment was cancelled or failed. Please try again.');
  }
}
