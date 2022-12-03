import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  apiUrl: string = '';
  constructor(private http: HttpClient) {
    this.apiUrl = environment.host;
  }
  public getTeamPlayers(teamId:number) {
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/teamPlayers/${teamId}`);
  }

  public getTeams() {
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/teams`);
  }

  public assignPlayerToTeam(playerId:number,teamId:number,playerName:string) {
    return this.http
      .put(`${this.apiUrl}/BoFUN/game/assignPlayerToTeam/${playerId}`, {teamId: teamId,username:playerName});
  }
}
