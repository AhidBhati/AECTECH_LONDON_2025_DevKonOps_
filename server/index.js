const express = require('express');
const mongoose = require('mongoose');
const cors = require('cors');
const app = express();
const projectRoutes = require('./routes/projects'); // Correct path to the routes file
const chatRoutes = require('./routes/chats'); // Correct path to the routes file
const commentRoutes = require('./routes/comments'); // Correct path to the routes file


const PORT = process.env.PORT || 5000;

// Enable CORS for all origins (this is good for development, but you may want to restrict it in production)
app.use(cors());

// Body parser middleware to parse incoming JSON requests
app.use(express.json());

app.use(express.static('public', {
  setHeaders: (res, path) => {
    if (path.endsWith('.wasm')) {
      res.setHeader('Content-Type', 'application/wasm');
    }
  }
}));

// Your MongoDB connection setup
mongoose.connect('mongodb+srv://Rhishi:rhishi@cluster0.gcypbm5.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0', {
  useNewUrlParser: true,
  useUnifiedTopology: true
}).then(() => console.log('MongoDB connected'))
  .catch(err => console.log(err));

// Define your routes here
app.use('/api/projects', projectRoutes); // Attach routes with a base path
// Define your routes here
app.use('/api/chats', chatRoutes); // Attach routes with a base path
// Define your routes here
app.use('/api/comments', commentRoutes); // Attach routes with a base path

// Start the server
app.listen(PORT, () => {
  console.log(`Server running on http://localhost:${PORT}`);
});
