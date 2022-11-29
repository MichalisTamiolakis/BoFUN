import { IPlayer } from "../Player/player";
import { ITeam } from "../Team/team";
import { IRound } from "../Round/round";
export interface IGame{
    duration: number;
    totalPlayers: number;
    players: Array<IPlayer>;
    teams: Array<ITeam>;
    pantomime: boolean;
    pictionary: boolean;
    trivia: boolean;
    sequence: Array<number>;
    winningTeam: number;
    rounds: Array<IRound>;
}