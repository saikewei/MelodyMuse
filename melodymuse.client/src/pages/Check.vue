<template>
    <div>
      <el-card>
        <div slot="header" class="clearfix">
          <span></span>
        </div>
        <div style="margin-bottom: 20px; width: 500px; display: flex; align-items: center;">
          <el-select v-model="selectedTimeRange" placeholder="选择时间段">
            <el-option label="全部" value="all"></el-option>
            <el-option label="一天内" value="1d"></el-option>
            <el-option label="两天内" value="2d"></el-option>
            <el-option label="三天内" value="3d"></el-option>
            <el-option label="一周内" value="1w"></el-option>
            <el-option label="一个月内" value="1m"></el-option>
          </el-select>
          <el-input v-model="searchKeyword" placeholder="输入关键词" style="margin-left: 10px; width: 600px; "></el-input>
          <el-button type="primary" @click="handleSearch" style="margin-left: 10px;">查询</el-button>
        </div>
        <el-table :data="musicList" style="width: 100%">
          <el-table-column type="index" label="序号"></el-table-column>
          <el-table-column prop="title" label="歌名"></el-table-column>
          <el-table-column prop="singer" label="歌手"></el-table-column>
          <el-table-column label="歌词" width="300">
            <template slot-scope="scope">
              <div style="max-height: 100px; overflow-y: auto;">
                {{ scope.row.lyrics }}
              </div>
            </template>
          </el-table-column>
          <el-table-column label="操作" width="160">
            <template slot-scope="scope">
              <el-button type="success" size="small" @click="approveMusic(scope.row)">通过</el-button>
              <el-button type="danger" size="small" @click="rejectMusic(scope.row)">拒绝</el-button>
            </template>
          </el-table-column>
          <el-table-column prop="reviewComment" label="审核意见" width="180">
            <template slot-scope="scope">
              <el-input type="textarea" v-model="scope.row.reviewComment" placeholder="请输入"></el-input>
            </template>
          </el-table-column>
          <el-table-column prop="uploadTime" label="上传时间" width="180"></el-table-column>
          <el-table-column prop="status" label="状态" width="120">
            <template slot-scope="scope">
              <el-tag :type="getStatusTagType(scope.row.status)">{{ scope.row.status }}</el-tag>
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </div>
  </template>
  
  <script>
import axios from 'axios'; // Import Axios or your preferred HTTP client library

export default {
  data() {
    return {
      selectedTimeRange: 'all',
      searchKeyword: '',
      musicList: []
    };
  },
  mounted() {
    // Fetch initial data from the backend upon component mounting
    this.fetchMusicList();
  },
  methods: {
    //获取音乐列表
    async fetchMusicList() {
      try {
        // Example endpoint, replace with your actual backend endpoint
        const response = await axios.get('/api/submit/uploadSong'); 
        // Assuming your backend returns an array of music objects
        this.musicList = response.data; // Update musicList with fetched data
      } catch (error) {
        console.error('Error fetching music list:', error);
      }
    },

    //搜索逻辑的实现
    async handleSearch() {
      try {
        const response = await axios.get('/api/music', {
          params: {
            keyword: this.searchKeyword,
            timeRange: this.selectedTimeRange
          }
        });
        this.musicList = response.data; 
      } catch (error) {
        console.error('Error searching music:', error);
      }
    },

    //审核通过
    async approveMusic(row) {
      try {
        await axios.put(`/api/music/${row.musicId}/approve`); 
        row.status = '已通过';
        console.log('通过:', row);
      } catch (error) {
        console.error('Error approving music:', error);
      }
    },

    //审核不通过
    async rejectMusic(row) {
      try {
        await axios.put(`/api/music/${row.musicId}/reject`); 
        row.status = '已拒绝';
        console.log('拒绝:', row);
      } catch (error) {
        console.error('Error rejecting music:', error);
      }
    },


    getStatusTagType(status) {
      if (status === '已通过') return 'success';
      if (status === '已拒绝') return 'danger';
      return 'info';
    }
  }
};
</script>

  <style scoped>
  .clearfix::after {
    content: "";
    display: table;
    clear: both;
  }
  </style>
  