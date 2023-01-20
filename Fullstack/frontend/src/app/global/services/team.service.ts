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

  public getTeam(teamId:number) {
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/team/${teamId}`);
  }

  public getNextTeam() {
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/nextTeam`);
  }

  public getVeryNextTeam() {
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/veryNextTeam`);
  }

  public assignPlayerToTeam(playerId:number,teamId:number,playerName:string) {
    return this.http
      .put(`${this.apiUrl}/BoFUN/game/assignPlayerToTeam/${playerId}`, {teamId: teamId,username:playerName});
  }

  public editTeam(teamId:number,name:string,image:string) {
    return this.http
      .put(`${this.apiUrl}/BoFUN/game/editTeam/${teamId}`, {name,image});
  }
}
