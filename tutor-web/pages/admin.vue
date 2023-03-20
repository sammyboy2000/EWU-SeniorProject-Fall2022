<template>
  <v-container v-if="permLevel[2] == 2" fluid>
    <v-row>
      <v-tabs
        v-model="userOption"
        align-tabs="center"
        background-color="primary darken-1"
      >
        <v-tab v-show="" :value="0" disabled></v-tab>
        <v-tab
          :value="1"
          @click=";(filter = -1), (classCode = ''), (classFilter = [])"
          >View Questions</v-tab
        >
        <v-tab
          :value="2"
          @click=";(filter = -1), (classCode = ''), (classFilter = [])"
          >View Statistics</v-tab
        >
        <v-tab
          :value="3"
          @click=";(filter = -1), (classCode = ''), (classFilter = [])"
          >Add/Modify Classes</v-tab
        >
        <v-tab
          :value="4"
          @click=";(filter = -1), (classCode = ''), (classFilter = [])"
          >Add/Modify Topics</v-tab
        >
        <v-tab
          :value="5"
          @click=";(filter = -1), (classCode = ''), (classFilter = [])"
          >Manage Users</v-tab
        >
      </v-tabs>
    </v-row>

    <!-- This is Option 1 (View Questions) -->
    <v-row v-if="userOption == 1">
      <v-col cols="4" sm="8">
        <v-card-title>Questions</v-card-title>
        <v-select
          v-model="filter"
          :items="classList"
          item-text="classCode"
          item-value="id"
          label="Filter by Class"
          style="width: 25%; padding-left: 15px"
        ></v-select>
      </v-col>
    </v-row>
    <v-divider :thickness="5"></v-divider>
    <v-row v-if="userOption == 1">
      <v-col cols="6">
        <v-card-title>Unanswered Questions</v-card-title>
        <v-card
          v-for="question in unansweredQuestions"
          :key="question.questionId"
          style="margin-top: 2px"
        >
          <v-card-text v-if="filter == -1">
            Class: {{ classNameFromId(question.classId) }} <br />
            Topic: {{ topicNameFromId(question.topicId) }} <br />
            Question: {{ question.question1 }}
          </v-card-text>
          <v-card-text v-if="filter == question.classId">
            Class: {{ classNameFromId(question.classId) }} <br />
            Topic: {{ topicNameFromId(question.topicId) }} <br />
            Question: {{ question.question1 }}
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="6">
        <v-card-title>Responses</v-card-title>
        <v-card
          v-for="question in answeredQuestions"
          :key="question.questionId"
          style="margin-top: 2px"
        >
          <v-card-text v-if="filter == -1">
            Class: {{ classNameFromId(question.classId) }} <br />
            Topic: {{ topicNameFromId(question.topicId) }} <br />
            Question: {{ question.question }} <br />
            Response: {{ question.response }}
          </v-card-text>
          <v-card-text v-if="filter == question.classId">
            Class: {{ classNameFromId(question.classId) }} <br />
            Topic: {{ topicNameFromId(question.topicId) }} <br />
            Question: {{ question.question }} <br />
            Response: {{ question.response }}
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <!-- This is Option 2 (View Statistics) -->
    <v-row v-if="userOption != 1" justify="center" align="center">
      <v-col cols="12" sm="8" md="6">
        <v-card-item v-show="userOption == 2">
          <v-card-title>Statistics</v-card-title>
          <v-select
            v-model="filter"
            :items="classList"
            item-text="classCode"
            item-value="id"
            label="Filter by Class"
            style="width: 25%; padding: 5px"
          ></v-select>
          <v-card
            v-for="stat in topicStatistics"
            :key="stat.topicId"
            style="margin-top: 2px"
          >
            <v-card-text v-if="filter == -1">
              Class: {{ stat.classCode }} <br />
              Topic: {{ stat.topic }} <br />
              Occurrences: {{ stat.occurences }}
            </v-card-text>
            <v-card-text v-if="filter == stat.classId">
              Class: {{ stat.classCode }} <br />
              Topic: {{ stat.topic }} <br />
              Occurrences: {{ stat.occurences }}
            </v-card-text>
          </v-card>
        </v-card-item>

        <!-- This is Option 3 (Add/Modify Classes) -->

        <v-card-item v-show="userOption == 3" class="included">
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
            @input="getFilteredClasses(), (selectedClass = null)"
          ></v-select>
          <v-card
            v-for="c in classList.slice(1)"
            :key="c.id"
            v-click-outside="{ handler: onClickOutside, include }"
            style="margin-top: 2px"
            @click="selectClass(c), (isActive = true)"
          >
            <v-card-text v-if="classFilter.length == 0">
              {{ c.classCode + ': ' + c.className }}
              <v-card-text v-if="isActive == true && selectedClass == c">
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
                style="margin-top: 2px"
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

        <!-- This is Option 4 (Add/Modify Topics) -->

        <v-card-item v-show="userOption == 4">
          <v-card-title
            >Add/Modify Topics<v-spacer /><v-btn
              color="primary"
              @click="toggleTopicDialog(), (classCode = ''), (topicName = '')"
              >Add</v-btn
            >
            &nbsp;
            <v-btn color="secondary" @click="toggleTopicEditDialog()"
              >Modify</v-btn
            >
          </v-card-title>

          <v-select
            v-model="filter"
            :items="classList"
            item-text="classCode"
            item-value="id"
            label="Filter by Class"
            style="width: 25%; padding: 5px"
          ></v-select>
          <v-card
            v-for="c in classList.slice(1)"
            :key="c.id"
            style="margin-top: 2px"
          >
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
                <li
                  v-for="topic in filteredTopics(classList[filter].id)"
                  :key="topic.id"
                >
                  {{ topic.topic1 }}
                </li>
              </ul>
            </v-card-text>
          </v-card>
        </v-card-item>

        <!-- This is Option 5 (Manage Users) -->

        <v-card-item v-if="userOption == 5">
          <v-card-title>Manage Users</v-card-title>
          <v-text-field
            v-model="inputUser"
            label="User Email"
            style="width: 25%; padding: 5px"
          ></v-text-field>
          <v-btn color="primary" @click="getFilteredUsers()">Search</v-btn>
          <br /><br />
          <v-card style="margin-top: 2px">
            <v-card>
              <v-card-title>
                {{ 'User: ' + returnedUser }}
              </v-card-title>
              <v-row>
                <v-col cols="4" style="margin-left: 15px">
                  <v-checkbox
                    v-model="userRoles[0]"
                    label="Student"
                  ></v-checkbox>
                </v-col>
                <v-col cols="4">
                  <v-checkbox v-model="userRoles[1]" label="Tutor"></v-checkbox>
                </v-col>
                <v-col cols="4" style="margin-left: 15px">
                  <v-checkbox v-model="userRoles[2]" label="Admin"></v-checkbox>
                </v-col>
              </v-row>
              <v-card-text>
                <v-btn color="primary" @click="updateUser()">Update</v-btn>
                <v-btn color="secondary" @click="removeUser()">Remove</v-btn>
              </v-card-text>
            </v-card>
          </v-card>
        </v-card-item>
      </v-col>
    </v-row>
    <!--Dialogs-->
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
                  <v-select
                    v-model="classCode"
                    :items="classList.slice(1)"
                    item-text="classCode"
                    item-value="classCode"
                    label="Class"
                    required
                  ></v-select>
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
    <div>
      <v-dialog v-model="editTopicDialog" max-width="500px">
        <v-card>
          <v-card-title>
            <span class="headline">Edit or Remove Topic</span>
          </v-card-title>
          <v-card-text>
            <v-container>
              <v-row>
                <v-col cols="12">
                  <v-select
                    v-model="classCode"
                    :items="classList.slice(1)"
                    item-text="classCode"
                    item-value="id"
                    label="Class Code"
                    required
                  >
                  </v-select>
                </v-col>
                <v-col cols="12">
                  <v-select
                    v-model="topicName"
                    :items="filteredTopics(classCode)"
                    item-text="topic1"
                    item-value="topic1"
                    label="Topic"
                    required
                  >
                  </v-select>
                </v-col>
                <v-spacer /><v-btn
                  color="primary"
                  @click="
                    ;(classCode = classNameFromId(classCode)),
                      toggleRenameTopicDialog()
                  "
                  >Edit</v-btn
                >
                &nbsp;
                <v-btn
                  color="secondary"
                  @click="
                    ;(classCode = classNameFromId(classCode)),
                      removeTopic(),
                      toggleTopicEditDialog()
                  "
                  >Remove</v-btn
                >
              </v-row>
            </v-container>
          </v-card-text>
        </v-card>
      </v-dialog>
      <v-dialog v-model="renameTopicDialog" max-width="450px">
        <v-card>
          <v-card-title>
            <span class="headline">Rename Topic</span>
          </v-card-title>
          <v-card-text>
            <v-container>
              <v-row>
                <v-col cols="12">
                  <v-text-field
                    v-model="renameTopicName"
                    label="Topic"
                    required
                  ></v-text-field>
                </v-col>
                <v-spacer /><v-btn
                  color="primary"
                  @click="
                    renameTopic(),
                      toggleRenameTopicDialog(),
                      toggleTopicEditDialog()
                  "
                  >Confirm</v-btn
                >
              </v-row>
            </v-container>
          </v-card-text>
        </v-card>
      </v-dialog>
    </div>
    <!--End Dialogs-->
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
  renameTopicName: string = ''
  selectedClass: AppClass | null = null
  selectedQuestionIndex: number = -1
  permLevel: number[] = [-1, -1, -1]
  areYouSure: boolean = false
  addClassDialog: boolean = false
  addTopicDialog: boolean = false
  editTopicDialog: boolean = false
  renameTopicDialog: boolean = false
  filter: number = -1
  classFilter: string[] = []
  classPrefixes: string[] = []
  filteredClasses: AppClass[] = []
  inputUser: string = ''
  returnedUser: string = ''
  userRoles: boolean[] = [false, false, false]
  isActive: boolean = false

  // Lifecycle
  async mounted() {
    this.permLevel = await AuthenticationCheck(this.$axios)
    if (this.permLevel[2] !== 2) location.assign('/') // Redirect to home page if not a tutor
    this.getQuestions()
    this.getStatistics()
    await this.getClasses()
    this.getTopics()
    this.getClassPrefixes()
  }

  // Methods
  onClickOutside() {
    this.isActive = false
  }

  include() {
    return [document.querySelector('.included')]
  }

  toggleClassDialog() {
    this.addClassDialog = !this.addClassDialog
  }

  toggleTopicDialog() {
    this.addTopicDialog = !this.addTopicDialog
  }

  toggleTopicEditDialog() {
    this.editTopicDialog = !this.editTopicDialog
  }

  toggleRenameTopicDialog() {
    this.renameTopicDialog = !this.renameTopicDialog
  }

  filteredTopics(id: number) {
    return this.topics.filter((t) => t.classId === id)
  }

  getClassPrefixes() {
    let alreadyExists = ''
    this.classPrefixes = []
    this.classList.forEach((c) => {
      const prefix = c.classCode.toString().slice(0, 4)
      if (prefix !== alreadyExists) {
        this.classPrefixes.push(prefix)
        alreadyExists = prefix
      }
    })
  }

  getFilteredClasses() {
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
      .get('Questions/GetAdminAnsweredQuestions')
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

  topicNameFromId(i: number): string {
    let s = 'error'
    this.topics.forEach((t) => {
      if (t.id == i) {
        s = t.topic1
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

  async getFilteredUsers() {
    if (this.inputUser === '') {
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
        if (response.data === 'No user found') {
          alert('User not found')
        } else {
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
    if (this.returnedUser === '') {
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
        if (response.data === true) {
          this.updateUserRoles()  
          alert("The user's roles have been updated.")
        }
          else alert("The user's roles have not been updated.")
      })
      .catch((error) => {
        console.log(error)
      })
  }

  updateUserRoles() {
    this.$axios.post('/Token/UpdateUserRoles', {}, {
      params: {
        returnedUsername: this.returnedUser,
        updateStudent: this.userRoles[0],
        updateTutor: this.userRoles[1],
        updateAdmin: this.userRoles[2],
      },
    })
    .catch((error) => {
      console.log(error)
    })
  }

  removeUser() {
    if (this.returnedUser === '') {
      alert('Please search for a user.')
      return
    }
    this.$axios
      .post(
        '/database/RemoveUser',
        {},
        {
          params: {
            username: this.returnedUser,
          },
        }
      )
      .then((response) => {
        if (response.data === true) {
          this.removeUserFromToken()
          alert('The user has been removed.')
        }
        else alert('The user has not been removed.')
      })
      .catch((error) => {
        console.log(error)
      })
      location.reload()
  }

  removeUserFromToken() {
    if (this.returnedUser === '') {
      alert('Please search for a user.')
      return
    }
    this.$axios
      .post(
        '/Token/RemoveUser',
        {},
        {
          params: {
            returnedUsername: this.returnedUser,
          },
        }
      )
      .catch((error) => {
        console.log(error)
      })
  }

  renameTopic() {
    this.$axios
      .post(
        '/database/ModifyTopic',
        {},
        {
          params: {
            classCode: this.classCode,
            topic: this.topicName,
            newTopic: this.renameTopicName,
          },
        }
      )
      .then((response) => {
        alert(response.data)
        this.getTopics()
        this.getQuestions()
        this.getStatistics()
      })
  }

  removeTopic() {
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
            classCode: this.classCode,
            topic: this.topicName,
          },
        }
      )
      .then((response) => {
        alert(response.data)
        this.getTopics()
        this.getQuestions()
        this.getStatistics()
      })
  }
} // End of Class
</script>
