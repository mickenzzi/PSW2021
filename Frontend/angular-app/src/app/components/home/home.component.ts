import { Component, OnInit } from '@angular/core';
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

  constructor(private userService: UserService, private tokenStorage: TokenStorageService) { }

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
      },
      err => {
        this.errorMessage = err.error.message;
        this.isLoginFailed = true;
      }
    );
  }

  goToRegistration() {

  }

}
