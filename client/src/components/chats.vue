<template>
  <div class="container">
    <!-- Left Section: Table -->
    <div class="table-container">
      <h1>Issues</h1>
      <div v-if="chats.length > 0">
        <table>
          <thead>
            <tr>
              <th>Status</th>
              <th>Title</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="chat in chats" :key="chat._id">
              <td style="width: 10px;">{{ chat.status }}</td>
              <td>{{ chat.title }} <br/> <span style="font-size: 10px;">{{ chat.description }}</span></td>
              <td style="width: 10px;">
                <button @click="openViewComments(chat._id)">View</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-else>
        <p>No chats available.</p>
      </div>
    </div>

    <!-- Right Section: Model Viewer -->
    <div class="viewer-container">
      <ifcViewer />
    </div>

    <div v-if="isViewCommentsOpen" class="popup-overlay">
      <div class="popup-content">
        <button class="close-button" @click="closeViewComments">&times;</button>
        <viewComments :chatID="selectedChatID" @close="closeViewComments"/>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import ifcViewer from "./ifcViewer.vue"; // Import the ifcViewer component
import viewComments from "./viewComments.vue";
import { EventBus } from "../eventBus";

export default {
  components: {
    ifcViewer, // Register the ifcViewer component
    viewComments
  },
  data() {
    return {
      chats: [], // Array to store chats
      isViewCommentsOpen: false,
      selectedChatID: null,
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
    openViewComments(chatID) {
      this.selectedChatID = chatID; // Set the selected chat ID
      this.isViewCommentsOpen = true; // Open the popup
    },
        // Close the View Comments popup
      closeViewComments() {
      this.isViewCommentsOpen = false; // Close the popup
      this.selectedChatID = null; // Clear the selected chat ID
    },
  },
  mounted() {
    // Fetch chats when the component is mounted
    this.fetchChats();
    // Listen for the chatCreated event
    EventBus.on("chatCreated", () => {
      console.log("chatCreated event received");
      this.fetchChats(); // Refresh the chats
    });
  },

  beforeDestroy() {
    // Clean up the event listener when the component is destroyed
    EventBus.off("chatCreated");
  },
};
</script>

<style scoped>
/* Container for the layout */
.container {
  display: flex;
  height: 100vh; /* Full height */
}

/* Left Section: Table */
.table-container {
  width: 30%; /* 30% width for the table */
  padding: 20px;
  border-right: 1px solid #ddd; /* Add a border to separate the sections */
  overflow-y: auto; /* Enable scrolling if content overflows */
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 20px;
}

th,
td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}

th {
  background-color: #f4f4f4;
  font-weight: bold;
}

button {
  padding: 5px 10px;
  background-color: #007bff;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

button:hover {
  background-color: #0056b3;
}

/* Right Section: Model Viewer */
.viewer-container {
  width: 70%; /* 70% width for the viewer */
  padding: 20px;
  overflow: hidden; /* Prevent overflow */
}

/* Popup Overlay */
.popup-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

/* Popup Content */
.popup-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  width: 50%;
  max-height: 80%;
  overflow-y: auto;
  position: relative;
}

/* Close Button */
.close-button {
  position: absolute;
  top: 10px;
  right: 10px;
  background: none;
  border: none;
  font-size: 20px;
  cursor: pointer;
}
</style>