import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  apiUrl: string = '';

  constructor(private http: HttpClient) {
    this.apiUrl = environment.host;
  }

  public getGame() {
    return this.http
      .get(`${this.apiUrl}/BoFUN/game`);
  }

  public getRounds(){
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/round/all`);
  }

  public getWinnerTeam(){
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/winnerTeam`);
  }

  public getTeamsScores(){
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/teamsScores`);
  }

  public getPlayersScores(){
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/playersScores`);
  }

  public getGamesScores(){
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/gamesScores`);
  }
}
