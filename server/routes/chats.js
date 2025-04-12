const express = require('express');
const router = express.Router();
const Chat = require('../models/Chats'); // Assuming you have a Chat model

// Create a new chat
router.post('/new', async (req, res) => {
  try {
    console.log("Request body:::", req.body); // Log the request body to see the incoming data
    const chat = new Chat(req.body);
    const savedChat = await chat.save();
    res.status(201).json(savedChat);
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// Get all chats
router.get('/', async (req, res) => {
  try {
    const chats = await Chat.find();
    res.status(200).json(chats);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// Get a single chat by ID
router.get('/:id', async (req, res) => {
  try {
    const chat = await Chat.findById(req.params.id);
    if (!chat) {
      return res.status(404).json({ error: 'Chat not found' });
    }
    res.status(200).json(chat);
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// Update a chat by ID
router.put('/:id', async (req, res) => {
  try {
    const updatedChat = await Chat.findByIdAndUpdate(req.params.id, req.body, { new: true });
    if (!updatedChat) {
      return res.status(404).json({ error: 'Chat not found' });
    }
    res.status(200).json(updatedChat);
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

// Delete a chat by ID
router.delete('/:id', async (req, res) => {
  try {
    const deletedChat = await Chat.findByIdAndDelete(req.params.id);
    if (!deletedChat) {
      return res.status(404).json({ error: 'Chat not found' });
    }
    res.status(200).json({ message: 'Chat deleted successfully' });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

module.exports = router;