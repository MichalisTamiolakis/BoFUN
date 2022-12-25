import { IPlayer } from "../Player/player";
import { ITeam } from "../Team/team";
import { IRound, MiniGame } from "../Round/round";

var gameSettingsModule = require("../GameSettings/gameSettings.module");

export class Game{
    
    // Game settings
    duration: number;
    totalPlayers: number;
    players: Array<IPlayer>;
    teams: Array<ITeam>;
    pantomime: boolean;
    pictionary: boolean;
    trivia: boolean;
    sequence: Array<number>;
    winningTeam: number;
    rounds: Array<IRound>;

    // Internal game settings
    nextRoundId:number;
    hasGameStarted: boolean;
    hasGameEnded: boolean;


    constructor(duration:number, numberOfTeams:number, numberOfPlayers:number, pantomime:boolean, pictionary:boolean, trivia:boolean){

        let teamsIds: Array<number> = [];
        let colors: Array<string> = gameSettingsModule.availableTeamColors.slice();

        let teams: Array<ITeam> = [];
        for (let i = 0; i < numberOfTeams; i++) {
            let choice = Math.floor(Math.random() * colors.length);
            let chosenColor = colors[choice];
            colors.splice(choice, 1);
            teamsIds.push(i);
            
            teams.push({
            id: i,
            name: "Team " + (i + 1),
            image: "",
            members: [],
            color: chosenColor
            });
        }

        let currentIndex = teamsIds.length,
            randomIndex;

        // While there remain elements to shuffle.
        while (currentIndex != 0) {
            // Pick a remaining element.
            randomIndex = Math.floor(Math.random() * currentIndex);
            currentIndex--;

            // And swap it with the current element.
            [teamsIds[currentIndex], teamsIds[randomIndex]] = [
            teamsIds[randomIndex],
            teamsIds[currentIndex],
            ];
        }

        this.duration = duration;
        this.totalPlayers = numberOfPlayers;
        this.players = [];
        this.teams = teams;
        this.pantomime = pantomime;
        this.pictionary = pictionary;
        this.trivia = trivia;
        this.sequence = teamsIds;
        this.winningTeam = -1;
        this.rounds = [];
        this.hasGameStarted = false;
        this.hasGameEnded = false;
        this.nextRoundId = 0;
    }

    setupDummyGame():void{
        this.duration = 120;
        this.totalPlayers = 4;
        this.players = [
            {
                id:0,
                username:"Michalis",
                teamId: 0,
                image: "",
                positionId:0
            },
            {
                id:1,
                username:"Alexandra",
                teamId: 1,
                image: "",
                positionId:1
            },
            {
                id:2,
                username:"Makis",
                teamId: 0,
                image: "",
                positionId:2
            },
            {
                id:3,
                username:"Ioustini",
                teamId: 1,
                image: "",
                positionId:3
            },

        ]
        this.teams = [
            {
                id:0,
                name: "Team 1",
                image: "",
                members: [0,2],
                color: "#6096BA"
            },
            {
                id:1,
                name: "Team 2",
                image: "",
                members: [1,3],
                color: "#A663CC"
            }
        ];
        this.pantomime = true;
        this.pictionary = true;
        this.trivia = true;
        this.sequence = [1,0];
        this.winningTeam = 1;
        this.rounds = [{
            id: 0, 
            team: 1,
            player: 1,
            minigame: 0,
            minigameJSON: "",
            victory: true,
            remainingTime: 20,
            started: true,
            ended: true,
          },
          {
            id: 1, 
            team: 0,
            player: 0,
            minigame: 1,
            minigameJSON: "",
            victory: true,
            remainingTime: 20,
            started: true,
            ended: true,
          },
          {
            id: 2, 
            team: 1,
            player: 3,
            minigame: 2,
            minigameJSON: "",
            victory: true,
            remainingTime: 20,
            started: true,
            ended: true,
          },
          {
            id: 3, 
            team: 0,
            player: 2,
            minigame: 0,
            minigameJSON: "",
            victory: true,
            remainingTime: 20,
            started: true,
            ended: true,
          },
          {
            id: 4, 
            team: 1,
            player: 1,
            minigame: 1,
            minigameJSON: "",
            victory: false,
            remainingTime: 20,
            started: false,
            ended: true,
          },];
    }

    startGame():void{
        this.hasGameStarted = true;
        this.hasGameEnded = false;
    }

    endGame():void{
        this.hasGameStarted = true;
        this.hasGameEnded = true;
    }

