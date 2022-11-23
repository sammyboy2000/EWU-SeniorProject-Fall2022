<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <v-card>
        <v-card-title class="headline">
          You should automatically be redirected. If not, <v-btn> click here </v-btn>
        </v-card-title>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'

@Component
export default class IndexPage extends Vue {
  name: string = 'IndexPage'
  buttonText: string = 'Inspire me!'
  isLoading: boolean = false

  changeButtonText() {
    this.buttonText =
      this.buttonText === 'Inspire me!' ? 'Inspire me again!' : 'Inspire me!'
    this.isLoading = true
    setTimeout(() => {
      this.isLoading = false
    }, 1000)
  }
}
</script>
