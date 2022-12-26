import { Request, Response, NextFunction, Router } from "express";

import { Game } from "../../../models/Game/game";
import { ITeam } from "../../../models/Team/team";
import { IRound, MiniGame } from "../../../models/Round/round";
import { DIContainer, SocketsService } from "../../../services";

var currentGame: Game = require("../../../models/Game/currentGame.module").game;
const socketService = DIContainer.get(SocketsService);


export class Round {
    public applyRoutes(): Router {
    const router = Router();

    router
        .get("/all", this.getAllRounds()) // Returns all the rounds of the current game
        .get("/get/:roundId", this.getRound())
        .get("/current", this.getCurrentRound())
        .post("/new/:miniGame", this.newRound())
        .put("/editCurrent", this.editRound())
        .post("/current/start", this.startCurrentRound());
        
    return router;
    }

    public getAllRounds() {
    return async (
        req: Request,
        res: Response,
        next?: NextFunction
    ): Promise<Response> => {
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
        let newRound: IRound | undefined = await currentGame.newRound(
            Number(req.params.miniGame)
        );

        if (newRound) {
            return res.send(newRound);
        }

        return res.sendStatus(404);
    };
    }

    public editRound() {
        return async (
            req: Request,
            res: Response,
            next?: NextFunction
        ): Promise<Response> => {
            let currentRound: IRound | undefined = currentGame.getCurrentRound();
            if (currentRound) {
            currentRound.victory = req.body.victory;
            currentRound.started = req.body.started;
            currentRound.ended = req.body.ended;
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
            let currentRound: IRound | undefined = currentGame.getCurrentRound();
            if (currentRound) {
                
                socketService.broadcast("CurrentRoundStarted", currentRound);

                // TODO - Start Timer

                // socketService.broadcast("TeamUpdated", JSON.stringify(chosenTeam));
                this.StartTimer(currentRound);
                
                return res.send(currentRound);
            }
            return res.sendStatus(404);
        };
    }

    private StartTimer(round:IRound){
        round.started = true;
        let timer = setInterval(()=>{
            if(round.ended || round.remainingTime<=0)
            {
                round.ended = true;
                clearInterval(timer);
                return;
            }
            
            round.remainingTime -= 1;
            socketService.broadcast("CurrentRoundTimerUpdated", round.remainingTime);

        }, 1000);

        // else return Promise.delay(1000).then(() => a());
    }
}
