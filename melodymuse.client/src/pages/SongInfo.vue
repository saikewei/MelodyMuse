<template>
  <div>
    <h2>歌曲管理</h2>
    <el-dialog 
      v-model="dialogFromVisible" 
      title="歌曲信息编辑" 
      width="1500"
      v-if="dialogFromVisible">
        <EditForm :song_id="currentSong" @cancelEvent="dialogFromVisible=false" @submitEvent="refreshSongs"/>
    </el-dialog>
    <el-dialog 
      v-model="dialogCoverVisible" 
      title="更换歌曲海报" 
      width="1500"
      v-if="dialogCoverVisible">
      <div class="cover-container">
        <div class="cover-image">
          <img :src="oldCoverUrl" alt="旧海报" />
          <p>旧海报</p>
        </div>
        <div class="cover-image">
          <img :src="newCoverUrl" alt="新海报" />
          <p>新海报</p>
        </div>
      </div>
      <div class="dialog-footer">
        <el-button @click="dialogCoverVisible = false">保存</el-button>
        <el-button @click="dialogCoverVisible = false">取消</el-button>
      </div>
    </el-dialog>
    <el-table
      :data="songs"
      :default-sort="{ prop: 'songName', order: 'ascending' }"
      style="width: 90%"
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
      <el-table-column>
        <template v-slot="scope">
          <el-button
            type="primary"
            @click="handleCoverChange(scope.row)"
          >更改封面</el-button>
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

export default {
  data() {
    return {
      dialogFromVisible: false,
      dialogCoverVisible: false,
      currentSong: null,
      songs: null,
      tableHeight: 0
    };
  },
  components: {
    EditForm,
    ElTable,
    ElTableColumn,
    ElButton
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
      console.log(row);
      return format(new Date(row.songDate), 'yyyy-MM-dd');
    },
    updateTableHeight() {
      this.tableHeight = window.innerHeight - 200; // Adjust 100 to the desired offset
    }
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
</style>