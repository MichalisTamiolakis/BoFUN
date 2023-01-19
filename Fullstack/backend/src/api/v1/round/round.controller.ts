import { Request, Response, NextFunction, Router } from "express";

import { Game } from "../../../models/Game/game";
import { ITeam } from "../../../models/Team/team";
import { IRound, MiniGame } from "../../../models/Round/round";
import { DIContainer, SocketsService } from "../../../services";

var currentGameModule = require("../../../models/Game/currentGame.module");
const socketService = DIContainer.get(SocketsService);
var timer: string | number | NodeJS.Timer | undefined;

export class Round {
    public applyRoutes(): Router {
    const router = Router();

    router
        .get("/all", this.getAllRounds()) // Returns all the rounds of the current game
        .get("/get/:roundId", this.getRound())
        .get("/current", this.getCurrentRound())
        .post("/new/:miniGame", this.newRound())
        .put("/setResult", this.setResult())
        .post("/current/start", this.startCurrentRound());
        
    return router;
    }

    public getAllRounds() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        let currentGame:Game = currentGameModule.game;
        if (currentGame && currentGame.rounds) {
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
        let currentGame:Game = currentGameModule.game;
        let game: Game = currentGame;

        let result = undefined;
        if (game)
        result = game.rounds.find((e) => e.id == Number(req.params.roundId));

        if (result != undefined) return res.send(result);
        else return res.sendStatus(404);
    };
    }

    public getCurrentRound() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
        let currentGame:Game = currentGameModule.game;
        let currentRound: IRound | undefined = currentGame.getCurrentRound();
        if (currentRound) {
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
        let currentGame:Game = currentGameModule.game;
        let newRound: IRound | undefined = await currentGame.newRound(
            Number(req.params.miniGame)
        );

        if (newRound) {
            socketService.broadcast("NewRound", JSON.stringify(newRound));

            return res.send(newRound);
        }

        
        return res.sendStatus(500);
    };
    }

    public editRound() {
        return async (
            req: Request,
            res: Response,
            next?: NextFunction
        ): Promise<Response> => {
            let currentGame:Game = currentGameModule.game;
            let currentRound: IRound | undefined = currentGame.getCurrentRound();
            if (currentRound) {
            currentRound.victory = req.body.victory;
            currentRound.started = req.body.started;
            currentRound.ended = req.body.ended;
            socketService.broadcast("RoundUpdated", JSON.stringify(currentRound));

            return res.send(currentRound);
            }
            return res.sendStatus(404);
        };
    }

    public setResult() {
      return async (
          req: Request,
          res: Response,
          next?: NextFunction
      ): Promise<Response> => {
            let currentGame:Game = currentGameModule.game;
            let currentRound: IRound | undefined = currentGame.getCurrentRound();
            if (currentRound) {
                currentRound.victory = req.body.victory;
                currentRound.ended = true;
                clearInterval(timer);
                socketService.broadcast("RoundEnded", JSON.stringify(currentRound));
                socketService.broadcast("RoundUpdated", JSON.stringify(currentRound));
                
                return res.send(currentRound);
            }
            return res.sendStatus(404);
      };
  }

    public startCurrentRound(){
        return async (
            req: Request,
            res: Response,
            next?: NextFunction
        ): Promise<Response> => {
            let currentGame:Game = currentGameModule.game;
            let currentRound: IRound | undefined = currentGame.getCurrentRound();
            if (currentRound) {
                
                socketService.broadcast("CurrentRoundStarted", JSON.stringify(currentRound));
                currentRound.started = true;
                this.StartTimer(currentRound);
                socketService.broadcast("RoundUpdated", JSON.stringify(currentRound));

                
                return res.send(currentRound);
            }
            return res.sendStatus(404);
        };
    }

    private StartTimer(round:IRound){
        round.started = true;
        timer = setInterval(()=>{
            if(round.ended || round.remainingTime<=0)
            {
                round.victory = false;
                round.ended = true;
                socketService.broadcast("RoundEnded", JSON.stringify(round));
                clearInterval(timer);

                socketService.broadcast("RoundUpdated", JSON.stringify(round));

                return;
            }
            
            round.remainingTime -= 1;
            socketService.broadcast("RoundTimerUpdated", JSON.stringify(round));
            socketService.broadcast("RoundUpdated", JSON.stringify(round));

        }, 1000);
    }
}
