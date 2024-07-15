import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Department } from 'src/app/Models/department.model';
import { OwnerService } from 'src/app/Services/owner.service';

@Component({
  selector: 'app-departments',
  templateUrl: './departments.component.html',
  styleUrls: ['./departments.component.css']
})
export class DepartmentsComponent implements OnInit {
  departmentForm: FormGroup;
  departments: Department[] = [];
  submitType = 'Add';
  showForm = false;

  constructor(
    private fb: FormBuilder,
    private ownerService: OwnerService
  ) {
    this.departmentForm = this.fb.group({
      departmentId: [0],
      name: ['', [Validators.required, Validators.maxLength(30)]],
      description: ['', [Validators.required, Validators.maxLength(100)]],
    });
  }

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.ownerService.getDepartments().subscribe(
      (departments: Department[]) => {
        this.departments = departments;
      },
      (error) => {
        console.log(error);
        window.alert('Error loading departments: ' + error.message);
      }
    );
  }

  onSave(): void {
    if (this.departmentForm.valid) {
      const department: Department = {
        departmentId: this.departmentForm.get('departmentId')?.value,
        name: this.departmentForm.get('name')?.value,
        description: this.departmentForm.get('description')?.value,
      };

      if (this.submitType === 'Add') {
        this.ownerService.createDepartment(department).subscribe(
          (createdDepartment: Department) => {
            this.departments.push(createdDepartment);
            this.onCancel();
          },
          (error) => {
            window.alert('Error creating department: ' + error.message);
          }
        );
      } else {
        this.ownerService.updateDepartment(department.departmentId, department).subscribe(
          () => {
            const index = this.departments.findIndex((d) => d.departmentId === department.departmentId);
            this.departments[index] = department;
            this.onCancel();
          },
          (error) => {
            window.alert('Error updating department: ' + error.message);
          }
        );
      }
    }
  }

  onEdit(department: Department): void {
    this.submitType = 'Update';
    this.showForm = true;
    this.departmentForm.patchValue(department);
  }

  onDelete(departmentId: number): void {
    this.ownerService.deleteDepartment(departmentId).subscribe(
      () => {
        this.departments = this.departments.filter((d) => d.departmentId !== departmentId);
      },
      (error) => {
        window.alert('Error deleting department: ' + error.message);
      }
    );
  }

  onNew(): void {
    this.submitType = 'Add';
    this.showForm = true;
    this.departmentForm.reset({ departmentId: 0 });
  }

  onCancel(): void {
    this.showForm = false;
    this.departmentForm.reset();
  }
}
