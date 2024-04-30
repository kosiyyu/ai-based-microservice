import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import ChatView from '../views/ChatView.vue';
import { isAuthenticated } from '@/utils/authUtils';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/chat',
      name: 'chat',
      component: ChatView,
      beforeEnter: (_to, _from, next) => {
        if (isAuthenticated.value) {
          next();
        } else {
          next({ name: 'home'});
        }
      }
    }
  ]
});

export default router;
