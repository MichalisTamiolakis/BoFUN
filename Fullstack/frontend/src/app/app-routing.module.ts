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

const routes: Routes = [
  {
    path: "surroundwall",
    component: SurroundWallComponent,
    children: [
      {path: "pantomime", component: GameDisplayComponent},
      {path: "pictionary", component: PictionaryComponent},
    ]
  },
  {
    path: "smartphone",
    component: SmartphoneComponent,
    children: [
      {path: "join/:positionId", component: JoinTeamComponent},
      {path: "reviewTeam/:teamId/player/:playerId", component: ReviewTeamCardComponent},
      // {path: "pictionary", component: PictionaryComponent},
    ]
  },
  { path: "**", redirectTo: "surroundwall", pathMatch: "full" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
