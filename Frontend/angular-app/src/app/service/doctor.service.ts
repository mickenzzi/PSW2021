import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Recipe } from '../model/recipe';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  constructor(private http: HttpClient) { }
  private doctorUrl = "http://localhost:8000/doctors"

  public getAllSpecialist() : Observable<any> {
    return this.http.get(`${this.doctorUrl}/specialist`)
  }

  public getAllNonSpecialist() : Observable<any> {
    return this.http.get(`${this.doctorUrl}/nonspecialist`)
  }

  public getAllDoctors() : Observable<any> {
    return this.http.get(`${this.doctorUrl}`)
  }

  public getDoctor(id: string) : Observable<any> {
    return this.http.get(`${this.doctorUrl}/${id}`)
  }

  public createRecipe(recipe: Recipe) : Observable<any> {
    return this.http.post(`${this.doctorUrl}/recipe`, recipe)
  }

}
