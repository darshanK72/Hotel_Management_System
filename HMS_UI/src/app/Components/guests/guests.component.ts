import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Guest } from 'src/app/Models/guest.model';
import { ReceptionistService } from 'src/app/Services/reception.service';

@Component({
  selector: 'app-guests',
  templateUrl: './guests.component.html',
  styleUrls: ['./guests.component.css']
})
export class GuestsComponent implements OnInit {
  guestForm: FormGroup;
  guests: Guest[] = [];
  submitType = 'Add';
  showForm = false;
  filterParams: any = {};

  constructor(
    private fb: FormBuilder,
    private receptionService: ReceptionistService
  ) {
    this.guestForm = this.fb.group({
      guestId: [0],
      name: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      gender: ['', [Validators.required, Validators.maxLength(10)]],
      address: ['', [Validators.required, Validators.maxLength(200)]],
      phoneNumber: ['', [Validators.required, Validators.maxLength(10), Validators.pattern('[0-9]*')]],
      memberCode: [''],
      reservationId: ['']
    });
  }

  ngOnInit(): void {
    this.loadGuests();
  }

  loadGuests(): void {
    this.receptionService.searchGuests().subscribe(
      (guests: Guest[]) => {
        this.guests = guests;
      },
      (error) => {
        console.log(error);
        window.alert('Error loading guests: ' + error.message);
      }
    );
  }

  applyFilters(): void {
    const { name, email, phoneNumber, memberCode } = this.filterParams;
    this.receptionService.searchGuests(name, email, phoneNumber, memberCode).subscribe(
      (filteredGuests: Guest[]) => {
        this.guests = filteredGuests;
      },
      (error) => {
        console.log(error);
        window.alert('Error filtering guests: ' + error.message);
      }
    );
  }

  resetFilters(): void {
    this.filterParams = {};
    this.loadGuests();
  }

  onSave(): void {
    if (this.guestForm.valid) {
      const guest: Guest = {
        guestId: this.guestForm.get('guestId')?.value,
        name: this.guestForm.get('name')?.value,
        email: this.guestForm.get('email')?.value,
        gender: this.guestForm.get('gender')?.value,
        address: this.guestForm.get('address')?.value,
        phoneNumber: this.guestForm.get('phoneNumber')?.value,
        memberCode: this.guestForm.get('memberCode')?.value,
        reservationId: this.guestForm.get('reservationId')?.value
      };

      if (this.submitType === 'Add') {
        this.receptionService.createGuest(guest).subscribe(
          (createdGuest: Guest) => {
            this.guests.push(createdGuest);
            this.onCancel();
          },
          (error) => {
            window.alert('Error creating guest: ' + error.message);
          }
        );
      } else {
        this.receptionService.updateGuest(guest.guestId, guest).subscribe(
          () => {
            const index = this.guests.findIndex((g) => g.guestId === guest.guestId);
            this.guests[index] = guest;
            this.onCancel();
          },
          (error) => {
            window.alert('Error updating guest: ' + error.message);
          }
        );
      }
    }
  }

  onEdit(guest: Guest): void {
    this.submitType = 'Update';
    this.showForm = true;
    this.guestForm.patchValue(guest);
  }

  onDelete(guestId: number): void {
    this.receptionService.deleteGuest(guestId).subscribe(
      () => {
        this.guests = this.guests.filter((g) => g.guestId !== guestId);
      },
      (error) => {
        window.alert('Error deleting guest: ' + error.message);
      }
    );
  }

  onNew(): void {
    this.submitType = 'Add';
    this.showForm = true;
    this.guestForm.reset({ guestId: 0 });
  }

  onCancel(): void {
    this.showForm = false;
    this.guestForm.reset();
  }
}
