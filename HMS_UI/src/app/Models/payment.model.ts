export interface Payment {
  paymentId?: number;
  totalAmount: number;
  paymentTime: Date;
  creditCardDetails: string;
  billId?: number;
  guestId?: number;
  reservationId?: number;
}
