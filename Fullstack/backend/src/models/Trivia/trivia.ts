export interface ITrivia{
    id: number;
    category:string;
    question: string;
    answers: Array<string>;
    correctAnswer: number;
}