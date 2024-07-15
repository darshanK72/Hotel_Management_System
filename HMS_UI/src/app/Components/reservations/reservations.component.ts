import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Reservation } from 'src/app/Models/reservation.model';
import { Room } from 'src/app/Models/room.model';
import { ReceptionistService } from 'src/app/Services/reception.service';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent implements OnInit {
  reservations: Reservation[] = [];
  reservationForm: FormGroup;
  rooms: Room[] = [];
  submitType = 'Add';
  showForm = false;

  constructor(
    private fb: FormBuilder,
    private receptionService: ReceptionistService
  ) {
    this.reservationForm = this.fb.group({
      reservationId: [0],
      numberOfAdults: [0, Validators.required],
      numberOfChildren: [0, Validators.required],
      day: ['', Validators.required],
      checkInDate: ['', Validators.required],
      checkOutDate: ['', Validators.required],
      numberOfNights: [0, Validators.required],
      status: ['', Validators.required],
      roomId: [null], // Example: Add related fields as needed
      rateId: [null],
      paymentId: [null],
      billId: [null]
    });
  }

  ngOnInit(): void {
    this.loadReservations();
    this.loadRooms();
  }

  loadReservations(): void {
    this.receptionService.getReservations().subscribe(
      (reservations: Reservation[]) => {
        this.reservations = reservations;
      },
      (error) => {
        console.log(error);
        window.alert('Error loading reservations: ' + error.message);
      }
    );
  }

  onSave(): void {
    console.log(this.reservationForm.value);
    if (this.reservationForm.valid) {
      const reservation: Reservation = {
        reservationId: this.reservationForm.get('reservationId')?.value,
        numberOfAdults: this.reservationForm.get('numberOfAdults')?.value,
        numberOfChildren: this.reservationForm.get('numberOfChildren')?.value,
        day: this.reservationForm.get('day')?.value,
        checkInDate: new Date(this.reservationForm.get('checkInDate')?.value),
        checkOutDate: new Date(this.reservationForm.get('checkOutDate')?.value),
        numberOfNights: this.reservationForm.get('numberOfNights')?.value,
        status: this.reservationForm.get('status')?.value,
        roomId: this.reservationForm.get('roomId')?.value,
        rateId: this.reservationForm.get('rateId')?.value,
        paymentId: this.reservationForm.get('paymentId')?.value,
        billId: this.reservationForm.get('billId')?.value
      };

      if (this.submitType === 'Add') {
        this.receptionService.createReservation(reservation).subscribe(
          (createdReservation: Reservation) => {
            this.reservations.push(createdReservation);
            this.onCancel();
          },
          (error) => {
            window.alert('Error creating reservation: ' + error.message);
          }
        );
      } else {
        const reservationId = reservation.reservationId;
        this.receptionService.updateReservation(reservationId, reservation).subscribe(
          () => {
            const index = this.reservations.findIndex((r) => r.reservationId === reservationId);
            if (index !== -1) {
              this.reservations[index] = reservation;
              this.onCancel();
            }
          },
          (error) => {
            window.alert('Error updating reservation: ' + error.message);
          }
        );
      }
    }
  }

  onEdit(reservation: Reservation): void {
    this.submitType = 'Update';
    this.showForm = true;
    this.reservationForm.patchValue(reservation);
  }

  onDelete(reservationId: number): void {
    this.receptionService.deleteReservation(reservationId).subscribe(
      () => {
        this.reservations = this.reservations.filter((r) => r.reservationId !== reservationId);
      },
      (error) => {
        window.alert('Error deleting reservation: ' + error.message);
      }
    );
  }

  onNew(): void {
    this.submitType = 'Add';
    this.showForm = true;
    this.reservationForm.reset({ reservationId: 0 });
  }

  onCancel(): void {
    this.showForm = false;
    this.reservationForm.reset();
  }

  loadRooms(): void {
    this.receptionService.getRooms().subscribe(
      (rooms: Room[]) => {
        this.rooms = rooms;
      },
      (error) => {
        console.log(error);
        window.alert('Error loading rooms: ' + error.message);
      }
    );
  }
}
