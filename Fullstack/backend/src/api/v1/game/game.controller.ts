import { MiniGame } from './../../../../../frontend/src/app/global/models/round/round';
import { Request, Response, NextFunction, Router } from "express";
import { NotFound, BadRequest } from "http-errors";

import { IGame } from "../../../models/Game/game";
import { ITeam } from "../../../models/Team/team";
import { IPlayer } from "../../../models/Player/player";
import { DIContainer, SocketsService } from "../../../services";

var currentGameModule = require("../../../models/Game/currentGame.module");
var gameSettingsModule = require("../../../models/GameSettings/gameSettings.module");

export class Game {
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
      let teamsIds: Array<number> = [];
      let colors: Array<string> = gameSettingsModule.availableTeamColors.slice();

      let teams: Array<ITeam> = [];
      for (let i = 0; i < Number(req.body.totalTeams); i++) {
        let choice = Math.floor(Math.random() * colors.length);
        let chosenColor = colors[choice];
        colors.splice(choice, 1);
        teamsIds.push(i);
        
        teams.push({
          id: i,
          name: "Team " + (i + 1),
          image: "",
          members: [],
          color: chosenColor,
          nextPlayer:-1
        });
      }

      let currentIndex = teamsIds.length,
        randomIndex;

      // While there remain elements to shuffle.
      while (currentIndex != 0) {
        // Pick a remaining element.
        randomIndex = Math.floor(Math.random() * currentIndex);
        currentIndex--;

        // And swap it with the current element.
        [teamsIds[currentIndex], teamsIds[randomIndex]] = [
          teamsIds[randomIndex],
          teamsIds[currentIndex],
        ];
      }

      
      currentGameModule.game = {
        duration: req.body.duration,
        totalPlayers: req.body.totalPlayers,
        players: [],
        teams: teams,
        pantomime: Boolean(req.body.pantomime),
        pictionary: Boolean(req.body.pictionary),
        trivia: Boolean(req.body.trivia),
        sequence: teamsIds,
        winningTeam: -1,
        rounds: [],
      };
      
      console.log(req.body);
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
      let teamsId: Array<number> = currentGameModule.game.teams.map(({id} : {id:number})=>{ return id;});
      for (let i = 0; i < teamsId.length; i++) {
        var teamResults: {
          teamId:number,
          bestOverAll:any[],
          miniGames: any[]
        } = {
          teamId: i,
          bestOverAll : [],
          miniGames: []
        };
        var victoryRounds = currentGameModule.game.rounds.filter(({teamId} : {teamId:number},{victory} : {victory:boolean}) => {teamId === teamsId[i] && victory})
        for (let j = 0; j < 2; j++) { //for every miniGame
          var miniGame : {
            id:number,
            bestPlayers: any
          } = {
            id: -1,
            bestPlayers: []
          }
          miniGame.id = j;
          var victoryMiniGame = victoryRounds.filter(({miniGame}: {miniGame:number})=>{miniGame === j})
          let players: Array<IPlayer> = currentGameModule.game.players.filter(
            ({ teamId } : {teamId:number}) => teamId === i);
          let counters:Array<number> = [];
            players.forEach(player => {
              counters.push(0);
          });
          players.forEach(player => {
            victoryMiniGame.forEach((round:any) => {
                if(round.player === player.id) counters[players.indexOf(player)] ++;
            });
        });
        var maxScore = Math.max(...counters);
        var bestPlayers:any = [];
        counters.forEach(counter => {
          if(counter===maxScore) bestPlayers.push(players[counter])
        });
        miniGame.bestPlayers = bestPlayers
        teamResults.miniGames.push(miniGame)
        }
        results.push(teamResults)
      }
      // let team: any = teams.find(({ id }) => id === Number(req.params.teamId));
      // if (team !== undefined) {
      //   return res.send(team);
      // }
      return res.send(results);
    };
  }

}
