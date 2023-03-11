<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <v-card class="logo py-4 d-flex justify-center">
        <v-img src="\EWUlogoAlt.png" max-height="30%" :contain="true"> </v-img>
      </v-card>
      <v-card>
        <v-card-title v-if="!isLoggedIn" class="headline">
          Welcome to EWU Tutoring.
        </v-card-title>
        <v-card-title v-if="isLoggedIn" class="headline">
          Welcome to back to EWU Tutoring{{ userName }}.
        </v-card-title>
        <v-card-text>
          <p v-if="isLoggedIn">What would you like to do today?</p>
          <p v-if="!isLoggedIn">Please log in to continue</p>
          <p>If you have questions, please email sshaw16@ewu.edu.</p>
          <p>
            Find a bug? Report it on our github
            <a
              href="https://github.com/sammyboy2000/ewu-seniorproject-fall2022/issues"
              target="_blank"
              rel="noopener noreferrer"
              title="contribute"
            >
              issue board </a
            >.
          </p>
          <p v-if="isLoggedIn && !isTutor && !isAdmin">
            Want to become a tutor? Ask an existing tutor to add you to the
            list!
          </p>
          <div class="text-xs-right">
            <em><small>&mdash; A Fall 2022 Senior Project</small></em>
          </div>
          <hr class="my-3" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn v-if="!isLoggedIn" color="primary"> <login-dialog /> </v-btn>
          <v-btn v-if="!isLoggedIn" color="primary"> <signup-dialog /> </v-btn>
          <v-btn v-if="isStudent" color="primary" nuxt to="/student">
            Ask A Question</v-btn
          >
          <v-btn v-if="isTutor" color="primary" nuxt to="/tutor">
            Answer A Question</v-btn
          >
          <v-btn v-if="isAdmin" color="primary" nuxt to="/admin">
            Admin Tools
          </v-btn>
          <v-btn v-if="isAdmin" color="primary" nuxt to="/test_pages">
            Test Pages
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'
import { JWT } from '~/scripts/jwt'

@Component
export default class IndexPage extends Vue {
  name: string = 'IndexPage'
  userName: string = ''
  isLoggedIn: boolean = false
  isLoading: boolean = false
  isAdmin: boolean = false
  isTutor: boolean = false
  isStudent: boolean = false

  mounted() {
    this.checkAdmin()
    this.checkTutor()
    this.checkStudent()
    setTimeout(() => {
      if (this.isLoggedIn) {
        this.checkName()
      }
    }, 200)
  }

  checkAdmin() {
    this.$axios
      .get('Token/testadmin')
      .then((result) => {
        if (result.data === 'Authorized as Admin') this.isAdmin = true
        this.isLoggedIn = true
      })
      .catch(function (error) {})
  }

  checkTutor() {
    this.$axios
      .get('Token/testtutor')
      .then((result) => {
        if (result.data === 'Authorized as Tutor') this.isTutor = true
        this.isLoggedIn = true
      })
      .catch(function (error) {})
  }

  checkStudent() {
    this.$axios
      .get('Token/teststudent')
      .then((result) => {
        if (result.data === 'Authorized as Student') this.isStudent = true
        this.isLoggedIn = true
      })
      .catch(function (error) {})
  }

  checkName() {
    this.$axios
      .post(
        '/Token/getName',
        {},
        {
          params: {
            username: JWT.getUserName(),
          },
        }
      )
      .then((result) => {
        this.userName = ', ' + result.data
      })
      .catch(function (error) {})
  }
}
</script>
