<template>
  <v-container fluid>
    <v-row>
      <v-btn @click="userOption = 1">Ask a Question</v-btn>
      <v-btn @click="userOption = 2">Edit a Question</v-btn>
      <v-btn @click="userOption = 3">Delete a Question</v-btn>
      <v-btn @click="userOption = 4, getAnsweredQuestionData()">View Answered Questions</v-btn>
    </v-row>
    <v-row>
      <v-col cols="8" >
        <v-card v-show="userOption == 1">
          <v-card-title>
            <p>Welcome to the Student Question Page.</p>
            <p>This page will be allow the students to ask questions!</p>
          </v-card-title>
          <v-card-text>
            <v-select v-model="selectedClass" :items="classData" label="Class" style="width: 25%; padding: 5px;"></v-select>
            <br/>
            <div v-show="Number(selectedClass) != 0">
              <v-select
                v-model="selectedTopic"
                :items="topicData"
                label="Topic"
                style="width: 25%; padding: 5px;"
              ></v-select>
            </div>
            <br/>
            <div v-show="selectedTopic == 'other'">
              <textarea v-model="other" name="otherBox" style=" border: 2px solid gray; border-radius: 4px; resize: none; height: 50px; width: 40%; font-size: 14pt;">
                Please enter a topic...
              </textarea>
            </div>
            <br />
            <div>
              <v-header style="font-size: 16pt; color: black">Question:</v-header>
              <br />
                <textarea 
                  v-model="question" name="questionBox"  style="border: 2px solid gray; border-radius: 4px; resize: none; height: 200px; width: 100%; font-size: 14pt;">
                </textarea>
              </div>
            </v-card-text>
            <v-row class="px-2">
              <v-col cols="12">
                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="primary" @click="submitQuestion">
                    Submit
                  </v-btn>
                </v-card-actions>
              </v-col>
            </v-row>
          </v-card>
          <v-card v-show="userOption == 2">
            <v-card-title>Edit Previous Question</v-card-title>
          </v-card>
          <v-card v-show="userOption == 3">
            <v-card-title>Remove Previous Question</v-card-title>
          </v-card>
          <v-card v-show="userOption == 4">
            <v-card-title>View Answered Questions</v-card-title>
            <v-card v-for="answer in answeredQuestions" :key="answer">
              <v-card-title>{{ answer.question }}</v-card-title>
              <v-card-text>{{ answer.response }}</v-card-text>
            </v-card>
          </v-card>
        </v-col>
        <v-col cols="4">
          <v-card style="margin-bottom: 5px;">
            <v-card-title>Previously Asked Questions</v-card-title>
          </v-card>
          <v-card v-for="(question, index) in studentQuestions" :key="index"  style="margin-bottom: 10px;" @click="selectQuestion(index)">
            <v-card-title>Question {{ index + 1 }}</v-card-title>
            <v-card-text>
              {{ question.question1 }}
            </v-card-text>
            <div v-if="selectedQuestionIndex === index">
              <v-textarea v-if="selectedQuestionIndex === index" v-model="question.question1" auto-grow></v-textarea>
              <v-btn color="primary" @click="editSelectedQuestion()">Edit</v-btn>
              <v-btn color="secondary" @click="removeSelectedQuestion()">Remove</v-btn>
            </div>
          </v-card>
          <br />
        </v-col>
      </v-row>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

interface Question {
  questionId: string;
  studentId: number;
  classId: number;
  topicId: number;
  question1: string;
}

interface AnsweredQuestion {
  questionId: string;
  studentId: number;
  tutorId: number;
  classId: number;
  topicId: number;
  question: string;
  response: string;
}

@Component({})
export default class Student extends Vue {
  userOption: number = 1  
  classData: string = ''
  selectedClass: string = ''
  topicData: string = ''
  selectedTopic: string = ''
  selectClass: string = ''
  selectTopic: string = ''
  other: string = ''
  question: string = ''
  modQuestion: string = ''
  studentName: string = 'sshaw16@ewu.edu'
  studentQuestions: Question[] = []
  answeredQuestions: AnsweredQuestion[] = []
  selectedQuestion: Question | null = null;
  selectedQuestionIndex: number = -1;

  mounted() {
    this.initializeClassData()
    this.initializeTopicData()
    this.initializeQuestionData()
    this.getAnsweredQuestionData()
  }

  initializeClassData() {
    this.$axios.get('/database/getClasses').then((response) => {
      this.classData = response.data
    })
  }

  initializeTopicData() {
    this.$axios.get('/database/getTopics').then((response) => {
      this.topicData = response.data
    })
  }

  // Get previous questions asked by student
  initializeQuestionData() {
    this.$axios.get('/Questions/GetStudentsQuestions', {
      params: {
        studentUsername: this.studentName
      }
    }).then((response) => {
      console.log(response.data);
      this.studentQuestions = response.data
    })
  };

  // Get Answered Questions for Student
  getAnsweredQuestionData() {
    this.$axios.get('/Questions/GetAnsweredQuestions', {
      params: {
        studentUsername: this.studentName
      }
    }).then((response) => {
      console.log(response.data);
      this.answeredQuestions = response.data
    })
  };

  submitQuestion() {
    this.$axios.post('/Questions/AskQuestion', {},
    {
      params: {
        studentUsername: this.studentName,
        classCode: this.selectedClass,
        topic: this.selectedTopic,
        question: this.question,
      }
    }).then((response) => {
      console.log(response.data);
      this.initializeQuestionData(); // Refresh the list of questions
    })
  }

  // Not truely implemented yet
  editSelectedQuestion() {
    this.$axios.post('/Questions/EditQuestion', {},
    {
      params: {
        questionId: this.selectedQuestion?.questionId,
        studentUsername: this.selectedQuestion?.studentId,
        classCode: this.selectedQuestion?.classId,
        topic: this.selectedQuestion?.topicId,
        question: this.modQuestion,
      }
    }).then((response) => {
      console.log(response.data);
      this.initializeQuestionData(); // Refresh the list of questions
    })
  }

  // Not truely implemented yet
  removeSelectedQuestion() {
    this.$axios.post('/Questions/RemoveQuestion', {},
    {
      params: {
        questionId: question.questionId,
      }
    }).then((response) => {
      console.log(response.data);
      this.initializeQuestionData(); // Refresh the list of questions
    })
  }

  getAnsweredQuestions() {
    this.$axios.get('/Questions/GetAnsweredQuestions', {
      params: {
        studentUsername: this.studentName
      }
    }).then((response) => {
      console.log(response.data);
      this.answeredQuestions = response.data
    })
  }

  selectQuestion(index: number) {
    this.selectedQuestion = this.studentQuestions[index];
    this.selectedQuestionIndex = index;
  }
  

}
</script>
