import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Feedback } from '../model/feedback';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {

  constructor(private http: HttpClient) { }
  private feedbackUrl = "http://localhost:8000/feedbacks"

  public createFeedback(feedback: Feedback) : Observable<any> {
    return this.http.post(`${this.feedbackUrl}`, feedback)
  }

  public getAllFeedbacks() : Observable<any> {
    return this.http.get(`${this.feedbackUrl}`)
  }

  public updateFeedback(feedback: Feedback): Observable<any> {
    return this.http.put(`${this.feedbackUrl}`, feedback)
  }

}