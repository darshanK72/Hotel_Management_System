import { Bill } from "./bill.model";

export interface Payment {
    paymentId: number;
    totalAmount: number;
    paymentTime: Date;
    creditCardDetails: string;
    billId?: number | null;
    bill?: Bill | null;
  }
  