<template>
  <div class="container">
    <!-- Left Section: Table -->
    <div class="table-container">
      <div class="header">
        <h1>Issues</h1>
        <!-- Highlighted Button -->
        <button :class="added ? 'highlighted-btn' : ''" @click="openCommentsPopup">
          Notify
        </button>
      </div>
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
    <div class="notify-overlay" v-if="notify">
      <div class="" style="display: flex; align-items: center;">
        <h1>New Issues Added</h1>
        <button class="close-button" @click="notify = false">X</button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <button @click="closeNewIssues" style=" border: none; font-size: 20px; cursor: pointer;">X</button>
      </div>
      <p>- - - - - - - - - - - - - - - - - - - - - - - - - - - </p>
      <p>- - - - - - - - - - - - - - - - - - - - - - - - - - - </p>
      <p>- - - - - - - - - - - - - - - - - - - - - - - - - - - </p>
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
      added:false,
      notify: false,
      loadingLength: 0,
      newLength: 0,
      counter: 0,
    }
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
    openCommentsPopup(){
      this.notify = true;
    },
    pollingFunction() {
      setInterval(() => {
        this.fetchCommits(); // Fetch chats every 5 seconds
      }, 5000);
    },
    fetchCommits(){
      axios.get("http://localhost:5000/api/comments")
        .then(response => {
          console.log("Commits fetched successfully:", response.data);
          console.log("Commits:", response.data.length);
          if(this.counter == 0){
            console.log("First time fetching comments");
            this.loadingLength = response.data.length;
          }else {
            this.newLength = response.data.length;
            if (this.newLength > this.loadingLength) {
              console.log("New comments available!");
              this.notify = true; // Show the notification if there are new comments
              this.added = true;
            } 
          }
          this.counter++;
        })
        .catch(error => {
          console.error("Error fetching commits:", error);
        });
    },
    closeNewIssues(){
      this.notify = false; // Hide the notification
      this.added = false; // Reset the added state
      this.counter = 0; // Reset the counter
      this.newLength = 0; // Reset the new length
      this.loadingLength = 0; // Update the loading length to the new length
      console.log("New issues closed");
    }
  },
  mounted() {
    // Fetch chats when the component is mounted
    this.pollingFunction()
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

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

/* Highlighted Button */
.highlighted-btn {
  background-color: #ffcc00; /* Yellow background */
  color: #000; /* Black text */
  border: 2px solid #ffa500; /* Orange border */
  padding: 10px 15px;
  font-size: 16px;
  font-weight: bold;
  border-radius: 5px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.highlighted-btn:hover {
  background-color: #ffa500; /* Darker yellow on hover */
  color: #fff; /* White text on hover */
  box-shadow: 0 0 10px #ffa500; /* Glow effect */
}

.highlighted-btn:focus {
  outline: none;
  box-shadow: 0 0 15px #ffcc00; /* Focus glow effect */
}

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

.notify-overlay {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%); /* Center the notification */
  background: white;      
  padding: 20px;
  border-radius: 8px;

  z-index: 1000;
}
</style>