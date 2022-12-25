import mongoose, { Schema } from "mongoose";

export interface IPantomime{
    id:number;
    category: string;
    task: string;
}

const pantomimeSchema = new Schema(
    {
        _id: {type:Number, required:true},
        category: {type: String, required: true},
        task: {type: String, required: true}
    }
);


export const PantomimeModel = mongoose.model('Pantomime', pantomimeSchema);