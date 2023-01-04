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
                    
                    <div>
                    <label for="topic">Select a topic: </label>
                        <select v-model="selected" name="topic" @change="" >
                            <option value="0"></option>
                            <option value="1" name="ifs">If statements</option>
                            <option value="2" name="loops">Loops</option>
                            <option value="3" name="other">Other</option>
                        </select>
                    </div>
                    <div v-show="Number(selected) == 3">
                        <textarea name="otherBox" style="border:1px solid gray" id="other">Please enter a topic...</textarea>
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
export default {
    data() {
        return {
            selected: '',
        }
    }
}

</script>
