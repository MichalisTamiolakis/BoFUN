import { Request, Response, NextFunction, Router } from 'express';
import { NotFound, BadRequest } from 'http-errors';

import { IGame } from "../../../models/Game/game";
import { ITeam } from '../../../models/Team/team';

var currentGameModule = require('../../../models/Game/currentGame.module');
var gameSettingsModule = require('../../../models/GameSettings/gameSettings.module');

export class Game{
    public applyRoutes():Router {
        const router = Router();
        
        router.post('/create', this.createGame());


        return router; 

    }

    public createGame(){
        return async (req: Request, res: Response, next?: NextFunction): Promise<Response> => {
            
            let colors:Array<string> = gameSettingsModule.availableTeamColors;

            let teams:Array<ITeam> = 
            [];

            for (let i = 0; i < req.body.totalTeams; i++) {
                let choice = Math.floor(Math.random()*colors.length);
                let chosenColor = colors[choice];
                colors =colors.splice(choice,1);

                teams.push({
                    id:i,
                    name: "",
                    image: "",
                    members: [],
                    color: chosenColor,
                    sequence: []
                })
            }

            currentGameModule.game = 
            {
                duration: req.body.duration,
                totalPlayers: req.body.totalPlayers,
                players:[],
                teams: teams,
                pantomime: req.body.pantomime,
                pictionary: req.body.pictionary,
                trivia: req.body.trivia,
                sequence: [],
                winningTeam: -1,
                rounds: []
            };
            
            
            return res.sendStatus(200);
          };
    }
}