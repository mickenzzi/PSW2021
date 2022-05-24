import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Term } from '../model/term';
import { TermRequest } from '../model/term-request';

@Injectable({
  providedIn: 'root'
})
export class TermService {

  constructor(private http: HttpClient) { }
  private termsUrl = "http://localhost:8000/terms"

  public schedule(termRequest: TermRequest) : Observable<any> {
    return this.http.post(`${this.termsUrl}/schedule`, termRequest)
  }

  public reserve(termRequest: Term) : Observable<any> {
    console.log(termRequest);
    return this.http.post(`${this.termsUrl}/reserve`, termRequest)
  }
}
