<template>
  <div>
    <TheHeader :currentPage="currentPage"  />
    <TheAside />
    <el-dialog 
      v-model="dialogFromVisible" 
      title="歌曲信息编辑" 
      width="1500"
      v-if="dialogFromVisible">
        <EditForm :song_id="currentSong" @cancelEvent="dialogFromVisible=false" @submitEvent="refreshSongs"/>
    </el-dialog>
    <el-input
    v-model="searchQuery"
    class="search-el-input"
    style="width: 240px"
    placeholder="输入要查找的歌名"
    clearable
  />
    <el-table
      :data="filteredSongs"
      :default-sort="{ prop: 'songName', order: 'ascending' }"
      class="song-el-table"
      :height="tableHeight"
    >
      <el-table-column fixed prop="index" label="序号" type="index" :index="indexMethod" sortable width="180" />
      <el-table-column prop="songName" label="歌名" sortable width="180" />
      <el-table-column prop="singerName" label="歌手" sortable width="100" />
      <el-table-column prop="composerName" label="作词家" sortable width="100" />
      <el-table-column prop="songDate" label="发行日期" width="130" :formatter="dateFormatter"/>
      <el-table-column prop="songGenre" label="流派" width="130"/>
      <el-table-column prop="duration" label="时长" width="250"/>
      <el-table-column width="100">
        <template v-slot="scope">
          <el-button
            type="primary"
            @click="handleEdit(scope.row)"
          >编辑信息</el-button>
        </template>
      </el-table-column>
    </el-table> 
  </div>
</template>

<script>
import { ElTable, ElTableColumn, ElButton } from "element-plus"
import EditForm from "../components/SongInfoEditForm.vue"
import axios from "axios";
import { format } from 'date-fns';
import TheHeader from "../components/SimpleHeader.vue";
import TheAside from "../components/TheAside.vue";

export default {
  data() {
    return {
      dialogFromVisible: false,
      dialogCoverVisible: false,
      currentSong: null,
      songs: null,
      tableHeight: 0,
      currentPage: "歌曲信息",
      searchQuery: '', // New data property for the search input
      filteredSongs: [], // New reactive property for filtered songs
    };
  },
  components: {
    EditForm,
    ElTable,
    ElTableColumn,
    ElButton,
    TheHeader,
    TheAside,
  },
  mounted() {
    this.updateTableHeight();
    window.addEventListener('resize', this.updateTableHeight);
  },
  beforeUnmount() {
    window.removeEventListener('resize', this.updateTableHeight);
  },
  created() {
    this.fetchSongs();
  },
  watch: {
  searchQuery: {
    immediate: true,
    handler(newQuery) {
      this.filterSongsAsync(newQuery);
    }
  }
},
  methods: {
    indexMethod(index) {
      return index + 1;
    },
    handleEdit(row) {
      this.currentSong = row.songId;
      this.dialogFromVisible = true;
      console.log(this.currentSong);
    },
    handleCoverChange(row){
      this.currentSong = row.songId;
      this.dialogCoverVisible = true;
      console.log(this.currentSong);
    },
    async fetchSongs() {
      try {
        const response = await axios.get('https://localhost:7223/api/songedit');
        this.songs = response.data;
        this.filteredSongs = this.songs;
      } catch (error) {
        console.error('获取歌曲数据失败', error);
      }
    },
    async refreshSongs() {
      try {
        await this.fetchSongs();
        this.dialogFromVisible = false;
      } catch (error) {
        console.error('刷新歌曲数据失败', error);
      }
    },
    dateFormatter(row) {

      return format(new Date(row.songDate), 'yyyy-MM-dd');
    },
    async filterSongsAsync(query) {
      if (query === '') {
            // Perform the operation asynchronously
            return new Promise((resolve) => {
                setTimeout(() => {
                    this.filteredSongs = [...this.songs];
                    resolve();
                }, 0);
            });
      }

      return new Promise((resolve) => {
      setTimeout(() => {
        this.filteredSongs = this.songs.filter(song => {
          return song.songName.includes(query) ||
                 song.singerName.includes(query);
        });
        resolve();
      }, 0);
      });
  },
    updateTableHeight() {
      this.tableHeight = window.innerHeight - 200; // Adjust 100 to the desired offset
    },
    
  }
};
</script>

<style scoped>
.cover-container {
  display: flex;
  justify-content: space-between;
  margin-bottom: 20px;
}

.cover-image {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.cover-image img {
  width: 300px; /* Adjust width as needed */
  height: 300px; /* Adjust height as needed */
  margin-bottom: 10px;
}

.dialog-footer {
  display: flex;
  justify-content: flex-end;
}

.song-el-table{
  width: 85%;
  left: 150px;
}

.search-el-input{
  left: 150px;
}
</style>