<template>
    <div>
      <h1>Chats</h1>
      <!-- Display Chats in Rows -->
      <div v-if="chats.length > 0">
        <div v-for="chat in chats" :key="chat._id" >
          <div>
            <h3>{{ chat.title }}</h3>
            <p>{{ chat.description }}</p>
            <small>Created By: {{ chat.createdBy }}</small>
            <small>Status: {{ chat.status }}</small>
            <button @click="viewChat(chat._id)">View</button>
          </div>

        </div>
      </div>
  
      <!-- No Chats Message -->
      <div v-else>
        <p>No chats available.</p>
      </div>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  
  export default {
    data() {
      return {
        chats: [], // Array to store chats
      };
    },
    methods: {
      // Fetch all chats from the backend
      async fetchChats() {
        try {
          const response = await axios.get("http://localhost:5000/api/chats");
          this.chats = response.data;
        } catch (error) {
          console.error("Error fetching chats:", error);
          alert("Failed to load chats. Please try again.");
        }
      },
      // Navigate to view a specific chat
      viewChat(chatID) {
        this.$router.push(`/chats/${chatID}`); // Assuming you have a route for viewing a chat
      },
    },
    mounted() {
      // Fetch chats when the component is mounted
      this.fetchChats();
    },
  };
  </script>
  
  <style scoped>
  </style>