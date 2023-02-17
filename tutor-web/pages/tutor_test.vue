<template>
  <v-container fluid>
    <v-row>
      <v-col cols="8">
        <v-card>
          <v-card-title>Tutor Page</v-card-title>
          <v-card-text>
            <v-select
              v-model="question"
              :items="questionData"
              label="Questions"
              style="width: 25%; padding: 5px;"
            ></v-select>
            <v-textarea v-model="answer" label="Answer" rows="1" ></v-textarea>
            <v-btn color="primary">Submit</v-btn>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="4">
        <v-card v-for="(question, index) in questionData" :key="index" style="margin-bottom: 10px;">
          <v-card-title>Question {{ index + 1 }}</v-card-title>
          <v-card-text>
            {{ question }}
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
  question: string;
}

@Component({})
export default class Tutor extends Vue {
  questionData: Question[] = [];
  question: string = "";
  answer: string = "";

  mounted() {
    this.initializeQuestionData();
  }

  initializeQuestionData() {
    this.$axios.get('/database/GetQuestions', {
      params: { classCode: "1", topic: "1" }
    }).then((response) => {
      console.log(response.data);
      this.questionData = response.data.slice(0, 4)
    })
  };
}

</script>

<!--
  Add way for tutor to select the class and topic
  Figure out how to POST data to the database
  Maybe split into 2 pages. One for the tutor to select class and topic like student
  then pass in class and topic to another page for the tutor to answer the question
-->