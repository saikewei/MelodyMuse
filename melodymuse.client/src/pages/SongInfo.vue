<template>
    <div>
        <h2>歌曲管理</h2>
        <el-dialog 
        v-model="dialogFromVisible" 
        title="Shipping address" 
        width="1500"
        v-if="dialogFromVisible">
            <EditForm :song_id="currentSong" @cancelEvent="dialogFromVisible=false" @submitEvent="refreshSongs"/>
        </el-dialog>
        <el-table
    :data="songs"
    :default-sort="{ prop: 'songName', order: 'ascending' }"
    style="width: 80%"
  >
    <el-table-column prop="index" label="序号" type="index" :index="indexMethod" sortable width="180" />
    <el-table-column prop="songName" label="歌名" sortable width="180" />
    <el-table-column prop="singerName" label="歌手" sortable width="100" />
    <el-table-column prop="composerName" label="作词家" sortable width="100" />
    <el-table-column prop="songDate" label="发行日期" width="130" :formatter="dateFormatter"/>
    <el-table-column prop="songGenre" label="流派" width="130"/>
    <el-table-column prop="duration" label="时长" width="180"/>
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
          >更改封面</el-button>
        </template>
    </el-table-column>
  </el-table> 
    </div>
</template>
<script>
    import { ElTable, ElTableColumn, ElButton} from "element-plus"
    import EditForm from "../components/SongInfoEditForm.vue"
    import axios from "axios";
    import { format } from 'date-fns';

    export default {
        data() {
      return {
        dialogFromVisible: false,
        currentSong: null,
        songs:null
      };
    },
    components: {
      EditForm,
      ElTable,
      ElTableColumn,
      ElButton
    },
    mounted() {
    },
    created() {
    this.fetchSongs();
  },
    methods: {
    indexMethod(index){
        return index+1;
    },
    handleEdit(row){
        this.dialogFromVisible=true;
        this.currentSong=row.songId;
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
          this.dialogFromVisible=false;
        } catch (error) {
          console.error('刷新歌曲数据失败', error);
        }
      },
    dateFormatter(row){
      console.log(row);
      return format(new Date(row.songDate), 'yyyy-MM-dd');
    }
    }
};
</script>