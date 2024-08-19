import { createRouter, createWebHistory } from "vue-router";
import Home from "../pages/Home.vue";
import MyMusic from "../pages/MyMusic.vue";
import Singer from "../pages/Singer.vue";
import SongList from "../pages/SongList.vue";
import Login from "../pages/Login.vue";
import Register from "../pages/Register.vue";
import Sign from "../pages/Sign.vue";
import UserManage from "../pages/UserManage.vue"; //引入用于检测用户管理界面

const routes = [
  {
    path: "/",
    name: "home",
    component: Home,
  },
  {
    path: "/my-music",
    name: "my-music",
    component: MyMusic,
  },
  {
    path: "/singer",
    name: "singer",
    component: Singer,
  },
  {
    path: "/song-list",
    name: "song-list",
    component: SongList,
  },
  {
    path: "/login",
    name: "Login",
    component: Login,
  },
  {
    path: "/register",
    name: "Register",
    component: Register,
  },
  {
    path: "/usermanage",
    name: "usermanage", //引入用于检测用户管理界面
    component: UserManage, //引入用于检测用户管理界面
  },
];

const router = createRouter({
  history: createWebHistory(), // 使用默认的 base URL
  routes,
});

export default router;
