import { Request, Response, NextFunction, Router } from "express";
import { NotFound, BadRequest } from "http-errors";

import { IGame } from "../../../models/Game/game";
import { ITeam } from "../../../models/Team/team";
import { IPlayer } from "../../../models/Player/player";
import { IRound } from "../../../models/Round/round";
import { DIContainer, SocketsService } from "../../../services";
import { indexOf } from "lodash";

var currentGameModule = require("../../../models/Game/currentGame.module");
var gameSettingsModule = require("../../../models/GameSettings/gameSettings.module");

export class Round {
  public applyRoutes(): Router {
    const router = Router();

    router
      .get("/all", this.getAllRounds()) // Returns all the rounds of the current game
      .get("/get/:roundId", this.getRound())
      .get("/current", this.getCurrentRound())
      .post("/new", this.newRound());
    return router;
  }

  public getAllRounds() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {

        if(currentGameModule.game && currentGameModule.game.rounds){
            return res.send(currentGameModule.game.rounds);
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
        let game:IGame = currentGameModule.game;

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
        let game:IGame = currentGameModule.game;

        let result = undefined;
        if(game)
            result = game.rounds[game.rounds.length-1];

        if(result!=undefined)
            return res.send(result);
        else
            return res.sendStatus(404);
    };
  }

  public newRound() {
    return async (
      req: Request,
      res: Response,
      next?: NextFunction
    ): Promise<Response> => {
        
        
        
        if(currentGameModule.game)
        {
            let currentGame:IGame =currentGameModule.game;


            let lastRoundTeam:ITeam | undefined = currentGame.teams.find(e => e.id == currentGame.sequence[currentGameModule.game.sequence.length-1]);
            let lastRoundPlayer:number | undefined = lastRoundTeam?.members[0];

            let lastRound:IRound = {
                miniGame : 0,
                id : -1,
                team: lastRoundTeam? lastRoundTeam.id : -1,
                player: lastRoundPlayer? lastRoundPlayer : -1,
                victory: false,
                remainingTime: 0,
                started:true,
                ended:true
            }

            // If it is not the first round
            if(currentGameModule.game.rounds.length>0){
                lastRound = currentGameModule.game.rounds[currentGameModule.game.rounds.length-1];
            }

            let nextPlayingTeamSequenceIndex = currentGameModule.indexOf(lastRound.team);
            nextPlayingTeamSequenceIndex = nextPlayingTeamSequenceIndex + 1 > currentGameModule.sequence.length-1 ? 0:nextPlayingTeamSequenceIndex + 1;
            let nextPlayingTeamId = currentGameModule.game.sequence[nextPlayingTeamSequenceIndex];

            // Find the last round of this team
            let nextPlayingTeam:ITeam|undefined = currentGame.teams.find(e=>e.id==nextPlayingTeamId);

            // Find the next playing 
            let newRound:IRound = {
                miniGame : req.body.miniGame,
                id : currentGameModule.game.rounds.length,
                team: nextPlayingTeam? nextPlayingTeam.id : 0,
                player: nextPlayingTeam? nextPlayingTeam.nextPlayer?nextPlayingTeam.nextPlayer:0 : 0,
                victory: false,
                remainingTime: req.body.remainingTime,
                started:false,
                ended:false
            }

            currentGameModule.game.rounds.push(newRound);
        }
        
        

        return res.send(this.newRound);
    };
  }

}
