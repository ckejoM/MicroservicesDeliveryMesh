import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService } from './services/order.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <h1>Microservices Delivery Mesh</h1>
      <div class="card">
        <input [(ngModel)]="order.customerName" placeholder="Customer Name" />
        <input type="number" [(ngModel)]="order.totalAmount" placeholder="Amount" />
        <button (click)="placeOrder()">Place Order</button>
      </div>
      <p *ngIf="message">{{ message }}</p>
    </div>
  `
})
export class AppComponent {
  order = { customerName: '', totalAmount: 0 };
  message = '';

  constructor(private orderService: OrderService) {}

  placeOrder() {
    this.orderService.createOrder(this.order).subscribe({
      next: (res) => this.message = `Order ${res.id} placed! Checking mesh logs...`,
      error: (err) => this.message = 'Gateway Error: Check if services are running.'
    });
  }
}