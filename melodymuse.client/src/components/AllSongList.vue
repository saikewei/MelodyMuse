<template>
    <div class="playlist-container">
      <el-row :gutter="20" v-if="songlists.length > 0">
        <el-col :span="8" v-for="playlist in songlists" :key="playlist.songlistId">
          <el-card class="playlist-card" shadow="hover">
            <div class="playlist-header">
              <span>{{ playlist.songlistName }}</span>
            </div>
            <div class="playlist-content">
              <p>{{ playlist.description }}</p>
            </div>
            <div class="playlist-footer">
                <el-button type="primary">顺序播放</el-button>
                <el-button type="primary" @click="viewDetails(playlist)">歌单详情</el-button>
            </div>
          </el-card>
        </el-col>
      </el-row>
      <div v-else-if="loading" class="loading-message">
      <p>加载歌单中...</p>
        </div>
    <div v-else class="loading-message">
      <p>未创建任何歌单</p>
    </div>
    </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import api from '../api/http.js'

interface Playlist {
    songlistId: number
    songlistName: string
    description: string
}

const songlists = ref<Playlist[]>([])
const loading = ref(true)

const fetchSongList = async () => {
  try {
    const response = await api.apiClient.get('/api/songlist/getall');
    songlists.value = response.data;
    console.log(response.data);
  } catch (error) {
    console.error('Failed to fetch songlists:', error)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchSongList();
})

const viewDetails = (playlist: Playlist) => {
console.log(`Viewing details for playlist: ${playlist.name}`)
// 你可以在这里实现查看歌单详细信息的逻辑
}
</script>

<style>
.playlist-container {
    margin-top: 30px;
    height: 700px; /* 你可以根据需要调整高度 */
    overflow-y: auto; /* 添加垂直滚动条 */
    padding: 20px;

}

.playlist-card {
margin-bottom: 20px;
}

.playlist-header {
font-size: 18px;
font-weight: bold;
}

.playlist-content {
margin-top: 10px;
}

.playlist-footer {
margin-top: 20px;
text-align: right;
}

.loading-message,
.no-playlists-message {
  text-align: center; /* 水平居中 */
  padding: 20px;
}
</style>
