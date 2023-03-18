<template>
  <v-container v-if="permLevel == 1" fluid>
    <v-row>
      <v-tabs
        v-model="userOption"
        color="deep-purple-accent-4"
        align-tabs="center"
        >
        <v-tab v-show="" :value="0" disabled ></v-tab>
        <v-tab :value="1">Answer Questions</v-tab>
        <v-tab :value="2" @click="getAnsweredQuestionData()">View Answered Questions</v-tab>
        <v-tab :value="3">Modify/Remove Topics</v-tab>
        <v-tab :value="4">Manage User Privileges</v-tab>
      </v-tabs>
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
          <v-card
            v-for="(answer, index) in answeredQuestions"
            :key="answer.questionId"
          >
            <v-card-title>{{ answer.question }}</v-card-title>
            <v-card-text>Topic: {{ topicList[index] }}</v-card-text>
            <v-card-text>Response: {{ answer.response }}</v-card-text>
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
            <v-textarea
              v-if="selectedTopic !== ''"
              v-model="newTopic"
              label="New Topic"
              rows="1"
              auto-grow
            ></v-textarea>
            <v-btn color="primary" @click="toggleTopicDialog()"
              >Add Topic</v-btn
            >
            <v-btn color="primary" @click="modifyTopic()">Modify Topic</v-btn>
            <v-btn color="secondary" @click="removeTopic()">Remove Topic</v-btn>
          </v-card-text>
        </v-card>

        <!-- Manage User Privileges -->

        <v-card-item v-if="userOption == 4">
          <v-card-title>Manage Users</v-card-title>
          <v-text-field
            v-model="inputUser"
            label="User Email"
            style="width: 25%; padding: 5px"
          ></v-text-field>
          <v-btn color="primary" @click="getFilteredUsers()">Search</v-btn>
          <br /><br />
          <v-card style="margin-top: 2px;">
            <v-card>
              <v-card-title>
              {{ "User: " + returnedUser }}
              </v-card-title>
              <v-row>
                <v-col cols="4" style="margin-left: 15px;">
                  <v-checkbox v-model="userRoles[0]" label="Student"></v-checkbox>
                </v-col>
                <v-col cols="4">
                  <v-checkbox v-model="userRoles[1]" label="Tutor"></v-checkbox>
                </v-col>
                <v-col cols="4" style="margin-left: 15px;">
                  <v-checkbox v-model="userRoles[2]" label="Admin" disabled></v-checkbox>
                </v-col>
              </v-row>
              <v-card-text>
                <v-btn color="primary" @click="updateUser()">Update</v-btn>
              </v-card-text>
            </v-card>
          </v-card>
        </v-card-item>

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
            Topic: {{ topicList[index] }} <br /><br />
            {{ question.question1 }}
            <br /><br />
            Created on:
            <br />
            {{
              question.createdTime.split('T')[0] +
              ' @ ' +
              question.createdTime.split('T')[1].split('.')[0]
            }}
          </v-card-text>
        </v-card>
        <br />
      </v-col>
    </v-row>
    <div>
      <v-dialog v-model="addTopicDialog" max-width="500px">
        <v-card>
          <v-card-title>
            <span class="headline">Add Topic</span>
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
                    v-model="topicName"
                    label="Topic"
                    required
                  ></v-text-field>
                </v-col>
                <v-spacer /><v-btn color="primary" @click="addTopic()"
                  >Add</v-btn
                >
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
import { JWT } from '~/scripts/jwt'
import { Question, AnsweredQuestion } from '~/scripts/interfaces'
import {
  AuthenticationCheck,
  getAskedQuestionTopicName,
  getAnsweredQuestionTopicName,
} from '~/scripts/methods'

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
  newTopic: string = ''
  isLoading: boolean = true
  areYouSure: boolean = false
  addTopicDialog: boolean = false
  topicName: string = ''
  inputUser: string = ''
  returnedUser: string = ''
  userRoles: boolean[] = [false, false, false]

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
        this.topicList = await getAskedQuestionTopicName(
          this.questionData,
          this.$axios
        )
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

  toggleTopicDialog() {
    this.addTopicDialog = !this.addTopicDialog
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
        this.topicList = await getAnsweredQuestionTopicName(
          this.answeredQuestions,
          this.$axios
        )
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

  modifyTopic() {
    if (this.selectedClass === '') {
      alert('Select a class')
      return
    }
    if (this.selectedTopic === '') {
      alert('Select a topic')
      return
    }
    this.areYouSure = confirm(
      'Are you sure you want to modify this topic?' +
        '\n' +
        'Doing so will also modify all question topics associated with this topic.'
    )
    if (!this.areYouSure) {
      return
    }
    this.$axios
      .post(
        '/database/ModifyTopic',
        {},
        {
          params: {
            classCode: this.selectedClass,
            topic: this.selectedTopic,
            newTopic: this.newTopic,
          },
        }
      )
      .then((response) => {
        console.log(response.data)
        this.initializeTopicData()
        this.initializeQuestionData()
      })
  }

  addTopic() {
    this.$axios
      .post(
        '/database/AddTopic',
        {},
        {
          params: {
            classCode: this.classCode,
            topic: this.topicName,
          },
        }
      )
      .then((response) => {
        alert(response.data)
        this.initializeTopicData()
        this.toggleTopicDialog()
      })
  }

  async getFilteredUsers() {
    if(this.inputUser === '') {
      alert('Please enter a username')
      return
    }
    await this.$axios
      .get('database/GetUser', {
        params: {
          username: this.inputUser,
        },
      })      
      .then((response) => {
        console.log(response.data)
        if(response.data === 'No user found') {
          alert('User not found')
        }
        else { 
          this.returnedUser = response.data
        }
      })
      .catch((error) => {
        console.log(error)
      })
      this.getUserRoles()
  }

  getUserRoles() {
    this.$axios
      .get('database/GetUserRoles', {
        params: {
          username: this.returnedUser,
        },
      })
      .then((response) => {
        this.userRoles = response.data
      })
      .catch((error) => {
        console.log(error)
      })
  }

  updateUser() {
    if(this.returnedUser === '') {
      alert('Please search for a user.')
      return
    }
    this.$axios
      .post(
        '/database/ModifyUserRoles',
        {},
        {
          params: {
            username: this.returnedUser,
            isStudent: this.userRoles[0],
            isTutor: this.userRoles[1],
            isAdmin: this.userRoles[2],
          },
        }
      )
      .then((response) => {
        if(response.data === true) alert("The user's roles have been updated.")
        else alert("The user's roles have not been updated.")
      })
      .catch((error) => {
        console.log(error)
      })
  }
}
</script>
