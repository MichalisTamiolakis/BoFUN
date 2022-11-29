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
import { TeamCardComponent } from './components/team-card/team-card.component';

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
    TeamCardComponent
  ],
  imports: [
    SocketIoModule.forRoot(socketIoConfig),
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
