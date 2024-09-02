<template>
  <div>
      <TheHeader />
      <div class="page-container">
      <div class="profile">
        <div class="header">
          <img class="album-cover" :src="albumCover" alt="Album Cover" />
          <div class="album-info">
            <h1>{{ album.albumName }}</h1>
            <p style="color:darkgray;font-size: small;">
              发行日期：{{ formattedReleaseDate }} <br />
              发行公司：{{ album.albumCompany }} <br />
              制作人：{{ album.albumProducer }}
            </p>
            <div class="stats">
              <span>歌曲 {{ songCount }}</span>
            </div>
            <button class="play-button" @click="playFirstSong">▷播放专辑</button>
            <button 
            class="like-button" 
            :class="{ liked: isLiked }" 
            @click="toggleLike">
            {{ isLiked ? '已收藏' : '♡ 收藏' }} 
          </button>
          </div>
        </div>
  
        <div class="songs">
          <h2 style="margin-top: 10px; margin-bottom: 5px; margin-left: 10px; color:#284da0c1;">专辑歌曲</h2>
          <div class="songs-table">
            <table>
              <thead>
                <tr>
                  <th>歌曲</th>
                  <th>歌手</th>
                  <th>时长</th>
                  <!-- 添加按钮 -->
                  <th>操作</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(song, index) in album.songs" :key="index" @click="playSong(song.songId)">

                  <td>
                  <!-- 播放按钮 -->
                  <el-tooltip content="播放歌曲" placement="bottom">
                    <img :src="song.playing ? playClickedIcon : song.playHover ? playHoverIcon : playIcon"
                        @mouseover="song.playHover = true"
                        @mouseleave="song.playHover = false"
                        @click.stop="togglePlayIcon(song)"      
                        
                        class="play-icon"
                        alt="播放歌曲" />
                  </el-tooltip>
                  {{ index + 1 }}. {{ song.songName }}
                </td>
               <!-- togglePlayIcon(song)换成playSong(song.songId) -->

                  <td>{{ song.artistName }}</td>
                  <td>{{ formatDuration(song.duration) }}</td>

                  <!-- 添加收藏按钮 -->
                  <td>
                    <el-tooltip content="收藏歌曲" placement="bottom">
                      <img :src="song.liked ? likeClickedIcon : song.likeHover ? likeHoverIcon : likeIcon"
                           @mouseover="song.likeHover = true"
                           @mouseleave="song.likeHover = false"
                           @click="toggleLikeIcon(song)"
                           class="like-icon"
                           alt="收藏歌曲" />
                    </el-tooltip>
                  </td>
                  <!-- 加入专辑按钮 -->
                  <!--<td>
                     <button class="add-to-songlist-button" @click.stop="showSonglistModal(song)">
                      ➕ 
                     </button>
                  </td>-->
                  
    <td>
      <el-tooltip content="添加到歌单" placement="bottom">
        <img 
          :src="song.added ? addClickedIcon : song.addHover ? addHoverIcon : addIcon"
          @mouseover="song.addHover = true"
          @mouseleave="song.addHover = false"
          @click="toggleDialog(song.id)"  <!-- 修改此处，点击按钮打开弹窗 -->
          class="icon"
          alt="添加到歌单" 
        />
      </el-tooltip>
    </td>

    <!-- 引用弹窗组件 -->
    <el-dialog v-model="dialogVisible" width="500px" v-if="dialogVisible">
      <AddToSongList 
        :songId="currentSongId" 
        :dialogVisible="dialogVisible" 
        @update:dialogVisible="handleDialogClose" 
      />
    </el-dialog>
             </tr>
           </tbody>
         </table>
       </div>
     </div>
   </div>
 </div>
 
      <TheFooter  />
 </div>
