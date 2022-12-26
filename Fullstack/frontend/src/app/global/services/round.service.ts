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

  public setResult(victory:boolean){
    return this.http
      .put(`${this.apiUrl}/BoFUN/game/round/setResult`,{victory});
  }
  

  public startCurrentRound(){
    return this.http
      .post(`${this.apiUrl}/BoFUN/game/round/current/start`,{});
  }
}
