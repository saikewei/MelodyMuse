<template>
  <div>
    <el-card>
      <div slot="header" class="clearfix">
        <span></span>

      </div>
      <div style=" margin-bottom: 20px; width: 500px; display: flex; align-items: center;">
        <el-select  v-model="selectedTimeRange" placeholder="选择时间段">
          <el-option label="全部" value="all"></el-option>
          <el-option label="一天内" value="1d"></el-option>
          <el-option label="两天内" value="2d"></el-option>
          <el-option label="三天内" value="3d"></el-option>
          <el-option label="一周内" value="1w"></el-option>
          <el-option label="一个月内" value="1m"></el-option>
        </el-select>
        <el-input  v-model="searchKeyword" placeholder="输入关键词" style="margin-left: 10px; width: 600px; "></el-input>
        <el-button class="searchbutton" type="primary" @click="handleSearch" style="margin-left: 10px;">查询</el-button>
      </div>

      <el-table :data="musicList" style="width: 100%; height: 180px;"
    :header-cell-style="{ textAlign: 'center' }"
    :cell-style="{ textAlign: 'center' }">
    <el-table-column prop="songId" label="歌曲ID"></el-table-column>
    <el-table-column prop="songName" label="歌名"></el-table-column>
    <el-table-column prop="composerId" label="歌手ID"></el-table-column>
    <el-table-column prop="lyrics" label="歌词"></el-table-column>

    <el-table-column label="操作" width="160">
        <template #default="scope">
            <el-button type="success" size="small" @click="approveMusic(scope.row)">通过</el-button>
            <el-button type="danger" size="small" @click="rejectMusic(scope.row)">拒绝</el-button>
        </template>
    </el-table-column>

    <el-table-column label="审核意见" width="180">
        <el-input type="textarea" placeholder="请输入"></el-input>
    </el-table-column>
    <el-table-column prop="songDate" label="上传时间" width="180"></el-table-column>
</el-table>

    </el-card>
  </div>
</template>

<script>
import axios from 'axios'; // Import Axios or your preferred HTTP client library

export default {
data() {
  return {
    selectedTimeRange: 'all', // 默认选择全部时间段
    searchKeyword: '', // 搜索关键词
    musicList: [
 
    ] // 待审核音乐列表
  };
},
mounted() {
  //
  console.log(this.musicList); // 检查musicList是否有数据
  this.fetchMusicList();
},
methods: {
  //获取音乐列表
  async fetchMusicList() {
    try {
      const response = await axios.get('https://localhost:7223/api/songs/pending'); 
      console.log(response.data); // 打印后端返回的数据
      this.musicList = response.data; // Update musicList with fetched data
    } catch (error) {
      console.error('获取音乐列表错误:', error);
    }
  },

  //搜索逻辑的实现
  async handleSearch() {
    try {
      const response = await axios.get('https://localhost:7223/api/songs/pending', {
        params: {
          keyword: this.searchKeyword,
          timeRange: this.selectedTimeRange
        }
      });
      this.musicList = response.data; 
    } catch (error) {
      console.error('搜索时发生错误:', error);
    }
  },
  async approveMusic(song) {
  console.log('Approving song:', song); // 确认song对象内容
  if (!song.songId) {
    console.error('歌曲Id尚未定义');
    return;
  }

  try {
    const response = await axios.post(`https://localhost:7223/api/songs/${song.songId}/approve`);
    console.log('Approval response:', response.data); // 打印服务器返回的响应
    song.Status = 1; // 修改状态为通过
    await this.fetchMusicList(); // 重新获取列表
  } catch (error) {
    console.error('通过歌曲错误:', error);
  }
},

async rejectMusic(song) {
  console.log('Rejecting song:', song); // 确认song对象内容
  if (!song.songId) {
    console.error('歌曲Id尚未定义');
    return;
  }

  try {
    const response = await axios.post(`https://localhost:7223/api/songs/${song.songId}/reject`);
    console.log('Rejection response:', response.data); // 打印服务器返回的响应
    song.Status = 2; // 修改状态为拒绝
    await this.fetchMusicList(); // 重新获取列表
  } catch (error) {
    console.error('拒绝歌曲错误:', error);
  }
}
,


  getStatusTagType(status) {
    switch (status) {
    case 0:
      return 'info'; // Pending status
    case 1:
      return 'success'; // Approved status
    case 2:
      return 'danger'; // Rejected status
    default:
      return 'info';
    }
  }
}
};
</script>

<style scoped>
.clearfix::after {
  content: "";
  display: table;
  clear: both;
  width:1200px;
};
</style>
