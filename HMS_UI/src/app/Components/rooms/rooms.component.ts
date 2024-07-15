import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Room, RoomType, Status } from 'src/app/Models/room.model';
import { ManagerService } from 'src/app/Services/manager.service';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css'],
})
export class RoomsComponent implements OnInit {
  roomForm: FormGroup;
  rooms: Room[] = [];
  submitType = 'Add';
  showForm = false;

  roomTypes: { value: RoomType; label: string }[] = [
    { value: RoomType.SingleBed, label: 'Single Bed' },
    { value: RoomType.DoubleBed, label: 'Double Bed' },
    { value: RoomType.HoneyMoonSweet, label: 'Honey Moon Sweet' }
  ];

  roomStatus: { value: Status; label: string }[] = [
    { value: Status.Available, label: 'Available' },
    { value: Status.Reserved, label: 'Reserved' }
  ];
  

  constructor(
    private fb: FormBuilder,
    private managerService: ManagerService // Inject ManagerService
  ) {
    this.roomForm = this.fb.group({
      roomId: [0],
      roomNumber: ['', [Validators.required, Validators.maxLength(10)]],
      status: ['', [Validators.required, Validators.maxLength(20)]],
      perNightCharges: [0, [Validators.required]],
      roomType: [0, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.loadRooms();
  }

  loadRooms(): void {
    this.managerService.getRooms().subscribe(
      (rooms: Room[]) => {
        this.rooms = rooms;
      },
      (error) => {
        console.log(error);
        window.alert('Error loading rooms: ' + error.message);
      }
    );
  }

  getRoomTypeLabel(roomType: any): string {
    const found = this.roomTypes.find(type => type.value === roomType);
    return found ? found.label : 'Unknown';
  }
  onSave(): void {
    if (this.roomForm.valid) {
      const room: Room = {
        roomId: this.roomForm.get('roomId')?.value,
        roomNumber: this.roomForm.get('roomNumber')?.value,
        status: this.roomForm.get('status')?.value,
        perNightCharges: this.roomForm.get('perNightCharges')?.value,
        roomType: parseInt(this.roomForm.get('roomType')?.value),
      };

      if (this.submitType === 'Add') {
        this.managerService.createRoom(room).subscribe(
          (createdRoom: Room) => {
            this.rooms.push(createdRoom);
            this.onCancel();
          },
          (error) => {
            window.alert('Error creating room: ' + error.message);
            // Handle error (e.g., show a notification to the user)
          }
        );
      } else {
        this.managerService.updateRoom(room.roomId, room).subscribe(
          () => {
            const index = this.rooms.findIndex((r) => r.roomId === room.roomId);
            this.rooms[index] = room;
            this.onCancel();
          },
          (error) => {
            window.alert('Error updating room: ' + error.message);
            // Handle error (e.g., show a notification to the user)
          }
        );
      }
    }
  }

  onEdit(room: Room): void {
    this.submitType = 'Update';
    this.showForm = true;
    this.roomForm.patchValue(room);
  }

  onDelete(roomId: number): void {
    this.managerService.deleteRoom(roomId).subscribe(
      () => {
        this.rooms = this.rooms.filter((r) => r.roomId !== roomId);
      },
      (error) => {
        window.alert('Error deleting room: ' + error.message);
      }
    );
  }

  onNew(): void {
    this.submitType = 'Add';
    this.showForm = true;
    this.roomForm.reset({ roomId: 0 });
  }

  onCancel(): void {
    this.showForm = false;
    this.roomForm.reset();
  }
}
