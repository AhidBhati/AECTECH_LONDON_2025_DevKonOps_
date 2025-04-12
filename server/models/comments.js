const mongoose = require('mongoose');

const commentSchema = new mongoose.Schema({
  chatID: {
    type: mongoose.Schema.Types.ObjectId,
    ref: 'Chat', // Reference to the Chat schema
    required: true,
  },
  description: {
    type: String,
    required: true,
  }
}, { timestamps: true });

module.exports = mongoose.model('Comment', commentSchema);