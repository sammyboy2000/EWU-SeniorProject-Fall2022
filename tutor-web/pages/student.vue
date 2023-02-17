<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <v-card class="pt-4 justify-center">
        <v-card-title>
          <p>Welcome to the Student Question Page.</p>
          <p>This page will be allow the students to ask questions!</p>
        </v-card-title>

        <v-card-text>
          <v-select
            v-model="selectedClass"
            :items="classData"
            label="Class"
            style="width: 25%; padding: 5px;"
          ></v-select>
          <br />
          
          <div v-show="Number(selectedClass) != 0">
            <v-select
              v-model="selectedTopic"
              :items="topicData"
              label="Topic"
              style="width: 25%; padding: 5px;"
            ></v-select>
          </div>
          <br />
          <div v-show="selectedTopic == 'other'">
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
                        </textarea
            >
          </div>
          <br />
          <div>
            <v-header style="font-size: 16pt; color: black">Question:</v-header>
            <br />
            <textarea
              id="question"
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
              <v-btn class="black red--text"
                ><a href="http://google.com">Submit</a></v-btn
              >
            </v-card-actions>
          </v-col>
        </v-row>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({})
export default class Student extends Vue {
    classData: string = ''
    selectedClass: string = ''
    topicData: string = ''
    selectedTopic: string = ''
    selectClass: string = ''
    selectTopic: string = ''
    other: string = ''

    mounted() {
        this.initializeClassData()
        this.initializeTopicData()
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
    
}
</script>
