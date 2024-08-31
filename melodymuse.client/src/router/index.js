import { createRouter, createWebHistory } from "vue-router";
import Home from "../pages/Home.vue";
import MyMusic from "../pages/MyMusic.vue";
import Singer from "../pages/Singer.vue";
import SongList from "../pages/SongList.vue";
import Login from "../pages/Login.vue";
import Register from "../pages/Register.vue";
import Sign from "../pages/Sign.vue";
import SearchResultPage from "../pages/SearchResultPage.vue"; // ?????????????????????;
import UserManage from "../pages/UserManage.vue"; //?????????????????????
import SongInfoEdit from "../pages/SongInfo.vue";
import Check from "../pages/Check.vue";
import modify from "../pages/modify.vue";
import createAlbum from "../pages/CreateAlbum.vue";
import UploadSong from "../pages/UploadSong.vue";
import mediaplayer from "../pages/mediaplayer.vue";

import SingerDetail from "../pages/SingerDetail.vue";
import FollowedArtist from "../pages/FollowedArtist.vue";
import ForgottenPassword from "../pages/ForgottenPassword.vue";
import AlbumDetail from "../pages/AlbumDetail.vue";

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
    path: "/personal-info",
    name: "modify",
    component: modify,
  },
  {
    path: "/createAlbum",
    name: "createAlbum",
    component: createAlbum,
  },
  {
    path: "/uploadSong",
    name: "UploadSong",
    component: UploadSong,
  },
  {
    path: "/check-song",
    name: "Check",
    component: Check,
  },
  {
    path: "/song-info",
    name: "song-info",
    component: SongInfoEdit,
  },
  {
    path: "/sign",
    name: "Sign",
    component: Sign,
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
    name: "usermanage", //?????????????????????
    component: UserManage, //?????????????????????
  },
  {
    path: "/mediaplayer/:songId/:songList",  
    //eg:  /mediaplayer/1cd134c4-c/1cd134c4-c,6adb0c3a-c
    name: "mediaplayer",
    component: mediaplayer,
  },
  {
    //path: "/SingerDetail/",//调试用
    path: "/SingerDetail/:artistId",
    name: "SingerDetail",
    component: SingerDetail,
  },

  {
    path: "/FollowedArtist",
    name: "FollowedArtist",
    component: FollowedArtist,
  },
  {
    path: "/searchResultPage", // ???????��??????
    name: "search-result-page",
    component: SearchResultPage,
  },
  {
    path: "/ForgottenPassword",
    name: "ForgottenPassword",
    component: ForgottenPassword,
  },
  {
    //path: "/AlbumDetail/:albumId",
    path: "/AlbumDetail",//调试用
    name: "AlbumDetail",
    component: AlbumDetail,
  }
];

const router = createRouter({
  history: createWebHistory(), // ??????? base URL
  routes,
});

export default router;
