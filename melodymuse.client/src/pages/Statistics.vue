<template>
    <div>
        <SimpleHeader/>
        <TheAside/>
          <el-table :data="tableData" style="width: 100%; left: 150px;">
            <el-table-column prop="genreType" label="统计类别" />
            <el-table-column prop="genreName" label="统计结果" />
          </el-table>
    </div>
  </template>
  
  <script>
  import { ElMessage } from 'element-plus';
  import api from '../api/http.js';
import SimpleHeader from '@/components/SimpleHeader.vue';
import TheAside from '@/components/TheAside.vue';
  
  export default {
    data() {
      return {
        tableData: [
          { genreType: '最喜欢的歌曲类型', genreName: '' },
          { genreType: '最喜欢的歌手类型', genreName: '' },
        ],
      };
    },
    components:{
        SimpleHeader,
        TheAside
    },  
    methods: {
      async fetchStatistics() {
        try {
          const response = await api.apiClient.get('/api/favorite-genres');
          this.tableData[0].genreName = response.data.favoriteSongGenre;
          this.tableData[1].genreName = response.data.favoriteArtistGenre;
          // console.log(response.data);
        } catch (error) {
          if (error.response && error.response.status === 404) {
            ElMessage.warning('暂无数据');
          } else {
            ElMessage.error('获取数据发生未知错误');
          }
        }
      },
    },
    mounted() {
        this.tableData[0].genreName='暂无数据';
        this.tableData[1].genreName='暂无数据';
        this.fetchStatistics();
    },
  };
  </script>
  
  <style scoped>
  </style>
  