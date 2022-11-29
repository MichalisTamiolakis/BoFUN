import { Request, Response, NextFunction, Router } from 'express';
import { NotFound, BadRequest } from 'http-errors';

import { IGame } from "../../../models/Game/game";
import { ITeam } from '../../../models/Team/team';
import { IPlayer } from '../../../models/Player/player';

var currentGameModule = require('../../../models/Game/currentGame.module');
var gameSettingsModule = require('../../../models/GameSettings/gameSettings.module');

export class Game{
    public applyRoutes():Router {
        const router = Router();
        
        router
        
        .post('/create', this.createGame())
        .post('/createPlayer', this.createPlayer())
        .put('/assignPlayerToTeam/:id',this.assignPlayerToTeam())
        .get('/getGame',this.getGame())
        return router; 

    }

    public getGame(){
        return async (req: Request, res: Response, next?: NextFunction): Promise<Response> => {
            return res.send(currentGameModule.game);
          };
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
                    color: chosenColor
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
            return res.send(currentGameModule.game);
          }
          return res.sendStatus(400);
        };
      }
}