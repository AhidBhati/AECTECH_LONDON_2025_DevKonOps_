<template>
    <div>
      <h1>Create Comment</h1>
  
      <!-- Form to Create a New Comment -->
      <form @submit.prevent="createComment">
        <div>
          <label for="description">Description:</label>
          <textarea
            id="description"
            v-model="description"
            placeholder="Enter your comment"
            required
          ></textarea>
        </div>
  
        <button type="submit">Submit Comment</button>
      </form>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  
  export default {
    data() {
      return {
        description: "", // Comment description
      };
    },
    methods: {
      async createComment() {
        try {
          // Prepare the comment data
          const commentData = {
            chatID: this.$route.params.chatID, // Chat ID passed as a prop
            description: this.description,
          };
  
          // Send the data to the backend
          const response = await axios.post("http://localhost:5000/api/comments/new", commentData, {
            headers: {
              "Content-Type": "application/json",
            },
          });
  
          // Clear the form after successful submission
          this.description = "";
  
          alert("Comment created successfully!");
          console.log("Comment created:", response.data);
        } catch (error) {
          console.error("Error creating comment:", error);
          alert("Failed to create comment. Please try again.");
        }
      },
    },
  };
  </script>
  
  <style scoped>
  form {
    margin-top: 20px;
  }
  form div {
    margin-bottom: 15px;
  }
  label {
    display: block;
    font-weight: bold;
    margin-bottom: 5px;
  }
  textarea {
    width: 100%;
    padding: 8px;
    margin-bottom: 10px;
    border: 1px solid #ccc;
    border-radius: 4px;
  }
  button {
    padding: 10px 15px;
    background-color: #007bff;
    color: white;
    border: none;
    cursor: pointer;
  }
  button:hover {
    background-color: #0056b3;
  }
  </style>