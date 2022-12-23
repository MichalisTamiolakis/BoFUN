import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RoundService {
  apiUrl: string = '';
  
  
  
  constructor(private http: HttpClient) { this.apiUrl = environment.host;}

  public getCurrentRound(){
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/round/current`);
  }

  public editCurrentRound(victory:boolean,started:boolean,ended:boolean){
    return this.http
      .put(`${this.apiUrl}/BoFUN/game/round/editCurrent`,{victory,started,ended});
  }
}
