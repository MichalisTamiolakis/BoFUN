import mongoose, { Model, model, Schema } from "mongoose";
import { DefaultSchemaOptions } from "../shared";

export interface ITrivia{
    id: number;
    category:string;
    question: string;
    answers: Array<string>;
    correctAnswer: number;
}

const triviaSchema = new Schema(
    {
        _id: {type:Number, required:true},
        category: {type: String, required: true},
        question: {type: String, required: true},
        answers: [String],
        correctAnswer: {type: Number, required: true}
    }
);


export const TriviaModel = mongoose.model('Trivia', triviaSchema);
