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

  public reject(id: string) : Observable<any> {
    return this.http.delete(`${this.termsUrl}/reject/${id}`)
  }

  public delete(id: string) : Observable<any> {
    return this.http.delete(`${this.termsUrl}/${id}`)
  }

  public reserve(termRequest: Term) : Observable<any> {
    return this.http.post(`${this.termsUrl}/reserve`, termRequest)
  }

  public getAllPatientTerms(id: string) : Observable<any>{
    return this.http.get(`${this.termsUrl}/patient/${id}`)
  }

  public getAllDoctorTerms(id: string) : Observable<any>{
    return this.http.get(`${this.termsUrl}/doctor/${id}`)
  }

  public getAllPatientCompletedTerms(id: string) : Observable<any>{
    return this.http.get(`${this.termsUrl}/completed/${id}`)
  }
  public getAllPatientFutureTerms(id: string) : Observable<any>{
    return this.http.get(`${this.termsUrl}/future/${id}`)
  }
}
