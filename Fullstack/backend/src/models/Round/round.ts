export interface IRound{
    id:number; // The sequenceId
    team: number; //which team is playing
    player: number; // which player is playing
    minigame: MiniGame;
    minigameJSON: string,
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