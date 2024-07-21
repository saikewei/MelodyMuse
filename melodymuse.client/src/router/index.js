import { createRouter, createWebHistory } from 'vue-router';
import Check from "../pages/Check.vue"
import Home from '../pages/Home.vue';
import MyMusic from '../pages/MyMusic.vue';
import Singer from '../pages/Singer.vue';
import SongList from '../pages/SongList.vue';
import Login from '../pages/Login.vue'
import Register from "../pages/Register.vue"
import Sign from "../pages/Sign.vue"

const routes = [
    {
        path: '/',
        name: 'home',
        component: Home
    },
    {
        path: '/my-music',
        name: 'my-music',
        component: MyMusic
    },
    {
        path: '/singer',
        name: 'singer',
        component: Singer
    },
    {
        path: '/song-list',
        name: 'song-list',
        component: SongList
    },
    {
        path: '/login',
        name: 'Login',
        component: Login
    },
    {
        path: '/register',
        name: 'Register',
        component: Register
    },
    {
        path: '/check',
        name: 'Check',
        component: Check

    }
];

const router = createRouter({
    history: createWebHistory(), // ʹ��Ĭ�ϵ� base URL
    routes
});

export default router;
