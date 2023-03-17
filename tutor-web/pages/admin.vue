<template>
  <v-container v-if="permLevel == 2" fluid>
    <v-row>
      <v-btn @click="userOption = 1, filter = -1">View Questions</v-btn>
      <v-btn @click="userOption = 2, filter = -1">View Statistics</v-btn>
      <v-btn @click="userOption = 3, filter = -1">Add/Modify Classes</v-btn>
      <v-btn @click="userOption = 4, filter = -1">Add/Modify Topics</v-btn>
      <v-btn @click="userOption = 5, filter = -1">Modify Users</v-btn>
    </v-row>
    <v-row justify="center" align="center">
      <v-col cols="12" sm="8" md="6">
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
        <v-card-item v-show="userOption == 3">
          <v-card-title
            >Add/Modify Classes <v-spacer /><v-btn
              color="primary"
              @click="toggleClassDialog"
              >Add</v-btn
            ></v-card-title
          >
          <v-select
            v-model="classFilter"
            :items="classPrefixes.slice(1)"
            label="Filter by Classes"
            style="width: 25%; padding: 5px"
            multiple
            @input="getfilteredClasses(), selectedClass = null"
          ></v-select>
          <v-card v-for="c in classList.slice(1)" :key="c.id" @click="selectClass(c)">
            <v-card-text v-if="classFilter.length == 0">
              {{ c.classCode + ': ' + c.className }}
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
                <v-btn color="primary" @click="updateClass()">Update</v-btn>
                <v-btn color="secondary" @click="removeClass()">Remove</v-btn>
              </v-card-text>
            </v-card-text>
          </v-card>
          <v-card-item>
            <v-card-text v-if="classFilter.length > 0">
              <v-card
                v-for="c in filteredClasses"
                :key="c.id"
                @click="selectClass(c)"
                >
                {{ c.classCode + ': ' + c.className }}
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
                  <v-btn color="primary" @click="updateClass()">Update</v-btn>
                  <v-btn color="secondary" @click="removeClass()">Remove</v-btn>
                </v-card-text>
              </v-card>
            </v-card-text>
          </v-card-item>
        </v-card-item>
        <v-card-item v-show="userOption == 4">
          <v-card-title
            >Add/Modify Topics<v-spacer /><v-btn
              color="primary"
              @click="toggleTopicDialog"
              >Add</v-btn
            ></v-card-title>
          <v-select
            v-model="filter"
            :items="classList"
            item-text="classCode"
            item-value="id"
            label="Filter by Class"
            style="width: 25%; padding: 5px"
          ></v-select>
          <v-card v-for="c in classList.slice(1)" :key="c.id" style="margin-top: 5px;">
            <v-card-text v-if="filter == -1">
              {{ c.classCode }}
              <br />
              <ul>
                <li v-for="topic in filteredTopics(c.id)" :key="topic.id">
                {{ topic.topic1 }}
                </li>
              </ul>
            </v-card-text>
          </v-card>
          <v-card>
            <v-card-text v-if="filter != -1">
              {{ classList[filter].classCode }}
              <br />
              <ul>
                <li v-for="topic in filteredTopics(classList[filter].id)" :key="topic.id">
                {{ topic.topic1 }}
                </li>
              </ul>
            </v-card-text>
          </v-card>
        </v-card-item>
        <v-card v-if="userOption == 5">
          <v-card-title>Modify Users</v-card-title>
          <v-card>

          </v-card>
        </v-card>
      </v-col>
    </v-row>
    <div>
      <v-dialog v-model="addClassDialog" max-width="500px">
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
                <v-spacer /><v-btn color="primary" @click="addClass">Add</v-btn>
              </v-row>
            </v-container>
          </v-card-text>
        </v-card>
      </v-dialog>
    </div>
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
  topicName: string = ''
  selectedClass: AppClass | null = null
  selectedQuestionIndex: number = -1
  permLevel: number = -1
  areYouSure: boolean = false
  addClassDialog: boolean = false
  addTopicDialog: boolean = false
  filter: number = -1
  classFilter: string[] = []
  classPrefixes: string[] = []
  filteredClasses: AppClass[] = []

// Lifecycle
  async mounted() {
    this.permLevel = await AuthenticationCheck(this.$axios)
    if (this.permLevel !== 2) location.assign('/') // Redirect to home page if not a tutor
    this.getQuestions()
    this.getStatistics()
    await this.getClasses()
    this.getTopics()
    this.getClassPrefixes()
  }


// Methods
  toggleClassDialog() {
    this.addClassDialog = !this.addClassDialog
  }

  toggleTopicDialog() {
    this.addTopicDialog = !this.addTopicDialog
  }

  filteredTopics(id: number) {
    return this.topics.filter((t) => t.classId === id)
  }

  getClassPrefixes() {
    let alreadyExists = ''
    this.classPrefixes = []
    this.classList.forEach((c) => {
      let prefix = c.classCode.toString().slice(0, 4)
      if (prefix !== alreadyExists) {
        this.classPrefixes.push(prefix)
        alreadyExists = prefix
        console.log(alreadyExists)
      }
    })
  }

  getfilteredClasses() {
    this.filteredClasses = []
    this.classList.forEach((c) => {
      this.classFilter.forEach((f) => {
        if (c.classCode.toString().slice(0, 4) === f) {
          this.filteredClasses.push(c)
        }
      })
    })

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
    await this.$axios
      .get('Database/getClassesAdmin')
      .then((response) => {
        response.data.unshift({ id: -1, classCode: 'All Classes' })
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
    this.$axios
      .post(
        '/database/AddClass',
        {},
        {
          params: {
            classCode: this.classCode,
            className: this.className,
          },
        }
      )
      .then((response) => {
        alert(response.data)
        this.getClasses()
        this.toggleClassDialog()
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
        this.getTopics()
        this.toggleTopicDialog()
      })
  }

  updateClass() {
    this.$axios
      .post(
        '/database/UpdateClass',
        {},
        {
          params: {
            classCode: this.selectedClass?.classCode,
            className: this.selectedClass?.className,
          },
        }
      )
      .then((response) => {
        alert(response.data)
        this.getClasses()
      })
  }

  
  removeClass() {
    if (this.selectedClass === null) {
      alert('Select a class')
      return
    }
    this.areYouSure = confirm(
      'Are you sure you want to remove this class?' +
        '\n' +
        'Doing so will also remove all questions and topics associated with this class.'
    )
    if (!this.areYouSure) {
      return
    }
    this.$axios
      .post(
        '/database/RemoveClass',
        {},
        {
          params: {
            classId: this.selectedClass?.id,
          },
        }
      )
      .then((response) => {
        alert(response.data)
        this.getClasses()
      })
  }
}
</script>
