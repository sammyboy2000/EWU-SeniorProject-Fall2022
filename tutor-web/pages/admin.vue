<template>
  <v-container fluid>
    <v-row>
      <v-btn @click="userOption = 1">View Questions</v-btn>
      <v-btn @click="userOption = 2">View Statistics</v-btn>
      <v-btn @click="userOption = 3">Add/Modify Classes</v-btn>
      <v-btn @click="userOption = 4">Add/Modify Topics</v-btn>
      <v-btn @click="userOption = 5">Add/Modify Associated Classes?</v-btn>
    </v-row>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="py-4 justify-center">
          <v-card-title>
            This page will be used to view asked questions anonymously and view
            data related to questions asked.
          </v-card-title>
        </v-card>
        <v-card v-show="userOption == 1">
          <v-card-title>Questions</v-card-title>
          <v-card
            v-for="question in unansweredQuestions"
            :key="question.questionId"
          >
            <v-card-text>{{ question.question1 }}</v-card-text>
          </v-card>
          <v-card-title>Answered Questions</v-card-title>
          <v-card
            v-for="question in answeredQuestions"
            :key="question.questionId"
          >
            <v-card-text>{{ question.question1 }}</v-card-text>
          </v-card>
        </v-card>
        <v-card v-show="userOption == 2">
          <v-card-title>Statistics (WIP)</v-card-title>
        </v-card>
        <v-card v-show="userOption == 3">
          <v-card-title>Add/Modify Classes</v-card-title>
        </v-card>
        <v-card v-show="userOption == 4">
          <v-card-title>Add/Modify Topics</v-card-title>
        </v-card>
        <v-card v-show="userOption == 5">
          <v-card-title>Add/Modify Associated Classes? (WIP)</v-card-title>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

interface Question {
  questionId: string
  studentId: number
  classId: number
  topicId: number
  question1: string
}

@Component({})
export default class Student extends Vue {
  userOption: number = 1
  selectedQuestion: Question | null = null
  unansweredQuestions: Question[] = []
  answeredQuestions: Question[] = []
  selectedQuestionIndex: number = -1

  mounted() {
    this.getQuestions()
  }

  getQuestions() {
    this.$axios
      .get('Questions/GetQuestions')
      .then((response) => {
        this.unansweredQuestions = response.data
      })
      .catch((error) => {
        console.log(error)
      })
  }

  selectQuestion(index: number) {
    this.selectedQuestion = this.unansweredQuestions[index]
    this.selectedQuestionIndex = index
  }
}
</script>
