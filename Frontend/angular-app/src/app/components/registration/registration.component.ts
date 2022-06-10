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
    if (this.user.FirstName?.length == 0 || this.user.FirstName == undefined || this.user.LastName == undefined || this.user.LastName?.length == 0 ||
      this.user.Username?.length == 0 || this.user.Username == undefined || this.user.Password1 == undefined ||
      this.user.Password1?.length == 0 || this.user.Password?.length == 0 || this.user.Password == undefined ||
      this.user.Address === undefined || this.user.Address.length === 0 || this.user.Country === undefined || this.user.Country.length === 0 ||
      this.user.DateOfBirth === undefined || this.user.DateOfBirth.length === 0 || this.user.PhoneNumber === undefined || this.user.PhoneNumber.length === 0
    ) {
      this.passwordCheck = true;
      this.inputError = "Invalid data. Some fields are empty."
    }
    else if(new Date(this.user.DateOfBirth).toString() === 'Invalid Date'){
      this.passwordCheck = true;
      this.inputError = "Invalid date format. eg. MM/DD/YYYY"
    }
    else {
      if (this.user.Password.length < 5) {
        this.passwordCheck = true;
        this.inputError = "Invalid data. Password should have minimum 5 characters."
      }
      else if (this.user.Password1 != this.user.Password) {
        this.passwordCheck = true;
        this.inputError = "Passwords don't match."
      }
      else {
        this.passwordCheck = false;
        this.inputError = ""
        console.log(new Date(this.user.DateOfBirth))
        const data = {
          FirstName: this.user.FirstName,
          LastName: this.user.LastName,
          Username: this.user.Username,
          Password: this.user.Password,
          Address: this.user.Address,
          Country: this.user.Country,
          DateOfBirth: this.user.DateOfBirth,
          PhoneNumber: this.user.PhoneNumber
        }
        this.userService.registration(data).subscribe(() => {
          alert('Successfully registration!')
          this.router.navigate(['/'])
        })
      }

    }
  }

}
