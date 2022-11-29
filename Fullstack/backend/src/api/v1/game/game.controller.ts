import { Request, Response, NextFunction, Router } from 'express';
import { NotFound, BadRequest } from 'http-errors';
import { DIContainer, MinioService, SocketsService } from '@app/services';

import { IGame } from "../../../models/Game/game";

var currentGameModule = require('../../../models/Game/currentGame.module');

export class Game{
    public applyRoutes():Router {
        const router = Router();
        
        router.post('/create', this.createGame());


        return router; 

    }

    public createGame(){
        return async (req: Request, res: Response, next?: NextFunction): Promise<Response> => {
            return res.sendStatus(200);
          };
    }
}