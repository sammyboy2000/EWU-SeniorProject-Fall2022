<template>
  <v-container v-if="isLoading == false && permLevel[0] == 0" fluid>
    <v-row>
      <v-tabs
        v-model="userOption"
        align-tabs="center"
        background-color="primary darken-1"
      >
        <v-tab v-show="" :value="0" disabled></v-tab>
        <v-tab :value="1">View Questions</v-tab>
        <v-tab :value="2" @click="getAnsweredQuestionData()"
          >View Answered Questions</v-tab
        >
      </v-tabs>
    </v-row>
    <v-row>
      <v-col cols="8">
        <v-card v-show="userOption == 1">
          <v-card-title>
            <v-header style="font-size: 16pt"
              >Welcome! <br />
              Select a class below to begin</v-header
            >
          </v-card-title>
          <v-card-text>
            <v-select
              v-model="selectedClass"
              :items="classData"
              label="Class"
              style="width: 25%; padding: 5px"
              @change="selectedTopic = ''"
            ></v-select>
            <br />
            <div v-show="Number(selectedClass) != 0">
              <v-select
                v-model="selectedTopic"
                :items="topicData"
                label="Topic"
                style="width: 25%; padding: 5px"
                @click="initializeTopicData()"
              ></v-select>
            </div>
            <br />
            <div v-show="selectedTopic == 'Other'">
              <textarea
                v-model="other"
                name="otherBox"
                style="
                  border: 2px solid gray;
                  border-radius: 4px;
                  resize: none;
                  height: 50px;
                  width: 40%;
                  font-size: 14pt;
                "
              >
                Please enter a topic...
              </textarea>
            </div>
            <br />
            <div>
              <v-header style="font-size: 16pt; font-weight: bold"
                >Question:</v-header
              >
              <br />
              <textarea
                v-model="question"
                name="questionBox"
                style="
                  border: 2px solid gray;
                  border-radius: 4px;
                  resize: none;
                  height: 200px;
                  width: 100%;
                  font-size: 14pt;
                "
              >
              </textarea>
            </div>
          </v-card-text>
          <v-row class="px-2">
            <v-col cols="12">
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="primary" @click="submitQuestion"> Submit </v-btn>
              </v-card-actions>
            </v-col>
          </v-row>
        </v-card>
        <v-card v-show="userOption == 2" v-if="isLoading == false">
          <v-card-title>View Answered Questions</v-card-title>
          <v-card
            v-for="(answer, index) in answeredQuestions"
            :key="answer.questionId"
          >
            <v-card-title>{{ answer.question }}</v-card-title>
            <v-card-text>Class: {{ answeredClassCodeList[index] }}</v-card-text>
            <v-card-text>Topic: {{ answeredtopicList[index] }}</v-card-text>
            <v-card-text>Reply: {{ answer.response }}</v-card-text>
            <v-card-text>
              Created at:
              {{
                answer.createdTime.split('T')[0] +
                ' @ ' +
                answer.createdTime.split('T')[1].split('.')[0]
              }}
              <br />
              Answered at:
              {{
                answer.answeredTime.split('T')[0] +
                ' @ ' +
                answer.answeredTime.split('T')[1].split('.')[0]
              }}
            </v-card-text>
          </v-card>
        </v-card>
      </v-col>
      <v-col cols="4">
        <v-card style="margin-bottom: 5px">
          <v-card-title>Previously Asked Questions</v-card-title>
        </v-card>
        <v-card
          v-for="(question, index) in studentQuestions"
          :key="index"
          style="margin-bottom: 10px"
          @click="selectQuestion(index)"
        >
          <v-card-title>Question {{ index + 1 }}</v-card-title>
          <v-card-text>
            Topic: {{ topicList[index] }} <br /><br />
            Question:<br />{{ question.question1 }} <br /><br />
            Created on:
            <br />
            {{
              question.createdTime.split('T')[0] +
              ' @ ' +
              question.createdTime.split('T')[1].split('.')[0]
            }}
          </v-card-text>
          <div v-if="selectedQuestionIndex === index">
            <v-textarea
              v-if="selectedQuestionIndex === index && editOption == true"
              v-model="modQuestion"
              auto-grow
            ></v-textarea>
            <v-card-item style="padding: 5px">
              <v-btn
                v-if="editOption == false"
                color="primary"
                @click="editOption = true"
                >Edit</v-btn
              >
              <v-btn
                v-if="editOption == true"
                color="primary"
                @click="editSelectedQuestion(), (editOption = false)"
                >Save Changes</v-btn
              >
              <v-btn
                v-if="editOption == false"
                color="secondary"
                @click="removeSelectedQuestion()"
                >Remove</v-btn
              >
            </v-card-item>
          </div>
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
import {
  AuthenticationCheck,
  getAskedQuestionTopicName,
  getAnsweredQuestionTopicName,
  getAnsweredQuestionClassCode,
} from '~/scripts/methods'

