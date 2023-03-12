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

export interface Topic {
  id: number
  classId: number
  topic1: string
}

export interface AppClass {
  id: number
  classCode: string
  className: string
  classDesc: string
}
export interface TopicAggregate {
  classCode: string
  classId: number
  topic: string
  topicId: number
  occurences: number
}
