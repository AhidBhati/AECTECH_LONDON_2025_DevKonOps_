const express = require('express');
const router = express.Router();
const Comment = require('../models/comments'); // Assuming you have a Comment model

// Create a new comment for a specific chat
router.post('/new', async (req, res) => {
  try {
    const comment = new Comment(req.body); // Expecting chatID, description, and createdBy in the request body
    const savedComment = await comment.save();
    res.status(201).json(savedComment);
  } catch (error) {
    res.status(400).json({ error: error.message });
  }
});

router.get('/:chatID', async (req, res) => {
    try {
      const { chatID } = req.params;
      const comments = await Comment.find({ chatID }); // Fetch comments where chatID matches
      res.status(200).json(comments);
    } catch (error) {
      res.status(500).json({ error: error.message });
    }
  });

  router.get('/', async (req, res) => {
    try {
      const comments = await Comment.find();
      res.status(200).json(comments);
    } catch (error) {
      res.status(500).json({ error: error.message });
    }
  });

module.exports = router;