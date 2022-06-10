import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TermResponse } from 'src/app/model/term-response';
import { User } from 'src/app/model/user';
import { DoctorService } from 'src/app/service/doctor.service';
import { TermService } from 'src/app/service/term.service';
import { TokenStorageService } from 'src/app/service/token-storage.service';

@Component({
  selector: 'app-client-terms',
  templateUrl: './client-terms.component.html',
  styleUrls: ['./client-terms.component.css']
})
export class ClientTermsComponent implements OnInit {

  constructor(private termService: TermService, private auth: TokenStorageService, private router: Router, private doctorService: DoctorService) { }

  currentUser: User = new User();
  futureTerms: TermResponse[] = []
  completedTerms: TermResponse[] = []

  ngOnInit(): void {
    if (this.auth.getToken() == null) {
      this.signout();
    }
    else {
      this.currentUser = this.auth.getUser();
    }
  }

  redirectHome(){
    this.router.navigate(['/client'])
  }

  signout(){
    this.auth.signOut();
    this.router.navigate([''])
  }

  getPatientFutureTerms() {
    this.termService.getAllPatientFutureTerms(this.currentUser.Id).subscribe(response => {
      this.futureTerms = response;
    })
  }

  getPatientCompletedTerms() {
    this.termService.getAllPatientCompletedTerms(this.currentUser.Id).subscribe(response => {
      this.completedTerms = response;
    })
  }


}
