<template>
  <v-container fluid>
    <v-row>
      <v-btn @click="userOption = 1">Answer Questions</v-btn>
      <v-btn @click=";(userOption = 2), getAnsweredQuestionData()"
        >View Answered Questions</v-btn
      >
    </v-row>
    <v-row>
      <v-col cols="8">
        <v-card v-show="userOption == 1">
          <v-card-title>Tutor Page</v-card-title>
          <v-card-text>
            <v-select
              v-model="selectedClass"
              :items="classData"
              label="Class"
              style="width: 25%; padding: 5px"
              @change="initializeQuestionData(selectedClass)"
            ></v-select>
            <v-card-title v-if="selectedQuestion !== null">
              Selected Question on right to answer:
              {{ selectedQuestion.question1 }}
            </v-card-title>
            <v-textarea
              v-model="answer"
              label="Answer"
              rows="1"
              auto-grow
            ></v-textarea>
            <v-btn color="primary" @click="answerQuestion()">Submit</v-btn>
          </v-card-text>
        </v-card>
        <v-card v-show="userOption == 2">
          <v-card-title>View Answered Questions</v-card-title>
          <v-card v-for="answer in answeredQuestions" :key="answer">
            <v-card-title>{{ answer.question }}</v-card-title>
            <v-card-text>{{ answer.response }}</v-card-text>
          </v-card>
        </v-card>
      </v-col>
      <v-col cols="4">
        <v-card style="margin-bottom: 5px">
          <v-card-title>Recent Questions</v-card-title>
        </v-card>
        <v-card
          v-for="(question, index) in questionData"
          :key="index"
          style="margin-bottom: 10px"
          @click="selectQuestion(index)"
        >
          <v-card-title>Question {{ index + 1 }}</v-card-title>
          <v-card-text>
            {{ question.question1 }}
          </v-card-text>
        </v-card>
        <br />
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { JWT } from '~/scripts/jwt'

interface Question {
  questionId: string
  studentId: number
  classId: number
  topicId: number
  question1: string
}

interface AnsweredQuestions {
  questionId: string
  studentId: number
  tutorId: number
  classId: number
  topicId: number
  question: string
  response: string
}

@Component({})
export default class Tutor extends Vue {
  userOption: number = 1
  questionData: Question[] = []
  answeredQuestions: AnsweredQuestions[] = []
  question: string = ''
  answer: string = ''
  classCode: string = ''
  topic: string = ''
  selectedQuestion: Question | null = null
  tutorUsername: string = JWT.getUserName()
  classData: string = ''
  selectedClass: string = ''

  mounted() {
    this.initializeQuestionData()
    this.initializeClassData()
  }

  initializeClassData() {
    this.$axios.get('/database/getClasses').then((response) => {
      this.classData = response.data
    })
  }

  // Get questions in FIFO order
  initializeQuestionData(options?: string) {
    this.$axios
      .get('/Questions/GetQuestions', {
        params: {
          classCode: options,
        },
      })
      .then((response) => {
        console.log(response.data)
        this.questionData = response.data.slice(0, 4)
      })
  }

  answerQuestion() {
    this.$axios.post(
      '/Questions/AnswerQuestion',
      {},
      {
        params: {
          questionId: this.selectedQuestion?.questionId,
          tutorUsername: this.tutorUsername,
          answer: this.answer,
        },
      }
    )
  }

  getAnsweredQuestionData() {
    this.$axios
      .get('/Questions/GetTutorAnsweredQuestions', {
        params: {
          tutorUsername: this.tutorUsername,
        },
      })
      .then((response) => {
        console.log(response.data)
        this.answeredQuestions = response.data
      })
  }

  selectQuestion(index: number) {
    this.selectedQuestion = this.questionData[index]
  }
}
</script>
