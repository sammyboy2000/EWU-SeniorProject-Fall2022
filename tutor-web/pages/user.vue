<template>
    <v-container v-if="isLoading == false && permLevel == 0" align="center" width-max="50%">     
        <v-row justify="center" align="center" style="background-color: darkred">
    <v-col cols="12" sm="8" md="6">       
      <v-card>
            <v-card-title>User Info</v-card-title>
            <v-card-text>
                Email (Optional): 
                <v-text-field v-model="email"></v-text-field>
            </v-card-text>
            <v-card-text>
                New Password (Optional): 
                <v-text-field type="password" v-model="password"></v-text-field>
            </v-card-text>
            <v-card-text>Upon successful update, you will be logged out. Please log back in with your new information.</v-card-text>
            <v-card-text align="right">
                <v-btn nuxt to="/">Home</v-btn>
                <v-btn @click="updateUser()" color="primary">Update Info</v-btn>
            </v-card-text>
        </v-card>
        </v-col>
        </v-row>
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
  export default class User extends Vue {
    email: string = ''
    initialEmail: string = ''
    password: string = ''

  
    permLevel: number = -1 // permission level
    isLoading: boolean = true // is loading
    status: string = ''

    // Mounted functions
    async mounted() {
      this.permLevel = await AuthenticationCheck(this.$axios) // check authentication
      if (this.permLevel !== 0) location.assign('/') // redirect to home page if not student
      this.setEmail()
      this.isLoading = false
    }
  
    // Methods
    setEmail() {
      this.email = JWT.getUserName()
      this.initialEmail = this.email
    }
  
    updateUser(){
        if(this.email == this.initialEmail){
            this.email = ""
        }
        this.$axios.post(
        '/Token/UpdateUser',
        {},
        {
          params: {
            initialUsername: this.initialEmail,
            username: this.email,
            password: this.password,
          },
        }
      )
      .then((response) => {
        this.status = response.data
        alert(this.status)
            location.reload()
      })
    }
    
  }
  </script>
  