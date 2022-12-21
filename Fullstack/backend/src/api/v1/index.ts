import * as express from 'express';
import { Game } from './game/game.controller';
import { Round } from './round/round.controller';
const apiV1Router = express.Router();


apiV1Router

  .use('/game/', new Game().applyRoutes())
  .use('/game/round', new Round().applyRoutes());


export { apiV1Router };

