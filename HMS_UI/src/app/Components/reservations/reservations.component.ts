import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Guest } from 'src/app/Models/guest.model';
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
  guests:Guest[] = [];
  submitType = 'Add';
  showForm = false;
  isServerError = false;
  errorMessage:any;
  constructor(
    private fb: FormBuilder,
    private receptionService: ReceptionistService
  ) {
    this.reservationForm = this.fb.group({
      reservationId: [0],
      numberOfAdults: [0, Validators.required],
      numberOfChildren: [0, Validators.required],
      checkInDate: ['', Validators.required],
      checkOutDate: ['', Validators.required],
      day:[''],
      status:[''],
      roomId: [null],
      guestId: [null],
      rateId: [null],
      paymentId: [null],
      billId: [null],
      numberofNights:[0]
    });
  }

  ngOnInit(): void {
    this.loadReservations();
    this.loadRooms();
    this.loadGuests();
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

      const checkInDate = this.reservationForm.get('checkInDate')?.value;
      const checkOutDate = this.reservationForm.get('checkOutDate')?.value;

      const reservation: Reservation = {
        reservationId: this.reservationForm.get('reservationId')?.value,
        numberOfAdults: this.reservationForm.get('numberOfAdults')?.value,
        numberOfChildren: this.reservationForm.get('numberOfChildren')?.value,
        checkInDate: checkInDate ? new Date(checkInDate).toISOString() : null,
        checkOutDate: checkOutDate ? new Date(checkOutDate).toISOString() : null,
        roomId: this.reservationForm.get('roomId')?.value,
        guestId: this.reservationForm.get('guestId')?.value,
        rateId: this.reservationForm.get('rateId')?.value,
        paymentId: this.reservationForm.get('paymentId')?.value,
        billId: this.reservationForm.get('billId')?.value,
        day: this.reservationForm.get('day')?.value,
        status: this.reservationForm.get('status')?.value,
        numberOfNights : this.reservationForm.get('numberOfNights')?.value
      };

      if (this.submitType === 'Add') {
        this.receptionService.createReservation(reservation).subscribe(
          (createdReservation: Reservation) => {
            // this.reservations.push(createdReservation);
            this.loadReservations();
            this.onCancel();
            this.isServerError = false;
            this.errorMessage = '';
          },
          (error) => {
            // window.alert('Error creating reservation: ' + error);
            this.isServerError = true;
            this.errorMessage = error.error;
          }
        );
      } else {
        const reservationId = reservation.reservationId;
        console.log(reservation);
        this.receptionService.updateReservation(reservationId, reservation).subscribe(
          () => {
            const index = this.reservations.findIndex((r) => r.reservationId === reservationId);
            if (index !== -1) {
              // this.reservations[index] = reservation;
              this.loadReservations();
              this.onCancel();
              this.isServerError = false;
              this.errorMessage = '';
            }
          },
          (error) => {
            // window.alert('Error updating reservation: ' + error.message);
            this.isServerError = true;
            this.errorMessage = error.error;
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
        this.isServerError = false;
        this.errorMessage = '';
      },
      (error) => {
        this.isServerError = true;
        this.errorMessage = error.error;
        // window.alert('Error deleting reservation: ' + error.message);
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
        console.log(this.rooms);
        this.isServerError = false;
        this.errorMessage = '';
      },
      (error) => {
        console.log(error);
        // window.alert('Error loading rooms: ' + error.message);
        
        this.isServerError = true;
        this.errorMessage = error.error;
      }
    );
  }

  loadGuests(): void {
    this.receptionService.searchGuests().subscribe(
      (guests: Guest[]) => {
        this.guests = guests;
        console.log(this.guests);
        this.isServerError = false;
        this.errorMessage = '';
      },
      (error) => {
        console.log(error);
        // window.alert('Error loading rooms: ' + error.message);
        
        this.isServerError = true;
        this.errorMessage = error.error;
      }
    );
  }
}
