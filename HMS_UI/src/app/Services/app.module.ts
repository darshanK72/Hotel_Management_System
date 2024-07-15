import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { LoginComponent } from './Auth/login/login.component';
import { RegisterComponent } from './Auth/register/register.component';
import { LogoutComponent } from './Auth/logout/logout.component';
import { HeaderComponent } from './Layout/header/header.component';
import { FooterComponent } from './Layout/footer/footer.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { environment } from '../environments/environment';
import { ForgetComponent } from './Auth/forget/forget.component';
import { ToastrModule } from 'ngx-toastr'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NotfoundComponent } from './Layout/notfound/notfound.component';
import { AdminDashboardComponent } from './Components/admin-dashboard/admin-dashboard.component';
import { BookCardComponent } from './Components/book-card/book-card.component';
import { BookDetailComponent } from './Components/book-detail/book-detail.component'

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    LogoutComponent,
    HeaderComponent,
    FooterComponent,
    DashboardComponent,
    ForgetComponent,
    NotfoundComponent,
    AdminDashboardComponent,
    BookCardComponent,
    BookDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ToastrModule.forRoot(),
    BrowserAnimationsModule
  ],
  providers: [
  
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
