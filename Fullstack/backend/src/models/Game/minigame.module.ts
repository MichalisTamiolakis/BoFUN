import { IPantomime, PantomimeModel } from "../Pantomime/patomime";
import { IPictionary, PictionaryModel } from "../Pictionary/pictionary";
import { MiniGame } from "../Round/round";
import { ITrivia, TriviaModel } from "../Trivia/trivia";

async function getRandomMinigame(minigame:MiniGame):Promise<ITrivia | IPantomime | IPictionary | undefined>{
    
    // let foundItem:ITrivia|IPantomime|IPictionary;
    if(minigame === MiniGame.Trivia){

        let count = await TriviaModel.count().exec();
        let random = Math.floor(Math.random() * count);
        let docs = await TriviaModel.findOne().skip(random).exec();
        
        if(docs!=undefined){
            let result:ITrivia ={
                id: docs._id,
                category: docs.category,
                question: docs.question,
                answers: docs.answers,
                correctAnswer: docs.correctAnswer

            };
            return result;
        }

        return undefined;
    }
    else if(minigame === MiniGame.Pantomime){
        let count = await PantomimeModel.count().exec();
        let random = Math.floor(Math.random() * count);
        let docs = await PantomimeModel.findOne().skip(random).exec();
        
        if(docs!=undefined){
            let result:IPantomime ={
                id: docs._id,
                category: docs.category,
                task: docs.task,
            };
            return result;
        }

        return undefined;
    }
    else if(minigame === MiniGame.Pictionary){
        let count = await PictionaryModel.count().exec();
        let random = Math.floor(Math.random() * count);
        let docs = await PictionaryModel.findOne().skip(random).exec();
        
        if(docs!=undefined){
            let result:IPictionary ={
                id: docs._id,
                difficulty: docs.difficulty,
                task: docs.task
            };
            return result;
        }

        return undefined;
    }
    
    return undefined;
}


module.exports.getRandomMinigame = getRandomMinigame;