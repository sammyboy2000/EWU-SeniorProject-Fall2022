<template>
    <v-row justify="center" align="center">
        <v-col cols="12" sm="8" md="6">
            <v-card class="pt-4 justify-center">
                <v-card-title>
                    <p>Welcome to the Student Question Page.</p>
                    <p >This page will be allow the students to ask questions!</p>
                </v-card-title>

                <v-card-text>
                    <label for="class">Please select a class: </label>
                        <select v-model="selectClass" name="class" style="border-bottom: 2px solid gray">
                            <option value="0">Please select a class</option>
                            <option value="class1">CSCD 211 - Programming Principles I</option>
                            <option value="class2">CSCD 212 - Programming Principles II</option>
                            <option value="class3">CSCD 300 - Data Structures</option>
                            <option value="class4">CSCD 488 - Senior Project</option>
                        </select>
                    <br><br>
                    
                    <div v-show="Number(selectClass) != 0">
                    <label for="topic">Select a topic: </label>
                        <select v-model="selectTopic" name="topic" style="border-bottom: 2px solid gray">
                            <option value="0">Please select a topic</option>
                            <option value="1" name="ifs">If statements</option>
                            <option value="2" name="loops">Loops</option>
                            <option value="other" name="other">Other</option>
                        </select>
                    </div>
                    <br>
                    <div v-show="selectTopic == 'other'">
                        <textarea name="otherBox" style="border:1px solid gray" id="other">Please enter a topic...</textarea>
                    </div>

                </v-card-text>
                <v-row class="px-2">
                    <v-col cols="12">
                        <v-card-actions>
                            <v-spacer></v-spacer>
                            <v-btn class="black red--text"><a href="http://google.com">Submit</a></v-btn>
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
            selectClass: '',
            selectTopic: '',
        }
    }
}

</script>
