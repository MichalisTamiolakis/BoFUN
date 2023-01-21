import { Request, Response, NextFunction, Router } from "express";

import { Game } from "../../../models/Game/game";
import { ITeam } from "../../../models/Team/team";
import { IPlayer } from "../../../models/Player/player";
import { DIContainer, SocketsService } from "../../../services";
import { IRound } from "../../../models/Round/round";

var currentGameModule = require("../../../models/Game/currentGame.module");
var gameSettingsModule = require("../../../models/GameSettings/gameSettings.module");

const socketService = DIContainer.get(SocketsService);

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
      .get("/playersScores", this.getTeamsPlayersScores())
      .get("/teamsScores", this.getTeamsScores())
      .get("/gamesScores", this.getGamesScores())
      .get("/winnerTeam", this.getWinnerTeam())
      .get("/nextTeam", this.getNextTeam())
      .get("/veryNextTeam", this.getVeryNextTeam())
      .put("/assignPlayerToTeam/:playerId", this.assignPlayerToTeam())
      .put("/setPlayerName/:playerId", this.setNameToPlayer())
      .put("/setPlayerAvater/:playerId", this.setAvatarToPlayer())
      .put("/editTeam/:teamId", this.editTeam())
      .delete(
        "/removePlayer/:playerId/fromTeam/:teamId",
        this.removePlayerFromTeam()
      )
      .post("/create/dummyGame", this.createDummyGame())
      .get("/sendEvent", this.sendEvent())
      .post("/start", this.gameStart())
      .post("/end", this.gameEnd());
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
      let newGame: Game = new Game(
        Number(req.body.duration),
        Number(req.body.totalTeams),
        Number(req.body.totalPlayers),
        Boolean(req.body.pantomime),
        Boolean(req.body.pictionary),
        Boolean(req.body.trivia)
      );

      currentGameModule.game = newGame;

      return res.send(currentGameModule.game);
    };
  }

  public createDummyGame() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      currentGameModule.game.setupDummyGame();
      socketService.broadcast("GameStarted", JSON.stringify({}));
      return res.send(currentGameModule.game);
    };
  }

  public destroyGame() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      currentGameModule.game = null;

      return res.sendStatus(200);
    };
  }

  public gameStart(){
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      currentGameModule.game.hasGameStarted = true;
      console.log("GAME STARTED");
      socketService.broadcast("GameStarted", JSON.stringify({}));
      return res.sendStatus(200);
    };
  }

  public gameEnd(){
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      currentGameModule.game.hasGameEnded = true;
      socketService.broadcast("GameOver", JSON.stringify({}));
      return res.sendStatus(200);
    };
  }

  // Called from smartphone when a player scans the qr code and goes to the webpage
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
          username: "New Player",
          teamId: -1,
          image: "",
          positionId: req.body.positionId,
        };
        currentGameModule.game.players.push(newPlayer);

        console.log("Player created, seating at " + req.body.positionId);
        socketService.broadcast("SeatOccupied", Number(req.body.positionId));
        socketService.broadcast("PlayerUpdated", JSON.stringify(newPlayer));
        return res.send(newPlayer);
      }

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
        socketService.broadcast("PlayerUpdated", JSON.stringify(player));
      }
      let teams: Array<ITeam> = currentGameModule.game.teams;
      let chosenTeam = teams.find(({ id }) => id === Number(req.body.teamId));
      if (chosenTeam !== undefined) {
        chosenTeam.members.push(Number(req.params.playerId));
        // res.sendStatus(200);

        // socketService.broadcast("TeamUpdated", [teams]);
        socketService.broadcast("TeamUpdated", JSON.stringify(chosenTeam));
        return res.send(currentGameModule.game);
      }
      return res.sendStatus(400);
    };
  }

  public setAvatarToPlayer() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      let players: Array<IPlayer> = currentGameModule.game.players;
      let player = players.find(({ id }) => id === Number(req.params.playerId));
      if (player !== undefined) {
        player.image = req.body.image;
        socketService.broadcast("PlayerUpdated", JSON.stringify(player));
        return res.send(player);
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
        socketService.broadcast("PlayerUpdated", JSON.stringify(player));
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

        // const socketService = DIContainer.get(SocketsService);
        socketService.broadcast("TeamUpdated", JSON.stringify(chosenTeam));
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

  getCurrentRound(): IRound | undefined {
    let rounds = currentGameModule.game.rounds;
    if (rounds.length > 0) {
      return rounds[rounds.length - 1];
    }
    return undefined;
  }

  public getNextTeam() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      
      let currentGame:Game = currentGameModule.game;
      if(currentGame == undefined){
        return res.sendStatus(400);
      }

      let nextTeam:ITeam | undefined = currentGame.getNextTeam();

      if(!nextTeam)
        return res.sendStatus(400);
        
      return res.send(nextTeam);
    };
  }

  public getVeryNextTeam() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      
      let currentGame:Game = currentGameModule.game;
      if(currentGame == undefined){
        return res.sendStatus(400);
      }

      let nextTeam:ITeam | undefined = currentGame.getVeryNextTeam();

      if(!nextTeam)
        return res.sendStatus(400);
        
      return res.send(nextTeam);
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

  public editTeam() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      let teams: Array<ITeam> = currentGameModule.game.teams;
      let team: any = teams.find(({ id }) => id === Number(req.params.teamId));
      if (team !== undefined) {
        team.name = req.body.name;
        team.image = req.body.image;
        socketService.broadcast("TeamUpdated", JSON.stringify(team));
        return res.send(team);
      }
      return res.sendStatus(400);
    };
  }

  public setWinnerTeam() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      currentGameModule.game.winningTeam = Number(req.params.teamId);
      let team: any = currentGameModule.game.teams.find(
        ({ id }: { id: number }) => id === Number(req.params.teamId)
      );
      if (team !== undefined) {
        socketService.broadcast("GameOver", JSON.stringify(team));
        return res.send(team);
      }
      return res.sendStatus(400);
    };
  }

  public getWinnerTeam() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      let winningTeamId: number = currentGameModule.game.winningTeam;
      let team: any = currentGameModule.game.teams.find(
        ({ id }: { id: number }) => id === winningTeamId
      );
      if (team !== undefined) {
        
        return res.send(team);
      }
      return res.send({id:-1});
    };
  }

  public getTeamsPlayersScores() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      var results: any = [];
      let teamsId: Array<number> = currentGameModule.game.teams.map(
        ({ id }: { id: number }) => {
          return id;
        }
      );
      let selectedMiniGames: any = [];
      //gather selected miniGames
      if (currentGameModule.game.pantomime) selectedMiniGames.push(0);
      if (currentGameModule.game.trivia) selectedMiniGames.push(1);
      if (currentGameModule.game.pictionary) selectedMiniGames.push(2);

      for (let i = 0; i < teamsId.length; i++) {
        // for each team
        var teamResults: {
          teamName: number;
          bestOverall: any[];
          miniGames: any[];
        } = {
          teamName: currentGameModule.game.teams.find(
            ({ id }: { id: number }) => id === i
          ).name,
          bestOverall: [],
          miniGames: [],
        };
        var bestOverallCounters: any = []; // array of total wins in every game for each team's player

        //get players of current team
        let players: Array<IPlayer> = currentGameModule.game.players.filter(
          ({ teamId }: { teamId: number }) => teamId === i
        );

        //initialize bestOverallCounters with zeros
        players.forEach((element) => {
          bestOverallCounters.push(0);
        });

        //for every miniGame
        for (
          let miniGame = 0;
          miniGame < selectedMiniGames.length;
          miniGame++
        ) {
          let miniGameEntry: {
            miniGameId: any;
            bestPlayers: any[];
          } = {
            miniGameId: selectedMiniGames[miniGame],
            bestPlayers: [],
          };

          // victory counters for every player of a team in one minigame
          let victoryCounters: any = [];
          players.forEach((element) => {
            victoryCounters.push(0);
          });

          players.forEach((player) => {
            let victoryRounds = currentGameModule.game.rounds.filter(
              (round: any) => {
                return (
                  round.player === player.id &&
                  round.team === teamsId[i] &&
                  round.victory &&
                  round.minigame === selectedMiniGames[miniGame]
                );
              }
            );
            victoryCounters[players.indexOf(player)] = victoryRounds.length;
            bestOverallCounters[players.indexOf(player)] +=
              victoryRounds.length;
          });

          let maxScore = Math.max(...victoryCounters);
          for (
            let scoreIndex = 0;
            scoreIndex < victoryCounters.length;
            scoreIndex++
          ) {
            if (victoryCounters[scoreIndex] === maxScore) {
              miniGameEntry.bestPlayers.push(players[scoreIndex]);
            }
          }
          teamResults.miniGames.push(miniGameEntry);
        }
        let maxScore = Math.max(...bestOverallCounters);
        for (
          let scoreIndex = 0;
          scoreIndex < bestOverallCounters.length;
          scoreIndex++
        ) {
          if (bestOverallCounters[scoreIndex] === maxScore) {
            teamResults.bestOverall.push(players[scoreIndex]);
          }
        }
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
      let selectedMiniGames: any = [];
      //gather selected miniGames
      if (currentGameModule.game.pantomime) selectedMiniGames.push(0);
      if (currentGameModule.game.trivia) selectedMiniGames.push(1);
      if (currentGameModule.game.pictionary) selectedMiniGames.push(2);
      console.log("teamsId", teamsId);
      var results = [];
      for (let teamId = 0; teamId < teamsId.length; teamId++) {
        var teamResults: {
          teamId: number;
          miniGames: any[];
        } = {
          teamId: teamId,
          miniGames: [],
        };
        for (
          let miniGameId = 0;
          miniGameId < selectedMiniGames.length;
          miniGameId++
        ) {
          var miniGame: {
            miniGameId: number;
            statistics: any[];
          } = {
            miniGameId: selectedMiniGames[miniGameId],
            statistics: [],
          };
          var victoryRounds = currentGameModule.game.rounds.filter(
            (round: any) => {
              return (
                round.team === teamsId[teamId] &&
                round.victory &&
                round.minigame === selectedMiniGames[miniGameId]
              );
            }
          );

          var defeatRounds = currentGameModule.game.rounds.filter(
            (round: any) => {
              return (
                round.team === teamsId[teamId] &&
                !round.victory &&
                round.minigame === selectedMiniGames[miniGameId]
              );
            }
          );
          miniGame.statistics.push(victoryRounds.length);
          miniGame.statistics.push(defeatRounds.length);
          teamResults.miniGames.push(miniGame);
        }
        results.push(teamResults);
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
      console.log("teamsId", teamsId);
      let selectedMiniGames: any = [];
      //gather selected miniGames
      if (currentGameModule.game.pantomime) selectedMiniGames.push(0);
      if (currentGameModule.game.trivia) selectedMiniGames.push(1);
      if (currentGameModule.game.pictionary) selectedMiniGames.push(2);
      var results: any = [];
      for (let miniGameId = 0; miniGameId < selectedMiniGames.length; miniGameId++) {
        var miniGame: {
          miniGameId: number;
          statistics: Array<number>;
        } = {
          miniGameId: selectedMiniGames[miniGameId],
          statistics: [],
        };
        for (let teamId = 0; teamId < teamsId.length; teamId++) {
          var victoryRounds = currentGameModule.game.rounds.filter(
            (round: any) => {
              return (
                round.team === teamsId[teamId] &&
                round.victory &&
                round.minigame === selectedMiniGames[miniGameId]
              );
            }
          );
          console.log("victoryRounds=", victoryRounds, "-----------------");
          miniGame.statistics.push(victoryRounds.length);
        }
        results.push(miniGame);
      }
      return res.send(results);
    };
  }

  public sendEvent() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      let team: ITeam = currentGameModule.game.teams[0];
      team.name = "IT WORKS";
      socketService.broadcast("TeamUpdated", JSON.stringify(team));
      return res.sendStatus(200);
    };
  }
}
