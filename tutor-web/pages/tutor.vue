<template>
  <v-container v-if="permLevel == 1" fluid>
    <v-row>
      <v-btn @click="userOption = 1">Answer Questions</v-btn>
      <v-btn @click=";(userOption = 2), getAnsweredQuestionData()"
        >View Answered Questions</v-btn
      >
      <v-btn @click="userOption = 3">Super Magic Button</v-btn>
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
            Selected Question on right to answer:
            <v-card-title v-if="selectedQuestion !== null">
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
          <v-card v-for="(answer, index) in answeredQuestions" :key="answer.questionId">
            <v-card-title>{{ answer.question }}</v-card-title>
            <v-card-text>Topic: {{ topicList[index] }}</v-card-text>
            <v-card-text>Response: {{ answer.response }}</v-card-text>
            <v-card-text>
              Created at: {{ answer.createdTime.split('T')[0] + " @ " + answer.createdTime.split('T')[1].split('.')[0] }} 
              <br /> 
              Answered at: {{ answer.answeredTime.split('T')[0] + " @ " + answer.answeredTime.split('T')[1].split('.')[0] }}
            </v-card-text>
          </v-card>
        </v-card>

        <v-card v-show="userOption == 3">
          <v-card-title>Modify/Remove Topics</v-card-title>
          <v-card-text>
            <v-select
              v-model="selectedClass"
              :items="classData"
              label="Class"
              style="width: 25%; padding: 5px"
              @change="
                initializeQuestionData(selectedClass), (selectedTopic = '')
              "
            ></v-select>
            <v-select
              v-model="selectedTopic"
              :items="topicData"
              label="Topic"
              style="width: 25%; padding: 5px"
              @click="initializeTopicData()"
            ></v-select>
            <v-btn color="primary" @click="modifyTopic()">Modify Topic</v-btn>
            <v-btn color="primary" @click="removeTopic()">Remove Topic</v-btn>
          </v-card-text>
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
            Topic: {{ topicList[index] }}
            <br /><br />
            {{ question.question1 }}
            <br /><br />
            Created on: 
            <br />
            {{ question.createdTime.split('T')[0] + " @ " + question.createdTime.split('T')[1].split('.')[0] }}
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
import { Question, AnsweredQuestion } from '~/scripts/interfaces'
import { AuthenticationCheck, getAskedQuestionTopicName, getAnsweredQuestionTopicName } from '~/scripts/methods'

@Component({})
export default class Tutor extends Vue {
  userOption: number = 1
  questionData: Question[] = []
  answeredQuestions: AnsweredQuestion[] = []
  question: string = ''
  answer: string = ''
  classCode: string = ''
  topic: string = ''
  selectedQuestion: Question | null = null
  tutorUsername: string = ''
  classData: [] = []
  selectedClass: string = ''
  topicData: [] = []
  topicList: string[] = []
  selectedTopic: string = ''
  isLoading: boolean = true
  areYouSure: boolean = false

  permLevel: number = -1

  async mounted() {
    this.permLevel = await AuthenticationCheck(this.$axios) // Check authentication
    if (this.permLevel !== 1) location.assign('/') // Redirect to home page if not a tutor
    this.setTutorUsername()
    this.initializeQuestionData()
    this.initializeClassData()
    this.isLoading = false
  }

  setTutorUsername() {
    this.tutorUsername = JWT.getUserName()
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
      .then(async (response) => {
        this.questionData = response.data.slice(0, 4)
        this.topicList = await getAskedQuestionTopicName(this.questionData, this.$axios)
      })
  }

  initializeTopicData() {
    this.$axios
      .get('/database/getTopics', {
        params: {
          classCode: this.selectedClass,
        },
      })
      .then((response) => {
        this.topicData = response.data
      })
  }

  answerQuestion() {
    if (this.selectedClass === '') {
      alert('Select a class')
      return
    }
    if (this.selectedQuestion === null) {
      alert('A question must be selected from the list')
      return
    }
    if (this.answer === '') {
      alert('Answer field cannot be empty')
      return
    }
    console.log(this.tutorUsername)
    this.$axios
      .post(
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
      .then((response) => {
        console.log(response.data)
        this.initializeQuestionData()
      })
  }

  getAnsweredQuestionData() {
    this.$axios
      .get('/Questions/GetTutorAnsweredQuestions', {
        params: {
          tutorUsername: this.tutorUsername,
        },
      })
      .then(async (response) => {
        console.log(response.data)
        this.answeredQuestions = response.data
        this.topicList = await getAnsweredQuestionTopicName(this.answeredQuestions, this.$axios)
      })
  }

  selectQuestion(index: number) {
    this.selectedQuestion = this.questionData[index]
  }

  removeTopic() {
    if (this.selectedClass === '') {
      alert('Select a class')
      return
    }
    if (this.selectedTopic === '') {
      alert('Select a topic')
      return
    }
    this.areYouSure = confirm(
      'Are you sure you want to remove this topic?' +
        '\n' +
        'Doing so will also remove all questions associated with this topic.'
    )
    if (!this.areYouSure) {
      return
    }
    this.$axios
      .post(
        '/database/RemoveTopic',
        {},
        {
          params: {
            classCode: this.selectedClass,
            topic: this.selectedTopic,
          },
        }
      )
      .then((response) => {
        console.log(response.data)
        this.initializeTopicData()
        this.initializeQuestionData()
      })
  }
}
</script>
