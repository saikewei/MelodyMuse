<template>
    <div class="playlist-container">

      <!-- 编辑歌单按钮 -->
    <div class="header-section">
      <el-button type="primary" @click="openCreateDialog">创建歌单</el-button>
    </div>

    <!-- 分隔线 -->
    <div class="separator"></div>
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
                <el-button type="success" @click="playSongList(playlist)">播放</el-button>
                <el-button type="primary" @click="openEditDialog(playlist)">编辑</el-button>
                <el-button type="primary" @click="viewDetails(playlist)">详情</el-button>
                <el-button type="danger" @click="confirmDelete(playlist.songlistId)">删除</el-button>
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

    <el-dialog v-model="dialogVisible" width="700px">
        <div>歌单信息</div>
        <el-divider direction="horizontal"></el-divider> 
      <el-form :model="currentPlaylist" ref="formRef" label-width="90px" :rules="rules">
        <el-form-item label="歌单名称" prop="songlistName">
          <el-input v-model="currentPlaylist.songlistName" />
        </el-form-item>
        <el-form-item label="是否公开">
          <el-checkbox v-model="currentPlaylist.isPublic">公开</el-checkbox>
        </el-form-item>
        <el-divider direction="horizontal"></el-divider> 
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">取 消</el-button>
        <el-button type="primary" @click="savePlaylist">确 定</el-button>
      </span>
    </el-dialog>
    </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue';
import api from '../api/http.js';
import { useRouter } from 'vue-router';
import { ElMessageBox, ElMessage } from 'element-plus';

interface Playlist {
    songlistId: number
    songlistName: string
    description: string
}

const songlists = ref<Playlist[]>([]);
const loading = ref(true);
const dialogVisible = ref(false);
const currentPlaylist = ref<Partial<Playlist> & { isPublic?: boolean }>({});
const formRef = ref(null);
const router = useRouter();

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

const rules = {
  songlistName: [
    { required: true, message: '歌单名称不能为空', trigger: 'blur' }
  ]
}

onMounted(() => {
  fetchSongList();
})

const viewDetails = (playlist: Playlist) => {
  console.log(`Viewing details for playlist: ${playlist.songlistName}`)
  router.push(`/SonglistDetail/${playlist.songlistId}`);
}

const openCreateDialog = () => {
  currentPlaylist.value = {}; // 重置当前歌单
  dialogVisible.value = true;
};

const openEditDialog = (playlist: Playlist) => {
  currentPlaylist.value = { ...playlist }; // 填充当前歌单数据
  dialogVisible.value = true;
};

const playSongList = async (playlist: Playlist)=> {
  try{
    const songsResponse = await api.apiClient.get(`/api/songlist/${playlist.songlistId}/songs`);
    if(songsResponse.status < 400){
      console.log(songsResponse.data);
      // 生成 songList 参数，格式为 'songId1,songId2,...'
      const songList = songsResponse.data.map(s => s.songId).join(',');

      console.log(songList);

      // 跳转到播放页面，并传递 songId 和 songList 参数
      router.push(`/mediaplayer/${songsResponse.data[0].songId}/${songList}`);

    }
    else{
      ElMessage.error("无法播放歌单，歌单没有歌曲");
    }
  }catch(error){
    ElMessage.error("无法播放歌单:"+error);
  }
  
  //router.push(`/mediaplayer/${playlist.songlistId}`)
}


const confirmDelete = async (songlistId: number) => {
  try {
    await ElMessageBox.confirm(
      '你确定要删除这个歌单吗？', // 提示消息
      '确认删除', // 标题
      {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }
    ).then(async () => {
      await api.apiClient.delete(`/api/songlist/${songlistId}/delete`)
      fetchSongList() // 刷新歌单列表
      ElMessage.success('删除成功')
    }).catch(() => {
      ElMessage.info('删除操作已取消')
    })
  } catch (error) {
    console.error('Failed to delete playlist:', error)
  }
}


const savePlaylist = async () => {
  const form = formRef.value as any // 获取 el-form 实例

  form.validate(async (valid: boolean) => {
    if (valid) {
      try {
        const payload = {
          SonglistName: currentPlaylist.value.songlistName,
          IsPublic: currentPlaylist.value.isPublic?'1':'0'
        }
        console.log(payload)
        if (currentPlaylist.value.songlistId) {
          // 编辑模式
          await api.apiClient.put(`/api/songlist/${currentPlaylist.value.songlistId}/update`, payload);
          ElMessage.success('歌单编辑成功');
        } else {
          // 创建模式
          await api.apiClient.post('/api/songlist/add', payload);
          ElMessage.success('歌单创建成功');
        }
        fetchSongList() // 刷新歌单列表
        dialogVisible.value = false
      } catch (error) {
        console.error('Failed to save playlist:', error)
      }
    } else {
      ElMessage.error('请填写必填项')
    }
  })
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

.header-section {
  margin-bottom: 20px; /* 按钮与分隔线之间的间距 */
}

.separator {
  border-top: 1px solid #e4e4e4; /* 分隔线样式 */
  margin: 20px 0; /* 分隔线上下间距 */
}

</style>
