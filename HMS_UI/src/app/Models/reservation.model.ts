import { Bill } from "./bill.model";
import { Payment } from "./payment.model";
import { Rate } from "./rate.model";
import { Room } from "./room.model";

export interface Reservation {
    reservationId: number;
    numberOfAdults: number;
    numberOfChildren: number;
    day: string;
    checkInDate: Date;
    checkOutDate: Date;
    numberOfNights: number;
    status: string;
    roomId?: number | null;
    room?: Room | null;
    rateId?: number | null;
    rate?: Rate | null;
    paymentId?: number | null;
    payment?: Payment | null;
    billId?: number | null;
    bill?: Bill | null;
  }
  
  