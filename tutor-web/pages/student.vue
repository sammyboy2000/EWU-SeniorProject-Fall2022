<template>
    <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
    <v-card class="py-4 justify-center">
    <v-card-title>This page will be used to ask a question!</v-card-title>
    <v-card-text>
    <label for="class">Please select a class: </label>
    <select name="class" id="class">
        <option value="CSCD 211 - Programming Principles I">CSCD 211 - Programming Principles I</option>
        <option value="CSCD 212 - Programming Principles II">CSCD 212 - Programming Principles II</option>
        <option value="CSCD 300 - Data Structures">CSCD 300 - Data Structures</option>
        <option value="CSCD 488 - Senior Project">CSCD 488 - Senior Project</option>
    </select>
    <br><br>

    <label for="topic">Select a topic: </label>
    <select name="topic" id="topic">
        <option value="if">if statements</option>
        <option value="loops">loops</option>
    </select>

</v-card-text>
</v-card>
    </v-col>
</v-row>
</template>