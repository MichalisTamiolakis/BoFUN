export interface IRound{
    team: number; //which team is playing
    player: number; // which player is playing
    miniGame: MiniGame;
    victory: boolean;
    remainingTime: number;
    started: boolean;
}

enum MiniGame {
    Pantomime = 0,
    Trivia = 1,
    Pictionary = 2
  }