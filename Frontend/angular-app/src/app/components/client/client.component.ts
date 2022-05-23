import { DOCUMENT } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Doctor } from 'src/app/model/doctor';
import { TermRequest } from 'src/app/model/term-request';
import { User } from 'src/app/model/user';
import { DoctorService } from 'src/app/service/doctor.service';
import { TermService } from 'src/app/service/term.service';
import { TokenStorageService } from 'src/app/service/token-storage.service';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})
export class ClientComponent implements OnInit {

  termRequest: TermRequest = new TermRequest();
  nonSpecialist: Doctor[];
  specialist: Doctor[];
  selectedDoctor: Doctor = new Doctor();
  doctorPriority: boolean = false;
  currentUser: User = new User();

  flag1: boolean = false;
  isFailed: boolean = false;
  errorMessage: string = "";

  constructor(private termService: TermService, private auth: TokenStorageService, private router: Router, private doctorService: DoctorService) { 
    this.nonSpecialist = [];
    this.specialist = [];
  }

  ngOnInit(): void {
    if(this.auth.getToken() == null) {
      this.logout();
    }
    else {
      this.getSpecialistDoctor();
      this.getNonSpecialistDoctor();
      this.currentUser = this.auth.getUser();
    }
  }

  reserveTerm(){
    if(this.termRequest.startDate === undefined || this.termRequest.endDate === undefined || this.selectedDoctor === null){
      this.isFailed = true;
      this.errorMessage = "Chose a doctor."
    }
    else {
    if(Date.parse(this.termRequest.startDate) > 0 && Date.parse(this.termRequest.endDate) > 0){
      this.termRequest.doctorId = this.selectedDoctor.Id;
      this.termRequest.doctorPriority = this.doctorPriority;
      this.termRequest.userId = this.currentUser.Id;
      this.termService.schedule(this.termRequest).subscribe(data => {
        console.log(data);
      });
    }
    else{
      this.isFailed = true;
      this.errorMessage = "Invalid data format.";
    }
  }
  }

  logout() {
    this.auth.signOut();
    this.router.navigate(['/'])
    alert('User is not logged in.')
  }

  showTermReserveForm() {
    this.flag1 = true;
  }

  cancel() {
    this.flag1 = false;
  }

  getSpecialistDoctor() {
    this.doctorService.getAllSpecialist().subscribe( data => {
      this.specialist = data;
    })
  }

  getNonSpecialistDoctor() {
    this.doctorService.getAllNonSpecialist().subscribe( data => {
      this.nonSpecialist = data;
    })
  }

  changeDoctor(doctor: Doctor){
    this.selectedDoctor = doctor;
  }
}
