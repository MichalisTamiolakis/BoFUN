export interface Round{
    team: number; //which team is playing
    player: number; // which player is playing
    miniGame: MiniGame;
    victory: boolean;
    remainingTime: number;
    started: boolean;
    ended:boolean;
}

export enum MiniGame {
    Pantomime = 0,
    Trivia = 1,
    Pictionary = 2
  }