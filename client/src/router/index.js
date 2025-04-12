import { createRouter, createWebHistory } from 'vue-router';
import Chats from '../components/chats.vue'; // Main chats page component
import CreateChats from '../components/createChats.vue';
import CreateComments from '../components/createComments.vue';
import ViewComments from '../components/viewComments.vue'; // Component to view comments for a specific chat

const routes = [
  {
    path: '/',
    name: 'Chats',
    component: Chats, // Main chats page
  },
  {
    path: '/chats/:chatID', 
    name: 'ViewComments',
    component: ViewComments,
  },
  {
    path: '/createChats',
    name: 'CreateChats',
    component: CreateChats, // Page to create a new chat
  },
  {
    path: '/createComments',
    name: 'CreateComments',
    component: CreateComments, // Page to create a new comment
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;