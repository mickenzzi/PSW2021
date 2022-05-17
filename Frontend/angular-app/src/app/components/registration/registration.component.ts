import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { RegistrationRequest } from 'src/app/model/registration-request';
import { User } from 'src/app/model/user';
import { TokenStorageService } from 'src/app/service/token-storage.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  user: User = new User()
  passwordCheck: boolean = false;
  inputError: string = ""
  
  constructor(private router: Router, private userService: UserService, private tokenStorage: TokenStorageService) { }
  

  ngOnInit(): void {
  }

  onSubmit(): void {
    if(this.user.firstName?.length == 0 || this.user.firstName == undefined || this.user.lastName == undefined || this.user.lastName?.length == 0 ||
       this.user.username?.length == 0 || this.user.username == undefined || this.user.password1 == undefined ||
      this.user.password1?.length == 0 || this.user.password?.length == 0 || this.user.password == undefined) {
        this.passwordCheck = true;
        this.inputError = "Invalid data. Some fields are empty."
      }
    else {
      if(this.user.password.length < 5){
        this.passwordCheck = true;
        this.inputError = "Invalid data. Password should have minimum 5 characters."
      }
      else if(this.user.password1 != this.user.password){
        this.passwordCheck = true;
        this.inputError = "Passwords don't match."
      }
      else {
        this.passwordCheck = false;
        this.inputError = ""
        const data = {
          firstName: this.user.firstName,
          lastName: this.user.lastName,
          username: this.user.username,
          password: this.user.password
        }

        this.userService.registration(data).subscribe(() => {
          alert('Successfully registration!')
          this.router.navigate(['/'])
        })
      }

    }
  }

}
