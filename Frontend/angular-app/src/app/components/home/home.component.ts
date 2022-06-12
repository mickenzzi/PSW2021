import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Feedback } from 'src/app/model/feedback';
import { FeedbackResponse } from 'src/app/model/feedbackResponse';
import { LoginRequest } from 'src/app/model/login-request';
import { FeedbackService } from 'src/app/service/feedback.service';
import { TokenStorageService } from 'src/app/service/token-storage.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  form: LoginRequest = {
    username: '',
    password: ''
  };
  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage = '';

  allFeedbacks: Feedback[] = [];
  feedbacks: FeedbackResponse[] = [];

  flag1: boolean = false;

  constructor(private router: Router, private userService: UserService, private feedbackService: FeedbackService, private tokenStorage: TokenStorageService) { }

  ngOnInit(): void {
    this.getAllFeedbacks();
    this.tokenStorage.signOut();
    if (this.tokenStorage.getToken()) {
      this.isLoggedIn = true;
    }
  }

  onSubmit(): void {
    const data = {
      username: this.form.username,
      password: this.form.password
    }

    this.userService.login(data).subscribe(
      data => {
        if (data.LoggedUser.IsBlocked) {
          this.isLoginFailed = true;
          this.errorMessage = "Your account is blocked."
        }
        else {
          this.tokenStorage.saveToken(data.Token);
          this.tokenStorage.saveUser(data.LoggedUser);

          this.isLoginFailed = false;
          this.isLoggedIn = true;
          this.errorMessage = ""
          if (data.LoggedUser.Role === "Client") {
            this.router.navigate(['/client'])
          }
          else if (data.LoggedUser.Role === "Doctor") {
            this.router.navigate(['/doctor'])
          }
          else if (data.LoggedUser.Role === "Admin") {
            this.router.navigate(['/admin'])

          }
        }
      },
      err => {
        this.errorMessage = "Invalid username or password.";
        this.isLoginFailed = true;
      }
    );
  }

  getAllFeedbacks() {
    this.feedbackService.getAllFeedbacks().subscribe(response => {
      this.allFeedbacks = response;
      for (let feedback of this.allFeedbacks) {
        let feedbackResponse = new FeedbackResponse();
        feedbackResponse.Id = feedback.Id;
        feedbackResponse.Content = feedback.Content;
        feedbackResponse.Grade = feedback.Grade;
        feedbackResponse.IsPrivate = feedback.IsPrivate;
        feedbackResponse.IsVisible = feedback.IsVisible;
        if (feedback.IsVisible) {
          if (feedback.IsPrivate) {
            feedbackResponse.User = "Anonymous"
          }
          else {
            this.userService.getUserById(feedback.UserId ?? "").subscribe(response => {
              feedbackResponse.User = response.Username;
            })
          }
          this.feedbacks.push(feedbackResponse);
        }
      }
    })
  }

  showFeedbacks() {
    if (this.flag1) {
      this.flag1 = false;
    }
    else {
      this.flag1 = true;
    }
  }

  goToRegistration() {
    this.router.navigate(['/registration'])
  }

}
