import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Term } from 'src/app/model/term';
import { TermResponse } from 'src/app/model/term-response';
import { User } from 'src/app/model/user';
import { Comment } from 'src/app/model/comment';
import { DoctorService } from 'src/app/service/doctor.service';
import { TermService } from 'src/app/service/term.service';
import { FeedbackService } from 'src/app/service/feedback.service';
import { TokenStorageService } from 'src/app/service/token-storage.service';
import { CommentService } from 'src/app/service/comment.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Feedback } from 'src/app/model/feedback';

@Component({
  selector: 'app-client-terms',
  templateUrl: './client-terms.component.html',
  styleUrls: ['./client-terms.component.css']
})
export class ClientTermsComponent implements OnInit {

  constructor(private termService: TermService, private auth: TokenStorageService, private router: Router, private doctorService: DoctorService, private commentService: CommentService, private feedbackService: FeedbackService) { }

  currentUser: User = new User();
  futureTerms: TermResponse[] = []
  completedTerms: TermResponse[] = []
  selectedTerm: Term = new Term();
  comment: Comment = new Comment();
  feedback: Feedback = new Feedback();

  //future
  flag1: boolean = false;
  //completed
  flag2: boolean = false;
  //show comment
  flag3: boolean = false;
  //feedback
  flag4: boolean = false;

  ngOnInit(): void {
    if (this.auth.getToken() == null) {
      this.signout();
    }
    else {
      this.currentUser = this.auth.getUser();
    }
  }

  redirectHome() {
    this.router.navigate(['/client'])
  }

  signout() {
    this.auth.signOut();
    this.router.navigate([''])
  }

  getPatientFutureTerms() {
    this.termService.getAllPatientFutureTerms(this.currentUser.Id).subscribe(response => {
      this.futureTerms = response;
      this.flag1 = true;
    })
  }

  getPatientCompletedTerms() {
    this.termService.getAllPatientCompletedTerms(this.currentUser.Id).subscribe(response => {
      this.completedTerms = response;
      this.flag2 = true;
    })
  }

  showCompleted() {
    this.flag1 = false;
    this.flag4 = false;
    if (this.flag2) {
      this.flag2 = false;
    }
    else {
      this.getPatientCompletedTerms();
    }
  }

  showFuture() {
    this.flag2 = false;
    this.flag4 = false;
    if (this.flag1) {
      this.flag1 = false;
    }
    else {
      this.getPatientFutureTerms();
    }
  }


  selectTerm(term: TermResponse) {
    this.selectedTerm.Id = term.Id;
    this.selectedTerm.UserId = term.TermUser?.Id;
    this.selectedTerm.DoctorId = term.TermDoctor?.Id;
    this.selectedTerm.DateTimeTerm = term.DateTimeTerm;
    this.flag3 = true;
  }

  cancel() {
    this.flag3 = false;
    this.flag4 = false;
    this.feedback = new Feedback();
    this.comment = new Comment();
  }

  createComment() {
    this.comment.TermId = this.selectedTerm.Id;
    this.comment.UserId = this.currentUser.Id;
    this.commentService.createComment(this.comment).subscribe(response => {
      this.comment = new Comment();
      this.flag3 = false;
      alert("You are successfully create comment.");
    }, (error: HttpErrorResponse) => {
      alert("You are already created comment for this term.");
    })
  }

  createFeedback() {
    this.feedback.UserId = this.currentUser.Id;
    this.feedbackService.createFeedback(this.feedback).subscribe(response => {
      this.feedback = new Feedback();
      this.flag4 = false;
      alert("You are successfully create feedback.");
    }, (error: HttpErrorResponse) => {
      alert("You are already created too many feedbacks.");
    })
  }

  showFeedbackForm(){
    this.flag4 = true;
    this.flag1 = false;
    this.flag2 = false;
    this.flag3 = false;
  }

}