</template>
  
  <script>
  //import albumCover from '../assets/logo2.jpg';
  import TheFooter from "../components/TheFooter.vue";
  import TheHeader from '../components/TheHeader.vue';
  import api from '../api/http.js';
  import playIcon from '../assets/pics/play.png'; // 添加按钮图片路径↓
  import playClickedIcon from '../assets/pics/play-click.png'; 
  import playHoverIcon from '../assets/pics/play-cover.png'; 
  import likeIcon from '../assets/pics/like.png'; 
  import likeHoverIcon from '../assets/pics/like-cover.png'; 
  import likeClickedIcon from '../assets/pics/like-click.png'; 
  import addIcon from '../assets/pics/add.png'; 
  import addHoverIcon from '../assets/pics/add-cover.png'; 
  import addClickedIcon from '../assets/pics/add-click.png'; // 添加↑
  import AddToSongList from '../components/AddToSongList.vue';
  

  export default {
    data() {
      return {
        albumCover: '',
        userId: '',
        albumId: '',
        album: {
          albumName: '',
          albumReleasedate: '',
          albumCompany: '',
          albumProducer: '',
          
          songs: [
          ///*
          {
            "songId": "0c35751f-0",
            "songName": "缓缓",
            "duration": 236,
            "songDate": null,
            "songGenre": null
        },
        {
            "songId": "1b5ed95e-d",
            "songName": "天若有情",
            "duration": 205,
            "songDate": null,
            "songGenre": null
        },
        {
            "songId": "1b5ed95e-g",
            "songName": "天情",
            "duration": 215,
            "songDate": null,
            "songGenre": null
        },
        //*/
        ],
        isLiked: false,
        dialogAddVisible: false, // 弹窗状态
        currentSong: null, // 当前选择的歌曲
        songlists: [], // 用户的歌单列表
        },
         // 添加
        playIcon,
        playClickedIcon,
        playHoverIcon,
        likeIcon,
        likeHoverIcon,
        likeClickedIcon,
        addIcon,
        addHoverIcon,
        addClickedIcon,
      };
    },
   
    computed: {
      //歌曲计数
      songCount() {
        return this.album.songs.length;
      },
      //把发行日期转化为规范格式
      formattedReleaseDate() {
        const releaseDate = new Date(this.album.albumReleasedate);
        return releaseDate.toLocaleDateString();
      }
    },
    components: {
      AddToSongList, 
      TheHeader,
      TheFooter,
    },
    methods: {
      async fetchAlbumData() {
        try {
          const albumResponse = await api.apiClient.get(`/api/album/${this.albumId}`);
          this.album = albumResponse.data;
          await this.fetchAlbumCover();//获取专辑封面
          await this.fetchArtistName();//获取歌手名字
        } catch (error) {
          console.error('获取专辑信息失败:', error);
        }
      },

      async fetchAlbumCover() {
      try {
        const coverResponse = await api.apiClient.get(`/api/player/jpg`, {
          params: {
            albumId: this.albumId,
          },
          responseType: 'blob', // 表明服务器返回的数据是二进制流
        });

        const imageUrl = URL.createObjectURL(coverResponse.data);
        this.albumCover = imageUrl;
      } catch (error) {
        console.error('获取专辑封面失败:', error);
        this.albumCover = '../assets/logo2.jpg'; // 如果失败，使用默认封面
      }
    },
      async fetchArtistName() {
        try {
          const artistResponse = await api.apiClient.get(`/api/artist/${this.album.artistId}`);
          const artistName = artistResponse.data.artistName;
          this.album.songs.forEach(song => {
            song.artistName = artistName;
          });
        } catch (error) {
          console.error('获取歌手信息失败:', error);
        }
      },

      //点击播放专辑按钮，自动播放第一首歌曲
      playFirstSong() {
    try {
      // 获取第一首歌的 songId
      const firstSong = this.album.songs[0];
      const firstSongId = firstSong.songId;

      // 生成 songList 参数，格式为 'songId1,songId2,...'
      const songList = this.album.songs.map(s => s.songId).join(',');

      // 跳转到播放页面，并传递 songId 和 songList 参数
      this.$router.push({
        name: 'mediaplayer',
        params: {
          songId: firstSongId, // 第一首歌的 songId
          songList: songList
        }
      });

    } catch (error) {
      console.error('播放第一首歌曲失败:', error);
    }
  },
      //用户点击歌单任意歌曲，通过songId切换到播放页面
      /*playSong(songId) {
        this.$router.push({ name: 'PlayerPage', params: { songId: songId } });
      },
      */
      //收藏方法
      async toggleLikeIcon(song) {
        try {
          if (song.liked) {
            // 如果已收藏，发送请求删除收藏
            await api.apiClient.delete(`/api/users/remove`, {
              data: {
              userId: this.userId,
              songId: song.songId
              }
            });
            song.liked = false;
          } else {
            // 如果未收藏，发送请求添加收藏
            await api.apiClient.post(`/api/users/add`, {
              userId: this.userId,
              songId: song.songId
            });
            song.liked = true;
          }
        } catch (error) {
          console.error('切换收藏状态失败,请重试', error);
          song.liked = !song.liked; // 切换收藏状态失败，恢复到之前的状态
        }
      },
     /* //加入歌单
      async toggleAddIcon(song) {
    try {
      if (song.added) {
        // 已经添加过，发送请求删除
        await api.apiClient.delete(`/api/users/removefromplaylist`, {
          data: {
            userId: this.userId,
            songId: song.songId
          }
        });
        song.added = false;
      } else {
        // 未添加过，发送请求添加
        await api.apiClient.post(`/api/users/addtoplaylist`, {
          userId: this.userId,
          songId: song.songId
        });
        song.added = true;
      }
    } catch (error) {
      console.error('切换添加状态失败,请重试', error);
      song.added = !song.added; // 切换添加状态失败，恢复到之前的状态
    }
  },*/
     
  // 在专辑列表内播放，暂停，跳转音乐的方法（目前暂未实现列表内播放，但前端仍可保留），涉及歌曲URL
      togglePlayIcon(song) {
    try {
      // 获取当前歌曲的 ID
      const songId = song.songId;

      // 生成 songList 参数，格式为 'songId1,songId2,...'
      const songList = this.album.songs.map(s => s.songId).join(',');

      // 跳转到播放页面，并传递 songId 和 songList 参数
      this.$router.push({ 
        name: 'mediaplayer', 
        params: { 
          songId: songId, 
          songList: songList 
        } 
      });

    } catch (error) {
      console.error('跳转播放页面失败:', error);
    }
  },
      toggleAddIcon(song) {
          console.log(song);
          this.currentSongId=song.songId;
          this.dialogAddVisible=true;
      },
      handleDialogClose(isVisible) {
          this.dialogAddVisible = isVisible;
      },

      // 将毫秒转换为分钟和秒
      formatDuration(duration) {
        const minutes = Math.floor(duration / 60);
        const seconds = duration % 60;
        return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
      },
    // 收藏与取消收藏专辑的方法
    async toggleLike() {
      try {
        if (this.isLiked) {
          await api.apiClient.delete(`/api/users/removealbum`, {
            data: {
          userId: this.userId,
          albumId: this.albumId
        }
      });
          this.isLiked = false;//取消收藏
        } else {
          await api.apiClient.post(`/api/users/addalbum`, {
            userId: this.userId,
            albumId: this.albumId
          });
          this.isLiked = true;//收藏
        }
        this.$forceUpdate(); // 强制更新视图
      } catch (error) {
        console.error('切换收藏状态失败:', error);
        this.isLiked = !this.isLiked;
        this.$forceUpdate(); // 强制更新视图
      }
    },
  },

  //以下为加入专辑歌单（包括创建新专辑）

    // 显示弹窗和歌单列表
    async showSonglistModal(song) {
    this.currentSong = song; // 存储当前正在添加的歌曲
    await this.fetchUserSonglists(); // 从后端获取用户歌单
    this.isSonglistModalVisible = true;
  },

  // 从后端获取用户歌单
  async fetchUserSonglists() {
    try {
      const response = await api.apiClient.get(`/api/songlist/user/{userId}`);
      this.songlists = response.data;
    } catch (error) {
      console.error('获取用户歌单失败:', error);
    }
  },

  // 将歌曲添加到选中的歌单
  /*async addToSonglist(song, songlistId) {
    try {
      await api.apiClient.post(`/api/songlists/add`, {
        userId: this.userId,
        songlistId,
        songId: song.songId,
      });
      this.isSonglistModalVisible = false; // 添加后隐藏弹窗
    } catch (error) {
      console.error('添加歌曲到歌单失败:', error);
    }
  },

  // 创建新歌单
  async createNewSonglist() {
    try {
      const newSonglist = await api.apiClient.post(`/api/songlist/add`, {
        userId: this.userId,
        songlistName: '新歌单', // 如果需要，可以替换为动态名称
      });
      this.songlists.push(newSonglist.data); // 将新歌单添加到列表
    } catch (error) {
      console.error('创建歌单失败:', error);
    }
  },*/
    async created() {
      this.albumId = this.$route.params.albumId;
      if (this.albumId) {
        await this.fetchAlbumData();
      } else {
        console.error('未找到 albumId');
      }
      this.userId = '001'; // this.$route.params.userId
    }
  };
  </script>
  
  <style scoped>
  .page-container {
    background: linear-gradient(to top, #f0f0f5, #d8dff8);
    min-height: 94vh;
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 20px;
  }
  
  .profile {
    width: 90%;
    max-width: 1200px;
    max-height:680px;
    margin: 20px auto;
    background-color: #fff;
    border: 1px solid #ddd;
    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
  }
  
  .header {
    display: flex;
    align-items: center;
    padding: 20px;
  }
  /*
  .album-cover {
  width: 200px;
  height: 200px;
  margin-right: 20px;
  border-radius: 10px;
  object-fit: cover;
}
  */
  .album-cover {
    width: 150px;
    height: 150px;
    border-radius: 8px;
    margin-right: 30px;
    object-fit: cover;
  }
  
  .album-info {
    flex: 1;
  }
  
  .stats {
    display: flex;
    gap: 10px;
    margin-top: 10px;
  }
  
  .play-button, .like-button {
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

.like-button {
  background-color: #fff;
  color: #284da0c1;
  border: 1px solid #284da0c1;
  border-radius: 8px;
  margin-left: 2px;
}

.like-button.liked {
  border: 1px solid darkgray;
  background-color: darkgray;
  color: white;
}
  
  .songs {
    margin-top: 20px;
    padding: 20px;
    border-top: 1px solid #ddd;

  }
  
  .songs-table {
    max-height: 320px;
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
  .play-icon {
  width: 34px; /* 设置按钮的宽度 */
}
  .like-icon {
  width: 34px; /* 设置按钮的宽度 */
}
.add-to-songlist-button {
  padding: 5px 10px;
  border: 1px solid #284da0c1;
  background-color: white;
  color: #284da0c1;
  cursor: pointer;
  border-radius: 4px;
}

.songlist-item button {
  width: 100%;
  padding: 8px;
  margin: 5px 0;
  background-color: #f5f5f5;
  border: 1px solid #ddd;
  cursor: pointer;
  text-align: left;
}

.create-new-songlist-button {
  padding: 8px 12px;
  background-color: #284da0c1;
  color: white;
  border: none;
  cursor: pointer;
  margin-top: 10px;
}
.icon {
  width: 24px; /* 设置图标宽度 */
  height: 24px; /* 设置图标高度 */
  cursor: pointer; /* 鼠标悬停时显示手型 */
}
 
  </style>
  