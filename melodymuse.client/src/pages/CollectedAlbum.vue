<template>
    <div>
      <TheHeader />
      <div class="header">
        <p class="header-text">我的收藏</p>
        <div class="buttons">
          <button 
            @click="navigateTo('CollectedSong')"
            :class="{'active': currentTab === 'song'}">
            歌曲
          </button>
          <button 
            @click="navigateTo('CollectedAlbum')"
            :class="{'active': currentTab === 'album'}">
            专辑
          </button>
        </div>
      </div>
      <div class="collected-albums-list">
        <table>
          <thead>
            <tr>
              <th class="album-column">专辑</th>
              <th class="artist-column">歌手</th>
              <th class="company-column">公司</th>
              <th class="date-column">发行日期</th>
              <th class="action-column">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(album, index) in collectedAlbums" :key="index">
              <!-- 点击专辑名称跳转到 AlbumDetail 页面，并应用样式 -->
              <td>
                {{ index + 1 }}. 
                <router-link :to="{ name: 'AlbumDetail', params: { albumId: album.albumId } }" class="album-link">
                  {{ album.albumName }}
                </router-link>
              </td>
              <td>{{ album.artistName }}</td>
              <td>{{ album.albumCompany }}</td>
              <td>{{ formatDate(album.albumReleasedate) }}</td>
              <td>
                <el-tooltip content="取消收藏" placement="bottom">
                  <img :src="album.liked ? likeClickedIcon : likeIcon"
                       @click="toggleLikeIcon(album)"
                       class="like-icon"
                       alt="取消收藏" />
                </el-tooltip>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </template>
  
  
  <script>
  import TheHeader from "../components/TheHeader.vue";
  //import axios from 'axios';
  import api from '../api/http.js';
  import likeIcon from '../assets/pics/like.png'; 
  import likeClickedIcon from '../assets/pics/like-click.png'; 
  
  export default {
    name: "CollectedAlbum",
    components: {
      TheHeader,
      TheFooter,
    },
    data() {
      return {
        collectedAlbums: [
       
        ],
        likeIcon,
        likeClickedIcon,
        currentTab: 'album', // 当前页面为专辑
      };
    },
    methods: {
    // 页面跳转
    navigateTo(page) {
      if (page === 'CollectedSong') {
        this.currentTab = 'song';
        this.$router.push({ name: 'CollectedSong' });
      } else {
        this.currentTab = 'album';
      }
    },
      // 获取收藏的专辑信息
      async fetchCollectedAlbums() {
        try {
         const response = await api.apiClient.get(`/api/users/user/${this.userId}/albums`);
          //const response = await axios.get(`https://localhost:7223/api/users/user/${this.userId}/albums`);
          const albums = response.data.map(album => ({ ...album, liked: true }));
  
          // 获取歌手的名字并更新数据
          for (let album of albums) {
            if (album.artistId) {
              const artistResponse = await api.apiClient.get(`/api/artist/${album.artistId}`);
             //const artistResponse = await axios.get(`https://localhost:7223/api/artists/${album.artistId}`);
              album.artistName = artistResponse.data.artistName;//动态添加属性
            }
          }
  
          this.collectedAlbums = albums;
          console.log('收藏的专辑:', this.collectedAlbums);
        } catch (error) {
          console.error('获取收藏的专辑信息失败:', error);
        }
      },
      
      // 切换收藏状态
      async toggleLikeIcon(album) {
        album.liked = !album.liked;
        try {
          await api.apiClient.delete(`/api/users/removeAlbum`, {
          //await axios.delete(`https://localhost:7223/api/users/removeAlbum`, {
            data: {
              userId: this.userId,
              albumId: album.albumId
            }
          });
          await this.fetchCollectedAlbums();
        } catch (error) {
          console.error('取消收藏失败,请重试', error);
        }
      },
  
      // 格式化日期
      formatDate(date) {
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        return new Date(date).toLocaleDateString('zh-CN', options);
      }
    },
    mounted() {
      this.userId = '001'; // localStorage.getItem('userId');
      this.fetchCollectedAlbums();
    },
  };
  </script>
  
  <style scoped>
  .header {
    height: 220px;
    background-color: #284da0c1;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    margin-bottom: 20px;
  }
  
  .header-text {
    color: aliceblue;
    font-size: 30px;
    margin-top: 45px;
    text-align: center;
  }
  
  .buttons {
    margin-top: 10px;
  }
  
  .buttons button {
    padding: 10px 20px;
    margin: 10px 10px;
    border: none;
    border-radius: 20px;
    background-color: transparent;
    color: white;
    cursor: pointer;
  }
  
  .buttons button.active {
    background-color: white;
    color: black;
  }
  
  .collected-albums-list {
    margin-top: 20px;
    padding-right: 5px;
    max-height: 400px;
    overflow-y: auto;
  }
  
  .collected-albums-list::-webkit-scrollbar {
    width: 8px;
  }
  
  .collected-albums-list::-webkit-scrollbar-thumb {
    background-color: #c4c4c4;
    border-radius: 10px;
  }
  
  .collected-albums-list::-webkit-scrollbar-thumb:hover {
    background-color: #999;
  }
  
  .collected-albums-list::-webkit-scrollbar-track {
    background-color: #f0f0f0;
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
  
  .album-column {
    width: 36%; 
  }
  
  .artist-column {
    width: 24%;  
  }
  
  .company-column, .date-column, .action-column {
    width: 13%; 
  }
  
  .like-icon {
    width: 34px;
  }
  .album-link {
  color: #284da0c1; /* 蓝色文字 */
  background-color: transparent; /* 透明背景 */
}

.album-link:hover {
    text-decoration: underline; /* 下划线 */
    text-decoration-color: #284da0c1; /* 蓝色下划线 */
}
  </style>
  