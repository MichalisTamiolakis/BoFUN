import { Player } from "../Player/player";
import { Team } from "../Team/team";
import { Round } from "../Round/round";
export interface IGame{
    duration: number;
    totalPlayers: number;
    players: Array<Player>;
    teams: Array<Team>;
    pantomime: boolean;
    pictionary: boolean;
    trivia: boolean;
    sequence: Array<number>;
    winningTeam: number;
    rounds: Array<Round>;
}