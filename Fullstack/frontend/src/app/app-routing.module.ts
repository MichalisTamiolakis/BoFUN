import { TriviaHiddenQuestionComponent } from './pages/smartphone/trivia-hidden-question/trivia-hidden-question.component';
import { PantomimeMobileComponent } from './pages/smartphone/pantomime-mobile/pantomime-mobile.component';
import { ReviewTeamCardComponent } from './pages/smartphone/review-team-card/review-team-card.component';
import { JoinTeamComponent } from './pages/smartphone/join-team/join-team.component';
import { SmartphoneComponent } from './pages/smartphone/smartphone.component';
import { GameDisplayComponent } from './pages/surround-wall/game-display/game-display.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { ItemShopComponent } from './pages/item-shop/item-shop.component';
import { SurroundWallComponent } from './pages/surround-wall/surround-wall.component';
import { PictionaryComponent } from './pages/surround-wall/pictionary/pictionary.component';
import { MainComponent } from './pages/surround-wall/main/main.component';
import { PictionaryMobileComponent } from './pages/smartphone/pictionary-mobile/pictionary-mobile.component';
import { TriviaAnswerQuestionComponent } from './pages/smartphone/trivia-answer-question/trivia-answer-question.component';
import { TriviaComponent } from './pages/surround-wall/trivia/trivia.component';
import { StatisticsComponent } from './pages/surround-wall/statistics/statistics.component';
import { EndGameComponent } from './pages/surround-wall/end-game/end-game.component';

const routes: Routes = [
  {
    path: "surroundwall",
    component: SurroundWallComponent,
    children: [
      {path: "pantomime", component: GameDisplayComponent},
      {path: "pictionary", component: PictionaryComponent},
      {path: "main", component: MainComponent},
      {path: "trivia", component: TriviaComponent},
      {path: "statistics", component: StatisticsComponent},
      {path: "endGame", component: EndGameComponent},
    ]
  },
  {
    path: "smartphone",
    component: SmartphoneComponent,
    children: [
      {path: "join/:positionId", component: JoinTeamComponent},
      {path: "reviewTeam/:teamId/player/:playerId", component: ReviewTeamCardComponent},
      {path: "pantomime/:playerId", component: PantomimeMobileComponent},
      {path: "pictionary/:playerId", component: PictionaryMobileComponent},
      {path: "trivia/:playerId/hiddenQuestion", component: TriviaHiddenQuestionComponent},
      {path: "trivia/:playerId/answerQuestion", component: TriviaAnswerQuestionComponent},
    ]
  },
  { path: "**", redirectTo: "surroundwall", pathMatch: "full" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