@Component({})
export default class Student extends Vue {
  name: string = 'Student'
  userOption: number = 1 // 1 = ask question, 2 = view answered questions
  classData: [] = [] // class data from database
  classCodeList: string[] = [] // class list from database to handle questions class names
  selectedClass: string = '' // selected class from dropdown
  topicData: [] = [] // topic data from database
  topicList: string[] = [] // topic list from database to handle questions topic names
  selectedTopic: string = '' // selected topic from dropdown
  other: string = '' // other topic
  question: string = '' // question
  modQuestion: string = '' // modified question
  studentName: string = '' // student username
  studentQuestions: Question[] = [] // student questions
  answeredQuestions: AnsweredQuestion[] = [] // answered questions
  answeredtopicList: string[] = [] // answered questions topic list
  answeredClassCodeList: string[] = [] // class list from database to handle questions class names
  selectedQuestion: Question | null = null // selected question
  selectedQuestionIndex: number = -1 // selected question index
  editOption: boolean = false // edit option

  permLevel: number[] = [-1, -1, -1] // permission level
  isLoading: boolean = true // is loading

  // Mounted functions
  async mounted() {
    this.permLevel = await AuthenticationCheck(this.$axios) // check authentication
    if (this.permLevel[0] !== 0) location.assign('/') // redirect to home page if not student
    this.setStudentUsername()
    this.initializeClassData()
    this.initializeQuestionData()
    this.getAnsweredQuestionData()
    this.isLoading = false
  }

  // Methods
  setStudentUsername() {
    this.studentName = JWT.getUserName()
  }

  initializeClassData() {
    this.$axios.get('/database/getClasses').then((response) => {
      this.classData = response.data
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
        this.topicData = response.data.concat(['Other'])
      })
  }

  // Get previous questions asked by student
  initializeQuestionData() {
    this.$axios
      .get('/Questions/GetStudentsQuestions', {
        params: {
          studentUsername: this.studentName,
        },
      })
      .then(async (response) => {
        console.log(response.data)
        this.studentQuestions = response.data
        this.topicList = await getAskedQuestionTopicName(
          this.studentQuestions,
          this.$axios
        )
      })
  }

  // Get Answered Questions for Student
  getAnsweredQuestionData() {
    this.$axios
      .get('/Questions/GetAnsweredQuestions', {
        params: {
          studentUsername: this.studentName,
        },
      })
      .then(async (response) => {
        console.log(response.data)
        this.answeredQuestions = response.data
        this.answeredtopicList = await getAnsweredQuestionTopicName(
          this.answeredQuestions,
          this.$axios
        )
        this.answeredClassCodeList = await getAnsweredQuestionClassCode(
          this.answeredQuestions,
          this.$axios
        )
      })
  }

  submitQuestion() {
    if (this.selectedClass === '') {
      alert('Please select a class')
      return
    }
    if (this.selectedTopic === '') {
      alert('Please select a topic')
      return
    }
    if (this.selectedTopic === 'Other' && this.other === '') {
      alert('Please enter an other topic')
      return
    }
    if (this.question === '') {
      alert('Question field cannot be empty')
      return
    }
    if (this.selectedTopic === 'Other') {
      this.selectedTopic = this.other
      this.addTopicToClass()
    }
    this.$axios
      .post(
        '/Questions/AskQuestion',
        {},
        {
          params: {
            studentUsername: this.studentName,
            classCode: this.selectedClass,
            topic: this.selectedTopic,
            question: this.question,
          },
        }
      )
      .then((response) => {
        console.log(response.data)
        this.initializeQuestionData() // Refresh the list of questions
      })
  }

  editSelectedQuestion() {
    this.$axios
      .post(
        '/Questions/StudentModifyQuestion',
        {},
        {
          params: {
            questionId: this.selectedQuestion?.questionId,
            question: this.modQuestion,
          },
        }
      )
      .then((response) => {
        console.log(response.data)
        this.initializeQuestionData() // Refresh the list of questions
      })
  }

  removeSelectedQuestion() {
    this.$axios
      .post(
        '/Questions/StudentRemoveQuestion',
        {},
        {
          params: {
            questionId: this.selectedQuestion?.questionId,
            studentUsername: this.studentName,
          },
        }
      )
      .then((response) => {
        console.log(response.data)
        this.initializeQuestionData() // Refresh the list of questions
      })
  }

  getAnsweredQuestions() {
    this.$axios
      .get('/Questions/GetAnsweredQuestions', {
        params: {
          studentUsername: this.studentName,
        },
      })
      .then((response) => {
        console.log(response.data)
        this.answeredQuestions = response.data
      })
  }

  selectQuestion(index: number) {
    this.selectedQuestion = this.studentQuestions[index]
    this.selectedQuestionIndex = index
  }

  addTopicToClass() {
    console.log('Class: ', this.selectedClass, 'Topic: ', this.other)
    this.$axios
      .post(
        '/database/AddTopic',
        {},
        {
          params: {
            classCode: this.selectedClass,
            topic: this.other,
          },
        }
      )
      .then((response) => {
        console.log(response.data)
      })
  }
}
</script>
