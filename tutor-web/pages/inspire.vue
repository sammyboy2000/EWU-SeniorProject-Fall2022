<template>
  <v-row>
    <v-col class="text-center">
        <v-card>Let's check the weather:
          <v-card-text > {{ date }}</v-card-text>
          <v-card-text v-if="!showF"> {{ temperatureC }}</v-card-text>
          <v-card-text v-if="showF"> {{ temperatureF }}</v-card-text>
          <v-card-text > {{ summary }}</v-card-text>
        <footer>
          <small>
            <v-btn
                  color="blue darken-1"
                  text
                  @click="switchUnits"
                > switch units</v-btn>
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
  temperatureC: string = ''
  temperatureF: string = ''
  summary: string = ''
  showF: boolean = false

  mounted() {
    this.initializeWeather()
  }

  switchUnits() {
    this.showF = !this.showF
  }

  initializeWeather() {
    this.$axios.get('Weather').then((response) => {
      this.date = response.data[0].date
      this.temperatureC = `${response.data[0].temperatureC} c`
      this.temperatureF = `${response.data[0].temperatureF} f`
      this.summary = response.data[0].summary
    })
  }
}
</script>