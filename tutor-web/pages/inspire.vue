<template>
  <v-row>
    <v-col class="text-center">
        <v-card>Let's check the weather:
          <v-card-text > {{ date }}</v-card-text>
          <v-card-text v-if="!showF"> {{ temperatureC }}</v-card-text>
          <v-card-text v-if="showF"> {{ temperatureF }}</v-card-text>
          <v-card-text > {{ summary }}</v-card-text>
          <v-btn
                  color="blue darken-1"
                  text
                  @click="switchUnits"
                > switch units</v-btn>
        <footer>
          <small>
            <em>&mdash;in Spokane</em>
          </small>
        </footer>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
@Component({})
export default class Weather extends Vue {
  date: Date = new Date
  temperatureC: number = 0
  temperatureF: number= 0
  summary: string = ''
  showF: boolean = false

  mounted() {
    this.initializeWeather()
  }

  switchUnits() {
    this.showF = !this.showF
  }

  initializeWeather() {
    this.$axios.get('/api/Weather').then((response) => {
      this.date = response.data.date
      this.temperatureC = response.data.temperatureC
      this.temperatureF = response.data.temperatureF
      this.summary = response.data.summary
    })
  }
}
</script>