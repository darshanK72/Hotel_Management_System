import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Staff } from 'src/app/Models/staff.model';
import { ManagerService } from 'src/app/Services/manager.service';
import { OwnerService } from 'src/app/Services/owner.service';

@Component({
  selector: 'app-staffs',
  templateUrl: './staffs.component.html',
  styleUrls: ['./staffs.component.css']
})
export class StaffsComponent implements OnInit {
  staffForm: FormGroup;
  staffs: Staff[] = [];
  submitType = 'Add';
  showForm = false;

  constructor(
    private fb: FormBuilder,
    private managerService: ManagerService
  ) {
    this.staffForm = this.fb.group({
      staffId: [0],
      name: ['', [Validators.required, Validators.maxLength(100)]],
      age: ['', [Validators.required]],
      address: ['', [Validators.required, Validators.maxLength(200)]],
      nic: ['', [Validators.required, Validators.maxLength(20)]],
      salary: [0, [Validators.required]],
      designation: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      code: ['', [Validators.required, Validators.maxLength(50)]],
      departmentId: [0, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.loadStaffs();
  }

  loadStaffs(): void {
    this.managerService.getStaffs().subscribe(
      (staffs: Staff[]) => {
        this.staffs = staffs;
      },
      (error) => {
        console.log(error);
        window.alert('Error loading staffs: ' + error.message);
      }
    );
  }

  onSave(): void {
    if (this.staffForm.valid) {
      const staff: Staff = {
        staffId: this.staffForm.get('staffId')?.value,
        name: this.staffForm.get('name')?.value,
        age: this.staffForm.get('age')?.value,
        address: this.staffForm.get('address')?.value,
        nic: this.staffForm.get('nic')?.value,
        salary: this.staffForm.get('salary')?.value,
        designation: this.staffForm.get('designation')?.value,
        email: this.staffForm.get('email')?.value,
        code: this.staffForm.get('code')?.value,
        departmentId: this.staffForm.get('departmentId')?.value,
      };

      if (this.submitType === 'Add') {
        this.managerService.createStaff(staff).subscribe(
          (createdStaff: Staff) => {
            this.staffs.push(createdStaff);
            this.onCancel();
          },
          (error) => {
            window.alert('Error creating staff: ' + error.message);
          }
        );
      } else {
        this.managerService.updateStaff(staff.staffId, staff).subscribe(
          () => {
            const index = this.staffs.findIndex((s) => s.staffId === staff.staffId);
            this.staffs[index] = staff;
            this.onCancel();
          },
          (error) => {
            window.alert('Error updating staff: ' + error.message);
          }
        );
      }
    }
  }

  onEdit(staff: Staff): void {
    this.submitType = 'Update';
    this.showForm = true;
    this.staffForm.patchValue(staff);
  }

  onDelete(staffId: number): void {
    this.managerService.deleteStaff(staffId).subscribe(
      () => {
        this.staffs = this.staffs.filter((s) => s.staffId !== staffId);
      },
      (error) => {
        window.alert('Error deleting staff: ' + error.message);
      }
    );
  }

  onNew(): void {
    this.submitType = 'Add';
    this.showForm = true;
    this.staffForm.reset({ staffId: 0 });
  }

  onCancel(): void {
    this.showForm = false;
    this.staffForm.reset();
  }
}
