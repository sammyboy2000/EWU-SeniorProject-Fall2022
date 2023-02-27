<template>
  <v-container fluid>
    <v-row>
      <v-btn @click="userOption = 1">View Questions</v-btn>
      <v-btn @click="userOption = 2">View Statistics</v-btn>
      <v-btn @click="userOption = 3">[Placeholder]</v-btn>
    </v-row>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="py-4 justify-center">
          <v-card-title>
            This page will be used to view asked questions anonymously and view
            data related to questions asked.
          </v-card-title>
        </v-card>
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

@Component({})
export default class Student extends Vue {
  userOption: number = 1
  selectedQuestion: Question | null = null;
  listQuestions: Question[] = [];
  selectedQuestionIndex: number = -1;

  mounted() {
    
  }

  selectQuestion(index: number) {
    this.selectedQuestion = this.listQuestions[index];
    this.selectedQuestionIndex = index;
  }
  


}
</script>