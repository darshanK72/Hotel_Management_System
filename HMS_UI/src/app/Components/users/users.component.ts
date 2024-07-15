import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/Models/user.model';
import { OwnerService } from 'src/app/Services/owner.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  userForm: FormGroup;
  users: User[] = [];
  submitType = 'Add';
  showForm = false;

  constructor(
    private fb: FormBuilder,
    private ownerService: OwnerService
  ) {
    this.userForm = this.fb.group({
      userId: [0],
      username: ['', [Validators.required, Validators.maxLength(50)]],
      password: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      roleId: [0, [Validators.required]],
      departmentId: [0, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.ownerService.getUsers().subscribe(
      (users: User[]) => {
        this.users = users;
      },
      (error) => {
        console.log(error);
        window.alert('Error loading users: ' + error.message);
      }
    );
  }

  onSave(): void {
    if (this.userForm.valid) {
      const user: User = {
        userId: this.userForm.get('userId')?.value,
        username: this.userForm.get('username')?.value,
        password: this.userForm.get('password')?.value,
        email: this.userForm.get('email')?.value,
        roleId: this.userForm.get('roleId')?.value,
        departmentId: this.userForm.get('departmentId')?.value,
      };

      if (this.submitType === 'Add') {
        this.ownerService.createUser(user).subscribe(
          (createdUser: User) => {
            this.users.push(createdUser);
            this.onCancel();
          },
          (error) => {
            window.alert('Error creating user: ' + error.message);
          }
        );
      } else {
        this.ownerService.updateUser(user.userId, user).subscribe(
          () => {
            const index = this.users.findIndex((u) => u.userId === user.userId);
            this.users[index] = user;
            this.onCancel();
          },
          (error) => {
            window.alert('Error updating user: ' + error.message);
          }
        );
      }
    }
  }

  onEdit(user: User): void {
    this.submitType = 'Update';
    this.showForm = true;
    this.userForm.patchValue(user);
  }

  onDelete(userId: number): void {
    this.ownerService.deleteUser(userId).subscribe(
      () => {
        this.users = this.users.filter((u) => u.userId !== userId);
      },
      (error) => {
        window.alert('Error deleting user: ' + error.message);
      }
    );
  }

  onNew(): void {
    this.submitType = 'Add';
    this.showForm = true;
    this.userForm.reset({ userId: 0 });
  }

  onCancel(): void {
    this.showForm = false;
    this.userForm.reset();
  }
}
