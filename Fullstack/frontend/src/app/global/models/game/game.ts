import { Player } from "../player/player";
import { Team } from "../team/team";
import { Round } from "../round/round";
export interface Game{
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