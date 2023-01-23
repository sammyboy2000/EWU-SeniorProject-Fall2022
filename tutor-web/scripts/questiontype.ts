export class QuestionType {
  public isOther(value: string) {
    if (value == 'other')
      return '<div id="otherInput" onchange="QuestionType(value);"><input type="text" value="Input other topic" id="otherInput"></div>'
    else return ''
  }
}
