import { ITeam } from './../../../models/Team/team';
import { IPlayer } from "./../../../models/Player/player";
import { Request, Response, NextFunction, Router } from "express";
import { NotFound, BadRequest } from "http-errors";

import { IGame } from "../../../models/Game/game";

var currentGameModule = require("../../../models/Game/currentGame.module");

export class Game {
  public applyRoutes(): Router {
    const router = Router();

    router.post("/create", this.createGame());
    router.post("/createPlayer", this.createPlayer());

    return router;
  }

  public createGame() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      return res.sendStatus(200);
    };
  }

  public createPlayer() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
      let player: IPlayer = {
        id: new Date().getTime(),
        username: "",
        teamId: -1,
        image: "",
        positionId: req.body.positionId,
      };
      currentGameModule.game.players.push(player);
      res.sendStatus(200);
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
      let player = players.find(({ id }) => id === Number(req.params.id));
      if (player !== undefined) {
        player.teamId = req.body.teamId;
      }
      let teams: Array<ITeam> = currentGameModule.game.teams;
      let chosenTeam = teams.find(({ id }) => id === Number(req.body.teamId));
      if(chosenTeam !== undefined){
        chosenTeam.members.push(Number(req.params.id));
        res.sendStatus(200);
        return res.send(currentGameModule.game);
      }
      return res.sendStatus(400);
    };
  }
}
