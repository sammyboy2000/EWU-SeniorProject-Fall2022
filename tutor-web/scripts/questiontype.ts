

export class QuestionType {
    constructor(char: string) {
        this.char = char
    }

    char: string

    get isOther(): boolean {
        return true
    }
}