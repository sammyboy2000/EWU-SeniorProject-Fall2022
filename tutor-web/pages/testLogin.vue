<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <v-card>
        <v-card-title class="headline"> Test a permission level: </v-card-title>
        <v-card-text>
          <v-btn color="primary" to="student"> Student </v-btn>
        </v-card-text>
        <v-card-text>
          <v-btn color="primary" to="tutor"> Tutor </v-btn>
        </v-card-text>
        <v-card-text>
          <v-btn color="primary" to="admin"> Admin </v-btn>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>
