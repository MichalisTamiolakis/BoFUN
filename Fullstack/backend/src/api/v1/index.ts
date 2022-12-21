import * as express from 'express';
import { GameController } from './game/game.controller';
import { Round } from './round/round.controller';
const apiV1Router = express.Router();


apiV1Router

  .use('/game/', new GameController().applyRoutes())
  .use('/game/round', new Round().applyRoutes());


export { apiV1Router };

