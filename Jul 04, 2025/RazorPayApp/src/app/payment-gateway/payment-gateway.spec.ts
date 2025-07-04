import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentGateway } from './payment-gateway';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('PaymentGateway', () => {
  let component: PaymentGateway;
  let fixture: ComponentFixture<PaymentGateway>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentGateway, FormsModule]
    })
      .compileComponents();

    fixture = TestBed.createComponent(PaymentGateway);
    component = fixture.componentInstance;
    fixture.detectChanges();

    (window as any).Razorpay = class {
      constructor(public options: any) { }
      open() { }
    };
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a title', () => {
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('h1').textContent).toContain('RazorPay Testing Form');
  });

  it('should call the makePayment method on button click', () => {
    spyOn(component, 'onSubmit');
    const form = fixture.debugElement.query(By.css('form')).nativeElement;
    form.dispatchEvent(new Event('submit')); 
    fixture.detectChanges();

    expect(component.onSubmit).toHaveBeenCalled();
  });

  it('should validate the form inputs', () => {
    component.paymentForm.setValue({
      amount: '', 
      customerName: '',
      email: '',
      contactNumber: ''
    });
    expect(component.paymentForm.valid).toBeFalsy();
    component.paymentForm.setValue({
      amount: 100,
      customerName: 'Joel W',
      email: '',
      contactNumber: '1234567890'
    });
    expect(component.paymentForm.valid).toBeFalsy();
    component.paymentForm.setValue({
      amount: 100,
      customerName: 'Joel W',
      email: 'joel@gmail.com',
      contactNumber: '1234567890'
    });
    expect(component.paymentForm.valid).toBeTruthy();
  });

  it('should call the makePayment method with correct parameters', () => {
    spyOn(component, 'onSubmit').and.callThrough();
    component.paymentForm.setValue({
      amount: 100,
      customerName: 'Joel W',
      email: 'joelw@gmail.com',
      contactNumber: '1234567890'
    });
    component.onSubmit();
    expect(component.onSubmit).toHaveBeenCalledWith();
  });
});
