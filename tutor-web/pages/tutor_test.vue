<template>
  <v-container fluid>
    <v-row>
      <v-col cols="8">
        <v-card>
          <v-card-title>Tutor Page</v-card-title>
          <v-card-text>
            <v-card-title v-if="selectedQuestion !== null">
              {{ selectedQuestion.question1 }}
            </v-card-title>
            <v-textarea v-model="answer" label="Answer" rows="1" ></v-textarea>
            <v-btn color="primary" @click="answerQuestion()">Submit</v-btn>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="4">
        <v-card style="margin-bottom: 5px;">
          <v-card-title>Recent Questions</v-card-title>
        </v-card>
        <v-card v-for="(question, index) in questionData" :key="index" style="margin-bottom: 10px;" @click="selectQuestion(index)">
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
import { Component, Vue } from "vue-property-decorator";

interface Question {
  questionId: string;
  studentId: number;
  classId: number;
  topicId: number;
  question1: string;
}

@Component({})
export default class Tutor extends Vue {
  questionData: Question[] = [];
  question: string = "";
  answer: string = "";
  classCode: string = "";
  topic: string = "";
  selectedQuestion: Question | null = null;

  mounted() {
    this.initializeQuestionData();
  }

  // Get questions in FIFO order
  initializeQuestionData(options?: string) {
    this.$axios.get('/Questions/GetQuestions', {
      params: { options }
    }).then((response) => {
      console.log(response.data);
      this.questionData = response.data.slice(0, 4);
    })
  };

  answerQuestion() {
      this.$axios.post('/Questions/AnswerQuestion', {},
      {
        params: {
          questionId: this.selectedQuestion?.questionId,
          tutorUsername: "sshaw16@ewu.edu",
          answer: this.answer
        }
      })
    }

  selectQuestion(index: number) {
    this.selectedQuestion = this.questionData[index];
  }



}

</script>