    // Creates a new Round
    async newRound(miniGame:MiniGame):Promise<IRound | undefined>{
        
        let nextTeam:ITeam | undefined = this.getNextTeam();
        if(!nextTeam){
            console.log("Could not create round, next team is undefined");
            return undefined;
        }
        let nextPlayerForTeam:IPlayer | undefined = this.getNextPlayerForTeam(nextTeam);
        if(!nextPlayerForTeam){
            console.log("Could not create round, next player for team:" +nextTeam.id+"is undefined");
            return undefined;
        }
        
        const minigameModule = require('./minigame.module');
        

        let newRound:IRound = {
            id : this.nextRoundId++,
            team : nextTeam.id,
            player: nextPlayerForTeam.id,
            minigame: miniGame,
            minigameJSON: JSON.stringify(await minigameModule.getRandomMinigame(miniGame)),
            victory: false,
            remainingTime: this.duration,
            started: false,
            ended:false
        }

        this.rounds.push(newRound);

        return newRound;
    }

    // Returns a team given its id
    getTeam(teamId:number):ITeam | undefined{
        return this.teams.find(e=>e.id==teamId);
    }

    // Get the player with the given id
    getPlayer(playerId:number):IPlayer | undefined{
        return this.players.find(e=>e.id == playerId);
    }
    
    // Get the round with the given id
    geRound(roundId:number):IRound | undefined{
        return this.rounds.find(e=>e.id == roundId);
    }

    // Get the last round of the given team or undefined if it does not exist
    getLastTeamsRound(team:number | ITeam):IRound|undefined{
        if(typeof team == 'number'){
            for(let i=this.rounds.length-1; i>=0; i--){
                if(this.rounds[i].team == team)
                    return this.rounds[i];
            }
        }
        else{
            for(let i=this.rounds.length-1; i>=0; i--){
                if(this.rounds[i].team == team.id)
                    return this.rounds[i];
            }
        }
        return undefined;
    }

    // Get the current round or undefined if no rounds exist
    getCurrentRound():IRound | undefined{
        if(this.rounds.length>0){
            return this.rounds[this.rounds.length-1];
        }
        return undefined;
    }

    // Gets the next player of a team
    getNextPlayerForTeam(team:number | ITeam):IPlayer | undefined{
        
        let lastTeamsRound:IRound|undefined;
        let teamId:number;
        let teamObj:ITeam | undefined;

        if(typeof team == 'number'){
            teamId = team;
            teamObj = this.getTeam(team);
        }
        else{
            teamId = team.id;
            teamObj = team;
        }

        if(!teamObj){
            console.log("Could not find the team with the given id");
            return undefined;
        }

        
        lastTeamsRound = this.getLastTeamsRound(teamId);

        // There is a last round
        if(lastTeamsRound!=undefined){
            let lastRoundPlayerIndex:number = teamObj.members.indexOf(lastTeamsRound.player);
            let nextPlayerIndex:number = lastRoundPlayerIndex+1;
            if(nextPlayerIndex>=teamObj.members.length){ // wrap around
                nextPlayerIndex = 0;
            }
            return this.getPlayer(teamObj.members[nextPlayerIndex]);

        }
        else{

            if(teamObj.members.length>0){
                return this.getPlayer(teamObj.members[0]);
            }
            else{
                return undefined;
            }
        }

    }
    
    // Gets the next team playing or undefined if no teams exist
    getNextTeam():ITeam|undefined{
        let currentRound:IRound|undefined = this.getCurrentRound();
        
        if(this.sequence.length <= 0 || this.teams.length <= 0)
            return undefined;

        if(currentRound){
            // Find the team of the current round
            let currentRoundTeam:ITeam | undefined = this.getTeam(currentRound.team);
            
            if(currentRoundTeam){
                for(let i=0; i<this.sequence.length; i++){
                    if(this.sequence[i] == currentRoundTeam.id){
                        
                        // Next team index
                        let nextTeamSequenceIndex:number = i+1;
                        if(nextTeamSequenceIndex>=this.sequence.length){
                            nextTeamSequenceIndex=0;
                        }
                        return this.getTeam(this.sequence[nextTeamSequenceIndex]);
                    }
                }

                console.log("Incorrect team id in current round");
                return undefined;
            }
            else{
                console.log("Incorrect team id in current round");
            }

        }
        else{ // This is the first round
            return this.getTeam(this.sequence[0]);
        }
    }
}