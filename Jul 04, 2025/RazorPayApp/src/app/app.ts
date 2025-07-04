import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PaymentGateway } from "./payment-gateway/payment-gateway";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PaymentGateway],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'RazorPayApp';
}
