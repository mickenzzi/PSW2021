import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest } from '../model/login-request'

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }
  private userUrl = "http://localhost:8000/users"


  public login(request: LoginRequest) : Observable<any> {
    return this.http.post(`${this.userUrl}/login`, request)
  }
}
