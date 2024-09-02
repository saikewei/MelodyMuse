<template>
  <!-- 表单，用于选择歌单 -->
  <el-form ref="formRef">
    添加歌曲到歌单
    <el-form-item>
      <el-table
        :data="songlists"
        border
        @selection-change="handleSelectionChange"
        style="width: 100%; margin-top: 10px;"
      >
        <el-table-column type="selection" width="55"></el-table-column>
        <el-table-column
          prop="songlistName"
          label="歌单名称"
        >
        </el-table-column>
      </el-table>
    </el-form-item>
  </el-form>
  <!-- 对话框底部的操作按钮 -->
  <span slot="footer" class="dialog-footer">
    <el-button type="primary" @click="addToPlaylists" :disabled="isAddButtonDisabled">添加</el-button>
    <el-button @click="handleCancel">取消</el-button>
  </span>
</template>

<script lang="ts" setup>
import { ref, onMounted, watch, defineEmits, computed } from 'vue'
import api from '../api/http.js'
import { ElMessage } from 'element-plus';

interface Playlist {
  songlistId: number;
  songlistName: string;
}

// 定义接收的 props
const props = defineProps<{
  songId: number; // 要添加的歌曲 ID
  dialogVisible: boolean; // 对话框的显示状态
}>();

// 定义存储歌单列表的变量
const songlists = ref<Playlist[]>([]);
// 定义存储用户选择的歌单的变量
const selectedPlaylists = ref<Playlist[]>([]);

const emit = defineEmits<{
  (event: 'update:dialogVisible', value: boolean): void;
}>();

// 异步函数，获取用户的歌单列表
const fetchPlaylists = async () => {
  try {
    const response = await api.apiClient.get('/api/songlist/getall');
    songlists.value = response.data;
  } catch (error) {
    console.error('Failed to fetch playlists:', error);
  }
};

// 组件挂载时调用，加载歌单列表
onMounted(() => {
  fetchPlaylists();
});

// 监听 dialogVisible 的变化，当对话框打开时重置 selectedPlaylists
watch(() => props.dialogVisible, (newVal) => {
  if (newVal) {
    selectedPlaylists.value = [];
  }
});

const isAddButtonDisabled = computed(() => {
  return selectedPlaylists.value.length === 0;
});

// 处理选择变化
const handleSelectionChange = (selection: Playlist[]) => {
  selectedPlaylists.value = selection;
};

// 异步函数，将歌曲添加到选中的歌单
const addToPlaylists = async () => {
  try {
    // 遍历用户选中的歌单，将歌曲添加到每个选中的歌单中
    for (const playlist of selectedPlaylists.value) {
      await api.apiClient.post(`/api/songlist/${playlist.songlistId}/songs/${props.songId}/add`);
    }
    ElMessage.success('歌曲已成功添加到所选歌单');
    handleCancel();
    // dialogVisible.value = false;
  } catch (error) {
    console.error('Failed to add song to playlists:', error);
    ElMessage.error('添加歌曲到歌单失败');
  }
  console.log(selectedPlaylists.value);
};

// 关闭对话框的处理函数
const handleCancel = () => {
  emit('update:dialogVisible', false);
};
</script>

<style scoped>
.dialog-footer {
  text-align: right;
}
</style>
