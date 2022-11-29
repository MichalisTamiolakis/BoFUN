import * as express from 'express';
import { Game } from './game/game.controller';
const apiV1Router = express.Router();


apiV1Router

  .use('/game/', new Game().applyRoutes());


export { apiV1Router };

