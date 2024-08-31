<template>
  <div class="page-container">
    <div class="profile">
      <TheHeader />
      <div class="header">
        <img class="profile-picture" :src="profilePicture" alt="Profile Picture" />
        <div class="profile-info">
          <h1>{{ userInfo.userName }}</h1>
          <p style="color:darkgray">
            出生于 {{ formattedBirthday }} <br />
          </p>
          <div class="stats">
            <span>单曲 {{ singleCount }}</span>
            <span>专辑 {{ albumCount }}</span>
          </div>
        </div>
      </div>
  
      <div class="songs">
        <h2 style=" margin-top: 10px;margin-bottom: 5px; margin-left: 10px;color:#284da0c1;" > 所有歌曲</h2>
        <div class="songs-table">
          <table>
            <thead>
              <tr>
                <th>歌曲</th>
                <th>专辑</th>
                <th>时长</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(song, index) in songs" :key="index" @click="playSong(song.songId)">
                <td>{{ index + 1 }}. {{ song.songName }}</td>
                <td>{{ song.albumName }}</td>
                <td>{{ formatDuration(song.duration) }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import profilePicture from '../assets/logo2.jpg';
import TheHeader from '@/components/TheHeader.vue';
import api from '../api/http.js'
import uploadSong from '../components/userNewSong.vue'
import {ElButton,ElMessage} from 'element-plus'

export default{
    components:{
        TheHeader,
        uploadSong,
        profilePicture,
        ElButton,
    },
    data(){
        return{
            uploadWindowVisibility:false,
            userInfo:{
              "userId":"",
              "userName":"",
              "userEmail":"",
              "userPhone":"",
              "userSex":"",
              "userAge":"",
              "userBirthday":"",
              "userStatus":"",
              "isArtist":false,
              "artistId":"",
            },
            followersCount:0,
            songs:[]
        };
    },
    computed:{
      singleCount(){
        return this.songs.length;
      },
      albumCount(){
        const albums = this.songs.map(song => song.albumName);
        return new Set(albums).size();
      },
      formattedBirthday(){
        const birthDay = new Date(this.userInfo.userBirthday);
        return birthDay.toLocaleDateString();
      }
    },
    methods:{
      async fetchUserInfo(){
        try{
          const response= await api.apiClient.get("api/");
          if(response.status_code != "200"){
            ElMessage({
              message:"获取用户信息失败，请稍后再试",
              type:"error"
            })
            this.userInfo = response.data;

            if(this.userInfo.isArtist){
              this.fetchSongs(this.userInfo.artistId);
            }
          }
        }
        catch(error){
          console.error("获取用户信息失败:"+error);
        }
      },

      async fetchSongs(artistId){
        try{
          const songsResponse = await api.apiClient.get(`/api/artist/${this.artistId}/songs`);
          if(songsResponse.status_code!="200"){
            ElMessage({
              message:"获取歌曲信息失败,请稍后再试",
              type:"error"
            })
          }
          else{
            this.songs = await Promise.all(
              songsResponse.data.map(async (song) => {
                const albumResponse = await api.apiClient.get(`/api/songs/${song.songId}/album`);
                return {...song,albumName:albumResponse.data};
              })
            )
          }
        }catch(error){
          console.error("歌曲信息获取失败:"+error);
        }
      },

      playSong(songId){
        
      },

      formatDuration(duration) {
      const minutes = Math.floor(duration / 60);
      const seconds = duration % 60;
      return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
      }
    },

    async created(){
      try{
        await this.fetchUserInfo();
      }catch(error){
        console.error("加载页面失败"+error)
      }
    }



}
</script>

<style scoped>

.page-container {
  background: radial-gradient(circle, #f0f0f5, #d8d8fb); /* 从中心向外的渐变 */
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
}

.profile {
  width: 90%;
  max-width: 1200px; /* 设置最大宽度 */
  margin: 20px auto;
  background-color: #fff;
  border: 1px solid #ddd;
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1); /* 增加阴影效果 */
}
  
  .header {
  display: flex;
  align-items: center;
  padding: 20px;
}
  
  .profile-picture {
    width: 150px;
    height: 150px;
    border-radius: 50%;
    margin-right: 20px;
  }
  
  .profile-info {
    flex: 1;
  }
  
  .stats {
    display: flex;
    gap: 10px;
    margin-top: 10px;
  }
  
  .play-button, .follow-button {
    margin-top: 20px;
    padding: 10px 20px;
    border: none;
    cursor: pointer;
  }
  
  .play-button {
    background-color: #284da0c1;
    color: white;
    border-radius: 8px;
    margin-right: 2px;
  }

  

  
  .songs {
  margin-top: 20px;
  padding: 20px;
  border-top: 1px solid #ddd;
}
  
  .songs-table {
    max-height: 400px;
    overflow-y: auto;
  }
  
  table {
    width: 100%;
    border-collapse: collapse;
  }
  
  thead {
    background-color: #f4f4f4;
  }
  
  th, td {
    padding: 10px;
    text-align: left;
    border-bottom: 1px solid #ddd;
  }
  </style>
  