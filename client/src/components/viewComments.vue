<template>
    <div>
      <div>
      <div style="display: flex; justify-content: space-between; align-items: center;">
      <h1>Create Comment</h1>
      <!-- Close Button -->
      <button @click="closeModal" style=" border: none; font-size: 20px; cursor: pointer;">X</button>
    </div>
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
    <h1>Comments for Chat</h1>
      <div v-if="comments.length > 0" class="comments-list">
        <h3>Comments:</h3>
        <div v-for="comment in comments" :key="comment._id" class="comment-item">
          <p>{{ comment.description }}</p>
          <small>By: {{ comment.createdBy }}</small>
        </div>
      </div>
  
      <div v-else>
        <p>No comments available for this chat.</p>
      </div>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  
  export default {
    props: {
      chatID: {
      type: String,
      required: true,
    },
  },
    data() {
      return {
        chat: null, // Chat details
        comments: [], // Comments for the chat
        description: "",
      };
    },
    methods: {
      async createComment() {
        try {
          // Prepare the comment data
          const commentData = {
            chatID: this.chatID, // Chat ID passed as a prop
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
          console.log("Comment created:", response.data);
          this.fetchComments(this.chatID);
        } catch (error) {
          console.error("Error creating comment:", error);
          alert("Failed to create comment. Please try again.");
        }
      },
      // Fetch chat details
      async fetchChat(chatID) {
        try {
          const response = await axios.get(`http://localhost:5000/api/chats/${chatID}`);
          this.chat = response.data;
        } catch (error) {
          console.error("Error fetching chat details:", error);
          alert("Failed to load chat details. Please try again.");
        }
      },
      // Fetch comments for the chat
      async fetchComments(chatID) {
        try {
          const response = await axios.get(`http://localhost:5000/api/comments/${chatID}`);
          this.comments = response.data;
        } catch (error) {
          console.error("Error fetching comments:", error);
          alert("Failed to load comments. Please try again.");
        }
      },
      async closeModal(){
        this.$emit("close"); // Emit the close event
      }
    },
    mounted() {
        console.log("Mounted viewComments component"); // Log for debugging
      // Get the chatID from the route params
      const chatID = this.chatID;
        console.log("Chat ID:", chatID); // Log the chatID for debugging
      // Fetch chat details and comments
      this.fetchChat(chatID);
      this.fetchComments(chatID);
    },
  };
  </script>
  
  <style scoped>
  .comments-list {
    margin-top: 20px;
  }
  .comment-item {
    padding: 10px;
    border: 1px solid #ddd;
    border-radius: 5px;
    margin-bottom: 10px;
    background-color: #f9f9f9;
  }
  .comment-item p {
    margin: 0;
  }
  .comment-item small {
    color: #888;
  }
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