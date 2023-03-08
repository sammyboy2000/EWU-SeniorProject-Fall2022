<template>
  <v-container fluid>
    <v-row>
      <v-btn @click="userOption = 1">Ask a Question</v-btn>
      <v-btn @click=";(userOption = 2), getAnsweredQuestionData()"
        >View Answered Questions</v-btn
      >
    </v-row>
    <v-row>
      <v-col cols="8">
        <v-card v-show="userOption == 1">
          <v-card-title>
            <p>Welcome to the Student Question Page.</p>
            <p>This page will be allow the students to ask questions!</p>
          </v-card-title>
          <v-card-text>
            <v-select
              v-model="selectedClass"
              :items="classData"
              label="Class"
              style="width: 25%; padding: 5px"
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
              <v-header style="font-size: 16pt; color: black"
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
        <v-card v-show="userOption == 2">
          <v-card-title>View Answered Questions</v-card-title>
          <v-card v-for="answer in answeredQuestions" :key="answer.questionId">
            <v-card-title>{{ answer.question }}</v-card-title>
            <v-card-text>{{ answer.topicId }}</v-card-text>
            <v-card-text>{{ answer.response }}</v-card-text>
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
            {{ question.question1 }}
          </v-card-text>
          <div v-if="selectedQuestionIndex === index">
            <v-textarea
              v-if="selectedQuestionIndex === index && editOption == true"
              v-model="modQuestion"
              auto-grow
            ></v-textarea>
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

@Component({})
export default class Student extends Vue {
  name: string = 'Student'
  userOption: number = 1 // 1 = ask question, 2 = view answered questions
  classData: [] = [] // class data from database
  selectedClass: string = '' // selected class from dropdown
  topicData: [] = [] // topic data from database
  selectedTopic: string = '' // selected topic from dropdown
  other: string = '' // other topic
  question: string = '' // question
  modQuestion: string = '' // modified question
  studentName: string = JWT.getUserName() // student username
  studentQuestions: Question[] = [] // student questions
  answeredQuestions: AnsweredQuestion[] = [] // answered questions
  selectedQuestion: Question | null = null // selected question
  selectedQuestionIndex: number = -1 // selected question index
  editOption: boolean = false // edit option

  // Mounted functions
  mounted() {
    this.initializeClassData()
    this.initializeQuestionData()
    this.getAnsweredQuestionData()
  }

  // Methods

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
      .then((response) => {
        console.log(response.data)
        this.studentQuestions = response.data
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
      .then((response) => {
        console.log(response.data)
        this.answeredQuestions = response.data
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
