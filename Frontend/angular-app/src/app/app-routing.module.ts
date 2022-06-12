import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientComponent } from './components/client/client.component';
import { HomeComponent } from './components/home/home.component';
import { RegistrationComponent } from './components/registration/registration.component';
import { ClientTermsComponent } from './components/client-terms/client-terms.component';
import { DoctorComponent } from './components/doctor/doctor.component';
import { AdminComponent } from './components/admin/admin.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'
  },
  { 
    path: 'registration',
    component: RegistrationComponent
  },
  { path: 'client',
    component: ClientComponent
  },
  {
    path: 'doctor',
    component: DoctorComponent
  },
  { 
    path: 'clientTerms',
    component: ClientTermsComponent
  },
  {
    path: 'admin',
    component: AdminComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
