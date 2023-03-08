<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <v-card class="logo py-4 d-flex justify-center">
        <NuxtLogo />
        <VuetifyLogo />
      </v-card>
      <v-card>
        <v-card-title class="headline">
          Welcome to the Vuetify + Nuxt.js template
        </v-card-title>
        <v-card-text>
          <p>
            Vuetify is a progressive Material Design component framework for
            Vue.js. It was designed to empower developers to create amazing
            applications.
          </p>
          <p>
            For more information on Vuetify, check out the
            <a
              href="https://vuetifyjs.com"
              target="_blank"
              rel="noopener noreferrer"
            >
              documentation </a
            >.
          </p>
          <p>
            If you have questions, please join the official
            <a
              href="https://chat.vuetifyjs.com/"
              target="_blank"
              rel="noopener noreferrer"
              title="chat"
            >
              discord </a
            >.
          </p>
          <p>
            Find a bug? Report it on the github
            <a
              href="https://github.com/vuetifyjs/vuetify/issues"
              target="_blank"
              rel="noopener noreferrer"
              title="contribute"
            >
              issue board </a
            >.
          </p>
          <p>
            Thank you for developing with Vuetify and I look forward to bringing
            more exciting features in the future.
          </p>
          <div class="text-xs-right">
            <em><small>&mdash; John Leider</small></em>
          </div>
          <hr class="my-3" />
          <a
            href="https://nuxtjs.org/"
            target="_blank"
            rel="noopener noreferrer"
          >
            Nuxt Documentation
          </a>
          <br />
          <a
            href="https://github.com/nuxt/nuxt.js"
            target="_blank"
            rel="noopener noreferrer"
          >
            Nuxt GitHub
          </a>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="primary" v-if="!isLoggedIn"> <login-dialog /> </v-btn>
          <v-btn color="primary" v-if="!isLoggedIn"> <signup-dialog /> </v-btn>
          <v-btn color="primary" nuxt to="/inspire" v-if="isAdmin"> {{ buttonText }} </v-btn>
          <v-btn
            color="secondary"
            :loading="isLoading"
            @click="changeButtonText"
          v-if="isAdmin">
            Change Text
          </v-btn>
          <v-btn color="primary" nuxt to="/test_pages" v-if="isAdmin"> Test Pages </v-btn>
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
  buttonText: string = 'Inspire me!'
  isLoggedIn: boolean = false
  isLoading: boolean = false
  isAdmin: boolean = false
  isTutor: boolean = false
  isStudent: boolean = false

  mounted() {
      this.checkAdmin()
      this.checkTutor()
      this.checkStudent()
  }
  changeButtonText() {
    this.buttonText =
      this.buttonText === 'Inspire me!' ? 'Inspire me again!' : 'Inspire me!'
    this.isLoading = true
    setTimeout(() => {
      this.isLoading = false
    }, 1000)
  }

  checkAdmin(){
    this.$axios.get('Token/TestAdmin').then((result) => {
          if(result.data === "Authorized as Admin")
          this.isAdmin = true
          this.isLoggedIn = true
        })
  }

  checkTutor(){
    this.$axios.get('Token/TestTutor').then((result) => {
          if(result.data === "Authorized as Tutor")
          this.isTutor = true
          this.isLoggedIn = true
        })
  }

  checkStudent(){
    this.$axios.get('Token/TestStudent').then((result) => {
          if(result.data === "Authorized as Student")
          this.isStudent = true
          this.isLoggedIn = true
        })
  }
}
</script>
