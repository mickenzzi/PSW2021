import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Comment } from '../model/comment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http: HttpClient) { }
  private commentUrl = "http://localhost:8000/comments"

  public createComment(comment: Comment) : Observable<any> {
    return this.http.post(`${this.commentUrl}`, comment)
  }

}
