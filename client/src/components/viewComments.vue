<template>
    <div>
      <h1>Comments for Chat</h1>
  

      <!-- <div v-if="chat">
        <h2>{{ chat.title }}</h2>
        <p>{{ chat.description }}</p>
        <small>Created By: {{ chat.createdBy }}</small>
        <small>Status: {{ chat.status }}</small>
      </div>
  
      <div v-if="comments.length > 0" class="comments-list">
        <h3>Comments:</h3>
        <div v-for="comment in comments" :key="comment._id" class="comment-item">
          <p>{{ comment.description }}</p>
          <small>By: {{ comment.createdBy }}</small>
        </div>
      </div>
  
      <div v-else>
        <p>No comments available for this chat.</p>
      </div> -->
    </div>
  </template>
  
  <script>
  import axios from "axios";
  
  export default {
    data() {
      return {
        chat: null, // Chat details
        comments: [], // Comments for the chat
      };
    },
    methods: {
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
    },
    mounted() {
        console.log("Mounted viewComments component"); // Log for debugging
      // Get the chatID from the route params
      const chatID = this.$route.params.chatID;
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
  </style>