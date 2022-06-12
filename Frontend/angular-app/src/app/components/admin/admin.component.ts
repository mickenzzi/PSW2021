import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Feedback } from 'src/app/model/feedback';
import { FeedbackResponse } from 'src/app/model/feedbackResponse';
import { User } from 'src/app/model/user';
import { FeedbackService } from 'src/app/service/feedback.service';
import { TokenStorageService } from 'src/app/service/token-storage.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  feedbacks: Feedback[] = []
  currentUser: User = new User();
  flag1: boolean = false;

  constructor(private router: Router, private userService: UserService, private feedbackService: FeedbackService, private auth: TokenStorageService) { }

  ngOnInit(): void {
    if (this.auth.getToken() == null) {
      this.signout();
    }
    else {
      this.currentUser = this.auth.getUser();
    }
  }

  getAllFeedbacks() {
    this.feedbackService.getAllFeedbacks().subscribe(response => {
      this.feedbacks = response;
      this.flag1 = true;
    })
  }

  signout() {
    this.auth.signOut();
    this.router.navigate(['']);
  }

  showFeedbacks(){
    if(this.flag1){
      this.flag1 = false;
    }
    else{
      this.getAllFeedbacks();
    }
  }

  setFeedbackVisible(feedback: Feedback){
    feedback.IsVisible = true;
    this.feedbackService.updateFeedback(feedback).subscribe(response => {
      this.getAllFeedbacks();
    })
  }

  setFeedbackInvisible(feedback: Feedback){
    feedback.IsVisible = false;
    this.feedbackService.updateFeedback(feedback).subscribe(response => {
      this.getAllFeedbacks();
    })
  }

}
