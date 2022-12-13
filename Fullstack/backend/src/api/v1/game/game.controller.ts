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

      .post("/create", this.createGame())
      .post("/createPlayer", this.createPlayer())
      .get("", this.getGame())
      .get("/teamPlayers/:teamId", this.getPlayersOfATeam())
      .get("/teams", this.getTeams())
      .get("/team/:teamId", this.getTeam())
      .put("/assignPlayerToTeam/:playerId", this.assignPlayerToTeam())
      .put("/setPlayerName/:playerId", this.setNameToPlayer())
      .delete("/removePlayer/:playerId/fromTeam/:teamId", this.removePlayerFromTeam());
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
      let colors: Array<string> = gameSettingsModule.availableTeamColors.slice();

      let teams: Array<ITeam> = [];
      for (let i = 0; i < Number(req.body.totalTeams); i++) {
        let choice = Math.floor(Math.random() * colors.length);
        let chosenColor = colors[choice];
        colors.splice(choice, 1);
        
        teams.push({
          id: i,
          name: "Team " + (i + 1),
          image: "",
          members: [],
          color: chosenColor,
        });
      }
      
      currentGameModule.game = {
        duration: req.body.duration,
        totalPlayers: req.body.totalPlayers,
        players: [],
        teams: teams,
        pantomime: Boolean(req.body.pantomime),
        pictionary: Boolean(req.body.pictionary),
        trivia: Boolean(req.body.trivia),
        sequence: [],
        winningTeam: -1,
        rounds: [],
      };
      
      console.log(req.body);
      return res.send(currentGameModule.game);
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
      }
      // return res.sendStatus(200);
      return res.send(currentGameModule.game);
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

  public removePlayerFromTeam(){
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      let players: Array<IPlayer> = currentGameModule.game.players;
      let player = players.find(({ id }) => id === Number(req.params.playerId));
      if (player !== undefined) {
        player.teamId = -1;
        player.username = '';
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
}
