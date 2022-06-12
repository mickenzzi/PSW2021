import { HttpErrorResponse } from '@angular/common/http';
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
  selector: 'app-doctor',
  templateUrl: './doctor.component.html',
  styleUrls: ['./doctor.component.css']
})
export class DoctorComponent implements OnInit {
  currentUser: User = new User();
  terms: TermResponse[] = [];
  terms1: TermResponse[] = [];
  specialist: Doctor[] = [];
  selectedTerm: Term = new Term();
  selectedTerm1: Term = new Term();
  termRequest: TermRequest = new TermRequest();
  selectedDoctor: Doctor = new Doctor();

  doctorPriority: boolean = false;

  //show doctor terms
  flag1: boolean = false;
  //show term reservation form
  flag2: boolean = false;
  //show finally reservation
  flag3: boolean = false;
  isFailed: boolean = false;
  errorMessage: string = "";

  constructor(private termService: TermService, private auth: TokenStorageService, private router: Router, private doctorService: DoctorService) { }

  ngOnInit(): void {
    if (this.auth.getToken() == null) {
      this.logout();
    }
    else {
      this.currentUser = this.auth.getUser();
      this.getSpecialistDoctor();
    }
  }

  logout() {
    this.auth.signOut();
    this.router.navigate(['/'])
    alert('User is not logged in.')
  }

  signout() {
    this.auth.signOut();
    this.router.navigate(['/'])
  }

  getDoctorTerms() {
    this.terms = []
    this.termService.getAllDoctorTerms(this.currentUser.Id).subscribe(response => {
      for(let term of response){
        if(!term.IsRejected){
          this.terms.push(term);
        }
      }
      this.flag1 = true;
    })
  }

  selectTerm(term: TermResponse) {
    if(this.flag2){
      this.flag2 = false;
    }
    else {
      this.flag2 = true;
    }
    this.selectedTerm.Id = term.Id;
    this.selectedTerm.UserId = term.TermUser?.Id;
    this.selectedTerm.DoctorId = term.TermDoctor?.Id;
    this.selectedTerm.DateTimeTerm = term.DateTimeTerm;
  }

  selectTerm1(term: TermResponse) {
    this.selectedTerm1.Id = term.Id;
    this.selectedTerm1.UserId = term.TermUser?.Id;
    this.selectedTerm1.DoctorId = term.TermDoctor?.Id;
    this.selectedTerm1.DateTimeTerm = term.DateTimeTerm;
  }

  showTerms(){
    if(this.flag1) {
      this.flag1 = false;
      this.flag2 = false;
      this.flag3 = false;
      this.selectedTerm = new Term();
      this.selectedTerm1 = new Term();
      this.selectedDoctor = new Doctor();
    }
    else {
    this.getDoctorTerms();
    }
  }

  deleteTerm(id: string) {
      this.termService.delete(id).subscribe(response => {
        alert("You are successfully delete term.");
        this.getDoctorTerms();
      }, (error: HttpErrorResponse) => {
        alert("You can't delete future terms.")
      })
  }

  getSpecialistDoctor() {
    this.doctorService.getAllSpecialist().subscribe(data => {
      this.specialist = data;
    })
  }

  changeDoctor(doctor: Doctor) {
    this.selectedDoctor = doctor;
  }

  cancel() {
    this.flag2 = false;
    this.isFailed = false;
    this.termRequest = new TermRequest();
    this.selectedDoctor = new Doctor();
    this.selectedTerm = new Term();
  }

  reserveTerm() {
    if (this.selectedDoctor.Id === null || this.selectedDoctor.Id === undefined) {
      this.isFailed = true;
      this.errorMessage = "Choose a doctor."
    }
    else if (this.termRequest.startDate === undefined || this.termRequest.endDate === undefined ||
      this.termRequest.startDate === null || this.termRequest.endDate === null) {
      this.isFailed = true;
      this.errorMessage = "Some fields are empty."
    }
    else {
      if (Date.parse(this.termRequest.startDate) > 0 && Date.parse(this.termRequest.endDate) > 0) {
        let now = new Date();
        let start = new Date(this.termRequest.startDate);
        if (start >= now) {
          this.termRequest.doctorId = this.selectedDoctor.Id;
          this.termRequest.doctorPriority = this.doctorPriority;
          this.termRequest.userId = this.selectedTerm.UserId;
          this.termService.schedule(this.termRequest).subscribe(data => {
            this.terms1 = data;
            this.errorMessage = ""
            this.flag3 = true;
            this.flag2 = false;
            if (this.terms.length === 0) {
              this.isFailed = true;
              this.errorMessage = "There is no free terms."
            }
          }, (error: HttpErrorResponse) => {
            this.isFailed = true;
            this.errorMessage = "You can reserve term only at full hours."
          });
        }
        else {
          this.isFailed = true;
          this.errorMessage = "You can't reserve time at past.";
        }
      }
      else {
        this.isFailed = true;
        this.errorMessage = "Invalid data format.";
      }
    }
  }

  createTerm() {
    if (this.selectedTerm1.Id === null || this.selectedTerm1.Id === undefined) {
      this.isFailed = true;
      this.errorMessage = "Choose a term."
    }
    else {
      this.termService.reserve(this.selectedTerm1).subscribe(data => {
        this.isFailed = false;
        this.selectedTerm1 = new Term();
        this.termRequest = new TermRequest();
        this.selectedDoctor = new Doctor();
        this.flag3 = false;
        alert("You are successfully reserve a term.");
      })
    }
  }

  cancelReservation() {
    this.flag3 = false;
    this.flag2 = false;
    this.flag1 = true;
    this.isFailed = false;
    this.selectedTerm1 = new Term();
  }
}
