const mongoose = require('mongoose');
const express = require('express');
const app = express();

const mongoUrl = process.env.MONGO_URL || 'mongodb://localhost:27017/db';

mongoose.connect(mongoUrl, {
  useNewUrlParser: true,
  useUnifiedTopology: true,
}).then(() => {
  console.log('Connected to MongoDB');
}).catch((err) => {
  console.error('MongoDB connection error:', err);
});

app.get('/', (req, res) => res.send('API is running!'));

app.listen(3000, () => console.log('Server started on port 3000'));
