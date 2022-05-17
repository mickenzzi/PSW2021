import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginRequest } from 'src/app/model/login-request';
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

  constructor(private router: Router, private userService: UserService, private tokenStorage: TokenStorageService) { }

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.isLoggedIn = true;
    }
  }

  onSubmit(): void {
    const data = {
			username : this.form.username,
			password : this.form.password
		}

    this.userService.login(data).subscribe(
      data => {
        this.tokenStorage.saveToken(data.Token);
        this.tokenStorage.saveUser(data.LoggedUser);

        this.isLoginFailed = false;
        this.isLoggedIn = true;
        this.errorMessage = ""
        this.router.navigate(['/client'])
      },
      err => {
        this.errorMessage = "Invalid username or password.";
        this.isLoginFailed = true;
      }
    );
  }
  goToRegistration() {
    this.router.navigate(['/registration'])
  }

}
