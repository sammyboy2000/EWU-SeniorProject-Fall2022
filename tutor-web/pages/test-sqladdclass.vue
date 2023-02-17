<template>
  <div>
    ##Not Implemented##
    <input v-model="classCode" type="text" />
    <button @click="addClass">Add Class</button>
  </div>
</template>

<script lang="ts">
import axios from 'axios';

export default {
  data() {
    return {
      classCode: ''
    };
  },
  methods: {
    async addClass() {
      try {
        await axios.post('/database/addClass', {
          classCode: this.classCode
        });
        alert('Class added successfully!');
      } catch (error) {
        console.error(error);
        alert('An error occurred while adding the class.');
      }
    }
  }
};
</script>