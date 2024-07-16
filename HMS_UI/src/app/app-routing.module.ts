import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { LogoutComponent } from './Components/logout/logout.component';
import { afterLoginGuard } from './Services/after-login.guard';
import { beforeLoginGuard } from './Services/before-login.guard';
import { NotfoundComponent } from './Layout/notfound/notfound.component';
import { HomeComponent } from './Layout/home/home.component';
import { RoomsComponent } from './Components/rooms/rooms.component';
import { DepartmentsComponent } from './Components/departments/departments.component';
import { StaffsComponent } from './Components/staffs/staffs.component';
import { UsersComponent } from './Components/users/users.component';
import { GuestsComponent } from './Components/guests/guests.component';
import { ReservationsComponent } from './Components/reservations/reservations.component';
import { BillComponent } from './Components/bill/bill.component';

const routes: Routes = [
  {
    path:'',
    redirectTo:'home',
    pathMatch:'full'
  },
  {
    path:'home',
    component : HomeComponent,
    canActivate:[afterLoginGuard]
  },
  {
    path:'login',
    component:LoginComponent,
    canActivate:[beforeLoginGuard]
  },
  {
    path:'logout',
    component:LogoutComponent,
    canActivate:[afterLoginGuard]
  },
  {
    path:'rooms',
    component:RoomsComponent,
    canActivate:[afterLoginGuard]
  },
  {
    path:'departments',
    component:DepartmentsComponent,
    canActivate:[afterLoginGuard]
  },
  {
    path:'users',
    component:UsersComponent,
    canActivate:[afterLoginGuard]
  },
  {
    path:'guests',
    component:GuestsComponent,
    canActivate:[afterLoginGuard]
  },
  {
    path:'staffs',
    component:StaffsComponent,
    canActivate:[afterLoginGuard]
  },
  {
    path:'reservations',
    component:ReservationsComponent,
    canActivate:[afterLoginGuard]
  },
  {
    path:'bills',
    component:BillComponent,
    canActivate:[afterLoginGuard]
  },
  {
    path:'**',
    component:NotfoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
