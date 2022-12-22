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

  public getTeamsScores(){
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/teamsScores`);
  }

  public getGamesScores(){
    return this.http
      .get(`${this.apiUrl}/BoFUN/game/gamesScores`);
  }
}
