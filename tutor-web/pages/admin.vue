<template>
  <v-container v-if="permLevel == 2" fluid>
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
            <v-card-text>
              {{ question.question }}
              <br />
              {{ question.response }}
            </v-card-text>
          </v-card>
        </v-card>
        <v-card v-show="userOption == 2">
          <v-card-title>Statistics</v-card-title>
          <v-card v-for="stat in topicStatistics" :key="stat.topicId">
            <v-card-text>
              Class: {{ stat.classCode }} <br />
              Topic: {{ stat.topic }} <br />
              Occurrences: {{ stat.occurences }}
            </v-card-text>
          </v-card>
        </v-card>
        <v-card v-show="userOption == 3">
          <v-card-title>Add/Modify Classes <v-spacer /><v-btn @click="toggleDialog" color="primary">Add</v-btn></v-card-title>
          <v-card v-for="c in classList" :key="c.id" @click="selectClass(c)">
            <v-card-text>
              {{ c.classCode }}
              <v-card-text v-if="selectedClass == c">
                <v-text-field
                  v-model="c.classCode"
                  label="Class Code"
                  required
                ></v-text-field>
                <v-text-field
                  v-model="c.className"
                  label="Class Name"
                  required
                ></v-text-field>
                <v-btn @click="updateClass(c)" color="primary">Update</v-btn>
                <v-btn @click="deleteClass(c)" color="secondary">Delete</v-btn>
              </v-card-text>
            </v-card-text>
          </v-card>
        </v-card>
        <v-card v-show="userOption == 4">
          <v-card-title>Add/Modify Topics</v-card-title>
          <v-card v-for="topic in topics" :key="topic.id">
            <v-card-text>
              {{ classNameFromId(topic.classId) }}
              <br />
              {{ topic.topic1 }}
            </v-card-text>
          </v-card>
        </v-card>
        <v-card v-show="userOption == 5">
          <v-card-title>Check Reqs (WIP)</v-card-title>
        </v-card>
      </v-col>
    </v-row>
    <div>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline">Add Class</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="classCode"
                  label="Class Code"
                  required
                ></v-text-field>
              </v-col>
              <v-col cols="12">
                <v-text-field
                  v-model="className"
                  label="Class Name"
                  required
                ></v-text-field>
              </v-col>
                <v-spacer /><v-btn @click="addClass" color="primary">Add</v-btn>
            </v-row>
          </v-container>
          </v-card-text>
          </v-card>
          </v-dialog>
  </div>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import {
  Question,
  AnsweredQuestion,
  Topic,
  AppClass,
  TopicAggregate,
} from '~/scripts/interfaces'
import { AuthenticationCheck } from '~/scripts/methods'

@Component({})
export default class Admin extends Vue {
  userOption: number = 1
  selectedQuestion: Question | null = null
  unansweredQuestions: Question[] = []
  answeredQuestions: AnsweredQuestion[] = []
  topicStatistics: TopicAggregate[] = []
  classList: AppClass[] = []
  topics: Topic[] = []
  classCode: string = ''
  className: string = ''
  selectedClass: AppClass | null = null
  selectedQuestionIndex: number = -1
  permLevel: number = -1
  dialog: boolean = false


  async mounted() {
    this.permLevel = await AuthenticationCheck(this.$axios)
    if (this.permLevel !== 2) location.assign('/') // Redirect to home page if not a tutor
    this.getQuestions()
    this.getStatistics()
    await this.getClasses()
    this.getTopics()
  }

  toggleDialog() {
    this.dialog = !this.dialog
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
    this.$axios
      .get('Questions/GetAnsweredQuestions')
      .then((response) => {
        this.answeredQuestions = response.data
      })
      .catch((error) => {
        console.log(error)
      })
  }

  getStatistics() {
    this.$axios
      .get('Database/GetQuestionData')
      .then((response) => {
        this.topicStatistics = response.data
      })
      .catch((error) => {
        console.log(error)
      })
  }

  async getClasses() {
    this.$axios
      .get('Database/getClassesAdmin')
      .then((response) => {
        this.classList = response.data
      })
      .catch((error) => {
        console.log(error)
      })
  }

  getTopics() {
    this.$axios
      .get('Database/GetTopicNameAdmin')
      .then((response) => {
        this.topics = response.data
      })
      .catch((error) => {
        console.log(error)
      })
  }

  classNameFromId(i: number): string {
    let s = 'error'
    this.classList.forEach((c) => {
      if (c.id == i) {
        s = c.classCode
        return s
      }
    })
    return s
  }

  selectQuestion(index: number) {
    this.selectedQuestion = this.unansweredQuestions[index]
    this.selectedQuestionIndex = index
  }

  selectClass(c: AppClass) {
    this.selectedClass = c
    console.log(this.selectedClass)
  }

  addClass() {

  }
}
</script>
