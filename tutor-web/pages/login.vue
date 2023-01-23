<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <v-card>
        <v-card-title class="headline">
          You should automatically be redirected. If not,
          <v-btn color="primary" to="testLogin"> click here </v-btn>
        </v-card-title>
      </v-card>
    </v-col>
  </v-row>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
export default class Login extends Vue {
  mounted() {
    setTimeout(() => {
      console.log('active')
    }, 3000)
  }
}
</script>
