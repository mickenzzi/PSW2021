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
  selectedTermForReject: Term = new Term();
  doctorPriority: boolean = false;
  currentUser: User = new User();

  patientTerms: TermResponse[] = [];
  patientTerms1: TermResponse[] = [];

  //create term form
  flag1: boolean = false;
  //select term from lists
  flag2: boolean = false;
  //show terms
  flag3: boolean = false;
  isFailed: boolean = false;
  errorMessage: string = "";

  constructor(private termService: TermService, private auth: TokenStorageService, private router: Router, private doctorService: DoctorService) {
    this.nonSpecialist = [];
    this.specialist = [];
    this.doctors = [];
    this.terms = [];
    this.patientTerms = [];
  }

  ngOnInit(): void {
    if (this.auth.getToken() == null) {
      this.logout();
    }
    else {
      this.currentUser = this.auth.getUser();
      this.getSpecialistDoctor();
      this.getNonSpecialistDoctor();
      this.getAllDoctors();
      this.getPatientTerms();
    }
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
          this.termRequest.userId = this.currentUser.Id;
          this.termService.schedule(this.termRequest).subscribe(data => {
            this.terms = data;
            this.errorMessage = ""
            this.flag2 = true;
            this.flag1 = false;
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

  logout() {
    this.auth.signOut();
    this.router.navigate(['/'])
    alert('User is not logged in.')
  }

  showTermReserveForm() {
    this.flag1 = true;
    this.flag2 = false;
    this.flag3 = false;
    this.isFailed = false;
    this.termRequest = new TermRequest();
    this.selectedDoctor = new Doctor();
    this.selectedTermForReject = new Term();
    this.selectedTerm = new Term();
  }

  cancel() {
    this.flag1 = false;
    this.flag3 = false;
    this.isFailed = false;
    this.termRequest = new TermRequest();
    this.selectedDoctor = new Doctor();
    this.selectedTermForReject = new Term();
    this.selectedTerm = new Term();
  }

  getSpecialistDoctor() {
    this.doctorService.getAllSpecialist().subscribe(data => {
      this.specialist = data;
    })
  }

  getNonSpecialistDoctor() {
    this.doctorService.getAllNonSpecialist().subscribe(data => {
      this.nonSpecialist = data;
    })
  }

  getAllDoctors() {
    this.doctorService.getAllDoctors().subscribe(data => {
      this.doctors = data;
    })
  }

  getPatientTerms() {
    this.patientTerms1 = [];
    this.termService.getAllPatientTerms(this.currentUser.Id).subscribe(response => {
      this.patientTerms = response;
      for(let term of this.patientTerms){
        if(!term.IsRejected){
          this.patientTerms1.push(term);
        }
      }
    })
  }


  changeDoctor(doctor: Doctor) {
    this.selectedDoctor = doctor;
  }

  selectTerm(term: TermResponse) {
    this.selectedTerm.Id = term.Id;
    this.selectedTerm.UserId = term.TermUser?.Id;
    this.selectedTerm.DoctorId = term.TermDoctor?.Id;
    this.selectedTerm.DateTimeTerm = term.DateTimeTerm;
  }

  selectTermForReject(term: TermResponse) {
    this.selectedTermForReject.Id = term.Id;
    this.selectedTermForReject.UserId = term.TermUser?.Id;
    this.selectedTermForReject.DoctorId = term.TermDoctor?.Id;
    this.selectedTermForReject.DateTimeTerm = term.DateTimeTerm;
  }


  createTerm() {
    if (this.selectedTerm.Id === null || this.selectedTerm.Id === undefined) {
      this.isFailed = true;
      this.errorMessage = "Choose a term."
    }
    else {
      this.termService.reserve(this.selectedTerm).subscribe(data => {
        this.isFailed = false;
        this.getPatientTerms();
        this.selectedTerm = new Term();
        this.termRequest = new TermRequest();
        this.selectedDoctor = new Doctor();
        this.flag2 = false;
        alert("You are successfully reserve a term.");
      })
    }
  }

  rejectTerm(id: string) {
    if (this.selectedTermForReject.Id.length === 0) {
      this.isFailed = true;
      this.errorMessage = "Select term."
    }
    else {
      this.termService.reject(id).subscribe(response => {
        alert("You are successfully rejected term.");
        this.getPatientTerms();
        this.isFailed = false;
        this.selectedTermForReject = new Term();
      }, (error: HttpErrorResponse) => {
        alert("You can't reject your term because the differnce between your term date and now is lower than 48 hours.");
      })
    }
  }

  cancelReservation() {
    this.flag3 = false;
    this.flag2 = false;
    this.flag1 = true;
    this.isFailed = false;
    this.selectedTerm = new Term();
  }

  showTerms() {
    this.flag3 = true;
    this.flag1 = false;
    this.flag2 = false;
    this.termRequest = new TermRequest();
    this.selectedDoctor = new Doctor();
    this.selectedTermForReject = new Term();
    this.selectedTerm = new Term();
  }

  signout() {
    this.auth.signOut();
    this.router.navigate(['']);
  }

  redirectToClientTerms(){
    this.router.navigate(['/clientTerms'])
  }
}
