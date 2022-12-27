<template>
    <v-row justify="center" align="center">
        <v-col cols="12" sm="8" md="6">
            <v-card class="pt-4 justify-center">
                <v-card-title>
                    This page will be used to ask a question!
                </v-card-title>
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
                            <select name="topic" v-bind:id="topic" onchange="checkOther(value)">
                                <option value="0"></option>
                                <option value="1" name="ifs">If statements</option>
                                <option value="2" name="loops">Loops</option>
                                <option value="3" name="other">Other</option>
                            </select>
                            <div v-bind:id="otherInput" style="display: none;">
                                <input type="text" value="Input other topic" id="otherInput">
                            </div>
                </v-card-text>
                <v-row class="px-2">
                            <v-col cols="12">
                                <v-card-actions>
                                    <v-spacer></v-spacer>
                                    <v-btn class="black red--text">Submit</v-btn>
                                </v-card-actions>
                            </v-col>
                        </v-row>
            </v-card>
        </v-col>
    </v-row>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'


@Component
export default class Student extends Vue {
    topic: string | undefined;
    otherInput: string | undefined;
    checkOther(value: number) {
        if(value == 3) {
            this.otherInput = "style=\"display: block"
        }
    }
}
</script>
