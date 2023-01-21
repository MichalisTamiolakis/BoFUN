
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Player } from '../models/player/player';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  apiUrl: string = '';
  constructor(private http: HttpClient) {
    this.apiUrl = environment.host;
  }
  public create(positionId:number) {
    return this.http
      .post(`${this.apiUrl}/BoFUN/game/createPlayer`, {positionId: positionId});
  }

  public setName(playerId:number,username:string) {
    return this.http
      .put(`${this.apiUrl}/BoFUN/game/setPlayerName/${playerId}`, {username: username});
  }

  public setAvatar(playerId:number,image:string) {
    return this.http
      .put(`${this.apiUrl}/BoFUN/game/setPlayerAvater/${playerId}`, {image: image});
  }

  public removePlayerFromTeam(playerId:number,teamId:number){
    return this.http
      .delete(`${this.apiUrl}/BoFUN/game/removePlayer/${playerId}/fromTeam/${teamId}`);
  }
}
