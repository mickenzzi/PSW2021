import { DOCUMENT } from '@angular/common';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { ThisReceiver } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Doctor } from 'src/app/model/doctor';
import { Term } from 'src/app/model/term';
import { TermRequest } from 'src/app/model/term-request';
import { TermResponse } from 'src/app/model/term-response';
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
  doctors: Doctor[];
  terms: TermResponse[] = [];
  selectedDoctor: Doctor = new Doctor();
  selectedTerm: Term = new Term();
  doctorPriority: boolean = false;
  currentUser: User = new User();

  flag1: boolean = false;
  flag2: boolean = false;
  isFailed: boolean = false;
  errorMessage: string = "";

  constructor(private termService: TermService, private auth: TokenStorageService, private router: Router, private doctorService: DoctorService) { 
    this.nonSpecialist = [];
    this.specialist = [];
    this.doctors = [];
    this.terms  = [];
  }

  ngOnInit(): void {
    if(this.auth.getToken() == null) {
      this.logout();
    }
    else {
      this.getSpecialistDoctor();
      this.getNonSpecialistDoctor();
      this.getAllDoctors();
      this.currentUser = this.auth.getUser();
    }
  }

  reserveTerm(){
    if(this.selectedDoctor.Id === null || this.selectedDoctor.Id === undefined){
      this.isFailed = true;
      this.errorMessage = "Choose a doctor."
    }
    else if(this.termRequest.startDate === undefined || this.termRequest.endDate === undefined ||
      this.termRequest.startDate === null || this.termRequest.endDate === null ) {
      this.isFailed = true;
      this.errorMessage = "Some fields are empty."
    }
    else {
      if(Date.parse(this.termRequest.startDate) > 0 && Date.parse(this.termRequest.endDate) > 0){
        this.termRequest.doctorId = this.selectedDoctor.Id;
        this.termRequest.doctorPriority = this.doctorPriority;
        this.termRequest.userId = this.currentUser.Id;
        this.termService.schedule(this.termRequest).subscribe(data => {
          this.terms = data;
          if(this.terms.length === 0){
            this.isFailed = true;
            this.errorMessage = "There is no free terms."
          }
        }, (error: HttpErrorResponse) => {
            this.flag1 = true;
            this.flag2 = false;
            this.isFailed = true;
            this.errorMessage = "You can reserve term only at full hours."
        });
        this.flag2 = true;
        this.flag1 = false;
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
    this.flag2 = false;
    this.isFailed = false;
  }

  cancel() {
    this.flag1 = false;
    this.isFailed = false;
    this.termRequest = new TermRequest();
    this.selectedDoctor = new Doctor();
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

  getAllDoctors(){
    this.doctorService.getAllDoctors().subscribe( data => {
      this.doctors = data;
    })
  }


  changeDoctor(doctor: Doctor){
    this.selectedDoctor = doctor;
  }

  selectTerm(term: TermResponse) {
    this.selectedTerm.Id = term.Id;
    this.selectedTerm.UserId = term.TermUser?.Id;
    this.selectedTerm.DoctorId = term.TermDoctor?.Id;
    this.selectedTerm.DateTimeTerm = term.DateTimeTerm;
  }


  createTerm() {
    if(this.selectedTerm.Id === null || this.selectedTerm.Id === undefined){
      this.isFailed = true;
      this.errorMessage = "Choose a term."
    }
    else{
      this.termService.reserve(this.selectedTerm).subscribe(data => {
        this.isFailed = false;
      })
    }
  }

  cancelReservation() {
    this.flag2 = false;
    this.flag1 = true;
    this.isFailed = false;
    this.selectedTerm = new Term();
  }
}
