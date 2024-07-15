import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './Layout/header/header.component';
import { FooterComponent } from './Layout/footer/footer.component';
import { LoginComponent } from './Components/login/login.component';
import { LogoutComponent } from './Components/logout/logout.component';
import { NotfoundComponent } from './Layout/notfound/notfound.component';
import { HomeComponent } from './Layout/home/home.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RoomsComponent } from './Components/rooms/rooms.component';
import { StaffsComponent } from './Components/staffs/staffs.component';
import { UsersComponent } from './Components/users/users.component';
import { DepartmentsComponent } from './Components/departments/departments.component';
import { GuestsComponent } from './Components/guests/guests.component';
import { ReservationsComponent } from './Components/reservations/reservations.component';
import { JwtInterceptor } from './Services/jwt.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    LogoutComponent,
    NotfoundComponent,
    HomeComponent,
    RoomsComponent,
    StaffsComponent,
    UsersComponent,
    DepartmentsComponent,
    GuestsComponent,
    ReservationsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
