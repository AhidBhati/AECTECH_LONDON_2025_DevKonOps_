<template>
    <div>
      <h1>Capture Image and Create Chat</h1>
  
      <!-- Video Stream for Camera -->
      <div v-if="showVideo">
        <video ref="video" autoplay playsinline style="width: 100%; max-width: 400px;"></video>
        <button @click="flipCamera">Flip Camera</button>
      </div>
  
      <!-- Capture Button -->
      <button v-if="showVideo" @click="captureImage">Capture Image</button>
  
      <!-- Preview Captured Image -->
      <div v-if="capturedImage">
        <h3>Preview:</h3>
        <img :src="capturedImage" alt="Captured Image" style="max-width: 400px; margin-top: 10px;" />
      </div>
  
      <!-- Form to Submit Chat -->
      <form @submit.prevent="createChat">
        <!-- <div>
          <label for="createdBy">Created By:</label>
          <input type="text" id="createdBy" v-model="createdBy" required />
        </div> -->
        <div>
          <label for="title">Title:</label>
          <input type="text" id="title" v-model="title" required />
        </div>
        <div>
          <label for="description">Description:</label>
          <input type="text" id="description" v-model="description" required />
        </div>
        <!-- <div>
          <label for="comments">Comments:</label>
          <textarea id="comments" v-model="comments" placeholder="Enter comments (comma-separated)" required></textarea>
        </div> -->
        <!-- <div>
          <label for="status">Status:</label>
          <input type="text" id="status" v-model="status" required />
        </div> -->
        <div>
          <label for="location">Location:</label>
          <input type="text" id="x" v-model="location.x" placeholder="x" required />
          <input type="text" id="y" v-model="location.y" placeholder="y" required />
          <input type="text" id="z" v-model="location.z" placeholder="y" required />
        </div>
        <div>
          <label for="ifc">IFC:</label>
          <input type="text" id="ifc" v-model="ifc" required />
        </div>
  
        <button type="submit">Create Chat</button>
      </form>
    </div>
  </template>
  
  <script>
  import axios from "axios";
  
  export default {
    data() {
      return {
          createdBy: "USER",
          title: "",
          description: "",
          comments: [],
          status: "Open",
          location: {
            x: "",
            y: "",
            z: "",
          },
          ifc: "",
          image: "", // Base64 string of the captured image
        capturedImage: null, // For displaying the captured image
        showVideo: true, // To toggle the video stream visibility
        currentFacingMode: "user", // Default to front camera
      };
    },
    methods: {
      // Start the camera
      async startCamera() {
        try {
          const stream = await navigator.mediaDevices.getUserMedia({
            video: { facingMode: this.currentFacingMode },
          });
          this.$refs.video.srcObject = stream;
        } catch (error) {
          console.error("Error accessing camera:", error);
          alert("Unable to access the camera. Please check your device permissions.");
        }
      },
      // Flip the camera
      async flipCamera() {
        // Toggle between front and back camera
        this.currentFacingMode = this.currentFacingMode === "user" ? "environment" : "user";
  
        // Restart the camera with the new facing mode
        this.startCamera();
      },
      // Capture the image from the video stream
      captureImage() {
        const video = this.$refs.video;
        const canvas = document.createElement("canvas");

        // Resize the image to reduce its size
        const maxWidth = 800; // Set the maximum width
        const scaleFactor = maxWidth / video.videoWidth;

        canvas.width = maxWidth;
        canvas.height = video.videoHeight * scaleFactor;

        const context = canvas.getContext("2d");
        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        // Convert the resized image to a Blob
        canvas.toBlob((blob) => {
            const reader = new FileReader();
            reader.onloadend = () => {
            this.capturedImage = reader.result; // Base64 string
            };
            reader.readAsDataURL(blob); // Convert Blob to Base64
        }, "image/jpeg", 0.7);

        this.showVideo = false; // Minimize the video stream after capturing the image
    },
        async createChat() {
        try {
            let chatData = {
            createdBy: this.createdBy,
            title: this.title,  
            description: this.description,
            // comments: this.comments,
            status: this.status,
            location: this.location,
            ifc: this.ifc,
            image : this.capturedImage, // Base64 string of the captured image

            }
            // formData.append("createdBy", this.createdBy);
            // formData.append("title", this.title);
            // formData.append("description", this.description);
            // formData.append("status", this.status);
            // formData.append("location", JSON.stringify(this.location));
            // formData.append("ifc", this.ifc);

            // Append the image file
            // if (this.capturedImage) {
            // const blob = await fetch(this.capturedImage).then((res) => res.blob());
            // chatData.image = blob; // Store the blob in the chat object
            // }
            console.log("chatData:", chatData); // Log the FormData object for debugging
            // Send the data to the backend
            const response = await axios.post("http://localhost:5000/api/chats/new", chatData, {
                headers: {
                    "Content-Type": "application/json",
                },
                });

            this.capturedImage = null;
            this.showVideo = true;

            console.log("Chat created:", response.data);
        } catch (error) {
            console.error("Error creating chat:", error);
            // alert("Failed to create chat. Please try again.");
        }
        },
    },
    mounted() {
      // Start the camera when the component is mounted
      this.startCamera();
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
  input,
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
  video {
    border: 1px solid #ccc;
    border-radius: 4px;
  }
  img {
    display: block;
    margin-top: 10px;
  }
  </style>