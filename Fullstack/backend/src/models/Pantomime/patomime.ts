export interface IPantomime{
    id:number;
    task: string;
    category: Category;
}

enum Category{
    Movie = 0,
    Book = 1,
    Action = 2,
}