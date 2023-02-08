<template>
<v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
    <v-card class="py-4 justify-center">
        <v-card-title>
        This page will be used to retrieve and answer questions!
        </v-card-title>
        <v-card-text> {{ classData }}</v-card-text>
        <v-select
            v-model="selectedClass"
            :items="classData"
            label="Class"
            style="width: 25%; padding: 5px;"
        ></v-select>
    </v-card>
    </v-col>
</v-row>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({})
export default class Student extends Vue {
    classData: string = ''
    selectedClass: string = ''

    mounted() {
        this.initializeClassData()
    }

    initializeClassData() {
        this.$axios.get('/database/getClasses').then((response) => {
        this.classData = response.data
        })
    }
}
</script>