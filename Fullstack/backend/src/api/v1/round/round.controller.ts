import { Request, Response, NextFunction, Router } from "express";

import { Game } from "../../../models/Game/game";
import { ITeam } from "../../../models/Team/team";
import { IRound, MiniGame } from "../../../models/Round/round";

var currentGame:Game = require("../../../models/Game/currentGame.module").game;

export class Round {
  public applyRoutes(): Router {
    const router = Router();

    router
      .get("/all", this.getAllRounds()) // Returns all the rounds of the current game
      .get("/get/:roundId", this.getRound())
      .get("/current", this.getCurrentRound())
      .post("/new/:miniGame", this.newRound());
    return router;
  }

  public getAllRounds() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {

        if(currentGame && currentGame.rounds){
            return res.send(currentGame.rounds);
        }

        return res.sendStatus(404);
    };
  }

  public getRound() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
        let game:Game = currentGame;

        let result = undefined;
        if(game)
            result = game.rounds.find(e => e.id == Number(req.params.roundId));

        if(result!=undefined)
            return res.send(result);
        else
            return res.sendStatus(404);
    };
  }

  public getCurrentRound() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {

        let currentRound:IRound|undefined = currentGame.getCurrentRound();
        if(currentRound){
            return res.send(currentRound);
        }
        return res.sendStatus(404);
    };
  }

  public newRound() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {

        let newRound:IRound | undefined = currentGame.newRound(Number(req.params.miniGame));

        if(newRound){
            return res.send(newRound);
        }
        
        return res.sendStatus(404);
    };
  }

}
