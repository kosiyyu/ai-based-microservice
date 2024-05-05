import { createRouter, createWebHistory } from 'vue-router';
import { isAuthenticated, checkIsAuthenticated } from '@/utils/authUtils';
import HomeView from '@/views/HomeView.vue';
import ChatView from '@/views/ChatView.vue';
import RegisterView from '@/views/RegisterView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      beforeEnter: () => {
        checkIsAuthenticated();
      },
      name: 'home',
      component: HomeView
    },
    {
      path: '/chat',
      name: 'chat',
      component: ChatView,
      beforeEnter: (_to, _from, next) => {
        checkIsAuthenticated();
        if (isAuthenticated.value) {
          next();
        } else {
          next({ name: 'home'});
        }
      }
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView
    }
  ]
});

export default router;
