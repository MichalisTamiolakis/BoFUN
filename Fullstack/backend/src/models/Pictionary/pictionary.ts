import mongoose, { Schema } from "mongoose";

export interface IPictionary{
    id:number;
    difficulty:number;
    task:string;
}

const pictionarySchema = new Schema(
    {
        _id: {type:Number, required:true},
        difficulty: {type: Number, required: false, default:0},
        task: {type: String, required: true},
    }
);


export const PictionaryModel = mongoose.model('Pictionary', pictionarySchema);