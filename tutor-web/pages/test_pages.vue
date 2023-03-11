<template>
  <container fluid>
    <v-row v-if="isAdmin">
      <v-col>
        <v-title>
          This page is used for testing purposes and contains a directory to
          test pages
        </v-title>
        <v-text-card>
          <br />
          <v-btn color="primary" nuxt to="/student"> Test Student </v-btn>
          <br /><br />
          <v-btn color="primary" nuxt to="/tutor_test"> Test Tutor </v-btn>
          <br /><br />
          <v-btn color="primary" nuxt to="/tutor"> Real Tutor Page </v-btn>
          <br /><br />
          <v-btn color="primary" nuxt to="/homepage">
            Test HomePage (WIP)
          </v-btn>
          <br /><br />
          <v-btn color="primary" nuxt to="/admin"> Admin Page </v-btn>
          <br /><br />
        </v-text-card>
      </v-col>
    </v-row>
  </container>
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { JWT } from '~/scripts/jwt'
@Component({})
export default class Test_Pages extends Vue {
  isAdmin: boolean = false

  mounted() {
    this.checkAdmin()
    setTimeout(() => {
      if (!this.isAdmin) location.assign('/')
    }, 1000)
  }

  checkAdmin() {
    this.$axios
      .get('Token/testadmin')
      .then((result) => {
        if (result.data === 'Authorized as Admin') this.isAdmin = true
      })
      .catch()
  }
}
</script>
