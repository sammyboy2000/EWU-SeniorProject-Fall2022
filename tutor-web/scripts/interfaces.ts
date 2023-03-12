export interface Question {
  questionId: string
  studentId: number
  classId: number
  topicId: number
  question1: string
  createdDate: string
}

export interface AnsweredQuestion {
  questionId: string
  studentId: number
  tutorId: number
  classId: number
  topicId: number
  question: string
  response: string
  createdTime: string
  answeredTime: string
}
