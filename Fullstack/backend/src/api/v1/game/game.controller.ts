import { Request, Response, NextFunction, Router } from "express";
import { NotFound, BadRequest } from "http-errors";

import { Game } from "../../../models/Game/game";
import { ITeam } from "../../../models/Team/team";
import { IPlayer } from "../../../models/Player/player";
import { DIContainer, SocketsService } from "../../../services";

var currentGameModule = require("../../../models/Game/currentGame.module");
var gameSettingsModule = require("../../../models/GameSettings/gameSettings.module");

export class GameController {
    public applyRoutes(): Router {
    const router = Router();

    router

        .delete("/", this.destroyGame())
        .post("/create", this.createGame())
        .post("/createPlayer", this.createPlayer())
        .get("", this.getGame())
        .get("/teamPlayers/:teamId", this.getPlayersOfATeam())
        .get("/teams", this.getTeams())
        .get("/team/:teamId", this.getTeam())
        .put("/assignPlayerToTeam/:playerId", this.assignPlayerToTeam())
        .put("/setPlayerName/:playerId", this.setNameToPlayer())
        .delete(
        "/removePlayer/:playerId/fromTeam/:teamId",
        this.removePlayerFromTeam()
        );
    return router;
    }

    public getGame() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        return res.send(currentGameModule.game);
    };
    }

    public createGame() {
        return async (
        req: Request,
        res: Response,
        next?: NextFunction
        ): Promise<Response> => {

            let newGame:Game = new Game(Number(req.body.duration), Number(req.body.totalTeams), Number(req.body.totalPlayers), Boolean(req.body.pantomime), Boolean(req.body.pictionary), Boolean(req.body.trivia));

            currentGameModule.game = newGame;
        
            return res.send(currentGameModule.game);
        };
    }

    public destroyGame(){
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        currentGameModule.game = null;

        return res.sendStatus(200);
    };
    }

    public createPlayer() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
        ): Promise<Response> => {
            // console.log("body",req.body.positionId )
            let allPlayers: Array<IPlayer> = currentGameModule.game.players;
            let player = allPlayers.find(
            ({ positionId }) => positionId === Number(req.body.positionId)
            );
            if (player === undefined) {
            let newPlayer: IPlayer = {
                id: req.body.positionId,
                username: "",
                teamId: -1,
                image: "",
                positionId: req.body.positionId,
            };
            currentGameModule.game.players.push(newPlayer);
            return res.send(newPlayer);
            }
            // return res.sendStatus(200);
            return res.send(player);
        };
    }

    public assignPlayerToTeam() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        let players: Array<IPlayer> = currentGameModule.game.players;
        let player = players.find(({ id }) => id === Number(req.params.playerId));
        if (player !== undefined) {
        player.teamId = req.body.teamId;
        player.username = req.body.username;
        }
        let teams: Array<ITeam> = currentGameModule.game.teams;
        let chosenTeam = teams.find(({ id }) => id === Number(req.body.teamId));
        if (chosenTeam !== undefined) {
        chosenTeam.members.push(Number(req.params.playerId));
        // res.sendStatus(200);

        const socketService = DIContainer.get(SocketsService);
        socketService.broadcast("TeamUpdated", Number(req.body.teamId));
        return res.send(currentGameModule.game);
        }
        return res.sendStatus(400);
    };
    }

    public setNameToPlayer() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        let players: Array<IPlayer> = currentGameModule.game.players;
        let player = players.find(({ id }) => id === Number(req.params.playerId));
        if (player !== undefined) {
        player.username = req.body.username;
        }
        return res.send(player);
    };
    }

    public removePlayerFromTeam() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        let players: Array<IPlayer> = currentGameModule.game.players;
        let player = players.find(({ id }) => id === Number(req.params.playerId));
        if (player !== undefined) {
        player.teamId = -1;
        // player.username = "";
        }
        let teams: Array<ITeam> = currentGameModule.game.teams;
        let chosenTeam = teams.find(({ id }) => id === Number(req.params.teamId));
        if (chosenTeam !== undefined) {
        for (let i = 0; i < chosenTeam.members.length; i++) {
            if (chosenTeam.members[i] === Number(req.params.playerId)) {
            chosenTeam.members.splice(i, 1); // Remove one element at pos i
            break;
            }
        }
        // res.sendStatus(200);

        const socketService = DIContainer.get(SocketsService);
        socketService.broadcast("TeamUpdated", Number(req.body.teamId));
        return res.send(currentGameModule.game);
        }
        return res.sendStatus(400);
    };
    }

    public getPlayersOfATeam() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        let allPlayers: Array<IPlayer> = currentGameModule.game.players;
        let players: Array<IPlayer> = allPlayers.filter(
        ({ teamId }) => teamId === Number(req.params.teamId)
        );
        return res.send(players);
    };
    }

    public getTeams() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        let teams: Array<ITeam> = currentGameModule.game.teams;
        return res.send(teams);
    };
    }

    public getTeam() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        let teams: Array<ITeam> = currentGameModule.game.teams;
        let team: any = teams.find(({ id }) => id === Number(req.params.teamId));
        if (team !== undefined) {
        return res.send(team);
        }
        return res.sendStatus(400);
    };
    }

    public getTeamsPlayersScores() {
        return async (
        req: Request,
        res: Response,
        next?: NextFunction
        ): Promise<Response> => {
        var results = [];
        let teamsId: Array<number> = currentGameModule.game.teams.map(
            ({ id }: { id: number }) => {
            return id;
            }
        );
        for (let i = 0; i < teamsId.length; i++) {
            var teamResults: {
            teamId: number;
            bestOverall: any[];
            miniGames: any[];
            } = {
            teamId: i,
            bestOverall: [],
            miniGames: [],
            };
            var victoryRounds = currentGameModule.game.rounds.filter(
            (
                { teamId }: { teamId: number },
                { victory }: { victory: boolean }
            ) => {
                teamId === teamsId[i] && victory;
            }
            );
            let players: Array<IPlayer> = currentGameModule.game.players.filter(
            ({ teamId }: { teamId: number }) => teamId === i
            );
            let overallCounters: Array<number> = [];
            players.forEach((player) => {
            overallCounters.push(0);
            });
            for (let j = 0; j < 3; j++) {
            //for every miniGame
            var miniGame: {
                id: number;
                bestPlayers: any;
            } = {
                id: -1,
                bestPlayers: [],
            };
            miniGame.id = j;
            var victoryMiniGame = victoryRounds.filter(
                ({ miniGame }: { miniGame: number }) => {
                miniGame === j;
                }
            );

            let counters: Array<number> = [];
            players.forEach((player) => {
                counters.push(0);
            });
            players.forEach((player) => {
                victoryMiniGame.forEach((round: any) => {
                if (round.player === player.id) {
                    overallCounters[players.indexOf(player)]++;
                    counters[players.indexOf(player)]++;
                }
                });
            });
            var maxScore = Math.max(...counters);
            var bestPlayers: any = [];
            counters.forEach((counter) => {
                if (counter === maxScore) bestPlayers.push(players[counter]);
            });
            miniGame.bestPlayers = bestPlayers;
            teamResults.miniGames.push(miniGame);
            }
            var overallMaxScore = Math.max(...overallCounters);
            overallCounters.forEach((counter) => {
            if (counter === overallMaxScore) teamResults.bestOverall.push(players[counter]);
            });
            results.push(teamResults);
        }
        return res.send(results);
        };
    }

    public getTeamsScores() {
        return async (
        req: Request,
        res: Response,
        next?: NextFunction
        ): Promise<Response> => {
        let teamsId: Array<number> = currentGameModule.game.teams.map(
            ({ id }: { id: number }) => {
            return id;
            }
        );
        var results = []
        for (let teamId = 0; teamId < teamsId.length; teamId++) {
            var teamResults: {
            teamId: number;
            miniGames: any[];
            } = {
            teamId:teamId,
            miniGames: []
            }
            for (let miniGameId = 0; miniGameId < 3; miniGameId++){
            var miniGame:{
                miniGameId: number,
                statistics: any[]
            }={
                miniGameId: miniGameId,
                statistics: []
            }
            var victoryRounds = currentGameModule.game.rounds.filter(
                (
                { teamId }: { teamId: number },
                { victory }: { victory: boolean }
                ) => {
                teamId === teamsId[teamId] && victory;
                }
            );

            var defeatRounds = currentGameModule.game.rounds.filter(
                (
                { teamId }: { teamId: number },
                { victory }: { victory: boolean }
                ) => {
                teamId === teamsId[teamId] && !victory;
                }
            );
            miniGame.statistics.push(victoryRounds.length)
            miniGame.statistics.push(defeatRounds.length)
            teamResults.miniGames.push(miniGame)
            }
            results.push(teamResults)
        }
        return res.send(results);
        };
    }

    public getGamesScores() {
        return async (
        req: Request,
        res: Response,
        next?: NextFunction
        ): Promise<Response> => {
        let teamsId: Array<number> = currentGameModule.game.teams.map(
            ({ id }: { id: number }) => {
            return id;
            }
        );
        var results:any = []
        for (let miniGameId = 0; miniGameId < 3; miniGameId++) {
            var miniGame: {
            miniGameId: number,
            statistics: Array<number>
            }={
            miniGameId: miniGameId,
            statistics: []
            }
            for (let teamId = 0; teamId < teamsId.length; teamId++) {
            var victoryRounds = currentGameModule.game.rounds.filter(
                (
                { teamId }: { teamId: number },
                { victory }: { victory: boolean }
                ) => {
                teamId === teamsId[teamId] && victory;
                }
            );
            miniGame.statistics.push(victoryRounds.length)
            }
            results.push(miniGame)
        }
        return res.send(results);
        };
    }
}
