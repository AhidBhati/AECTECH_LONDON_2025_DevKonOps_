<template>
  <div>
    <h2>Add a New Project</h2>
    <form @submit.prevent="addProject">
      <div>
        <label for="name">Project Name:</label>
        <input type="text" id="name" v-model="project.name" required />
      </div>

      <div>
        <label for="desc">Project Description:</label>
        <textarea id="desc" v-model="project.desc" required></textarea>
      </div>

      <button type="submit">Add Project</button>
    </form>

    <div v-if="message" :class="messageType">
      {{ message }}
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      project: {
        name: '',
        desc: ''
      },
      message: '',
      messageType: ''
    };
  },
  methods: {
    async addProject() {
      try {
        const response = await axios.post('http://localhost:5000/api/projects/new', this.project);
        console.log(response.data);
        this.message = 'Project added successfully!';
        this.messageType = 'success';
        this.project.name = '';
        this.project.desc = '';
      } catch (error) {
        this.message = 'Error adding project: ' + error.response.data.error;
        this.messageType = 'error';
      }
    }
  }
};
</script>

<style scoped>
/* Add some basic styles */
form {
  display: flex;
  flex-direction: column;
  width: 300px;
  margin: 0 auto;
}

form div {
  margin-bottom: 10px;
}

button {
  padding: 10px;
  background-color: #4CAF50;
  color: white;
  border: none;
  cursor: pointer;
}

button:hover {
  background-color: #45a049;
}

.success {
  color: green;
  text-align: center;
}

.error {
  color: red;
  text-align: center;
}
</style>
