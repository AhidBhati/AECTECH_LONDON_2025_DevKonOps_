const mongoose = require('mongoose');

const chatSchema = new mongoose.Schema({
    createdBy:{
        type: String,
        required: true
    },
    title:{
        type: String,
        required: true
    },
    description:{
        type: String,
        required: true
    },
  status: {
    type: String,
    required: true
  },
  location: {
    type: Object,
    required: true
  },
  image: {
    type: String,
    required: false 
  }
}, { timestamps: true });

module.exports = mongoose.model('Chat', chatSchema);
