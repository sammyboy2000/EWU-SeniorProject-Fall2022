export interface Question {
  questionId: string
  studentId: number
  classId: number
  topicId: number
  question1: string
}

export interface AnsweredQuestion {
  questionId: string
  studentId: number
  tutorId: number
  classId: number
  topicId: number
  question: string
  response: string
}
