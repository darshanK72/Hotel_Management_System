import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  userData$!: Observable<any>;
  ownerRole!: boolean;
  managerRole!: boolean;
  receptionRole!: boolean;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.userData$ = this.authService.loggedInUser$;
    this.userData$.subscribe((r) => {
      if (r.role == 'Owner') {
        this.ownerRole = true;
        this.managerRole = true;
        this.receptionRole = true;
      } else if (r.role == 'Manager') {
        this.ownerRole = false;
        this.managerRole = true;
        this.receptionRole = true;
      } else {
        this.ownerRole = false;
        this.managerRole = false;
        this.receptionRole = true;
      }
    });
  }
}
