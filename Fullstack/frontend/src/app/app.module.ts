import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TasksComponent } from './pages/tasks/tasks.component';
import { SocketIoModule, SocketIoConfig } from 'ngx-socket-io';
import { environment } from 'src/environments/environment';
import { ItemShopComponent } from './pages/item-shop/item-shop.component';
import { ItemPreviewComponent } from './pages/item-shop/item-preview/item-preview.component';
import { SurroundWallComponent } from './pages/surround-wall/surround-wall.component';
import { GameDisplayComponent } from './pages/surround-wall/game-display/game-display.component';
import { PictionaryComponent } from './pages/surround-wall/pictionary/pictionary.component';
import { SmartphoneComponent } from './pages/smartphone/smartphone.component';
import { JoinTeamComponent } from './pages/smartphone/join-team/join-team.component';
import { TeamCardComponent } from './components/smartphone/team-card/team-card.component';
import { FormsModule } from '@angular/forms';
import { ReviewTeamCardComponent } from './pages/smartphone/review-team-card/review-team-card.component';
import { SelectedTeamCardComponent } from './components/smartphone/selected-team-card/selected-team-card.component';
import { MainComponent } from './pages/surround-wall/main/main.component';
import { PantomimeMobileComponent } from './pages/smartphone/pantomime-mobile/pantomime-mobile.component';
import { PictionaryMobileComponent } from './pages/smartphone/pictionary-mobile/pictionary-mobile.component';
import { TriviaHiddenQuestionComponent } from './pages/smartphone/trivia-hidden-question/trivia-hidden-question.component';
import { TriviaAnswerQuestionComponent } from './pages/smartphone/trivia-answer-question/trivia-answer-question.component';
import { TriviaOptionComponent } from './components/smartphone/trivia-option/trivia-option.component';
import { TriviaComponent } from './pages/surround-wall/trivia/trivia.component';
import { StatisticsComponent } from './pages/surround-wall/statistics/statistics.component';
import { PlayerStatisticsCardComponent } from './components/surround-wall/player-statistics-card/player-statistics-card.component';
import { NgApexchartsModule } from "ng-apexcharts";
import { TeamStatisticsCardComponent } from './components/surround-wall/team-statistics-card/team-statistics-card.component';
import { GameStatisticsCardComponent } from './components/surround-wall/game-statistics-card/game-statistics-card.component';
import { ChartComponent } from './components/surround-wall/chart/chart.component';
import { EndGameComponent } from './pages/surround-wall/end-game/end-game.component';
import { IdleComponent } from './pages/smartphone/idle/idle.component';
const socketIoConfig: SocketIoConfig = { url: environment.host, options: {} };
@NgModule({
  declarations: [
    AppComponent,
    TasksComponent,
    ItemShopComponent,
    ItemPreviewComponent,
    SurroundWallComponent,
    GameDisplayComponent,
    PictionaryComponent,
    SmartphoneComponent,
    JoinTeamComponent,
    TeamCardComponent,
    ReviewTeamCardComponent,
    SelectedTeamCardComponent,
    MainComponent,
    PantomimeMobileComponent,
    PictionaryMobileComponent,
    TriviaHiddenQuestionComponent,
    TriviaAnswerQuestionComponent,
    TriviaOptionComponent,
    TriviaComponent,
    StatisticsComponent,
    PlayerStatisticsCardComponent,
    TeamStatisticsCardComponent,
    GameStatisticsCardComponent,
    ChartComponent,
    EndGameComponent,
    IdleComponent
  ],
  imports: [
    SocketIoModule.forRoot(socketIoConfig),
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgApexchartsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
