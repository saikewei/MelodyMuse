<template>
    <div>
        <h2>歌曲管理</h2>
        <el-dialog 
        v-model="dialogFromVisible" 
        title="Shipping address" 
        width="1500"
        v-if="dialogFromVisible">
            <EditForm :song_name="currentSong" @cancelEvent="dialogFromVisible=false"/>
        </el-dialog>
        <el-table
    :data="tableData"
    :default-sort="{ prop: 'song_name', order: 'ascending' }"
    style="width: 80%"
  >
    <el-table-column prop="index" label="序号" type="index" :index="indexMethod" sortable width="180" />
    <el-table-column prop="song_name" label="歌名" sortable width="180" />
    <el-table-column prop="singer_name" label="歌手" sortable width="100" />
    <el-table-column prop="composer_name" label="作词家" sortable width="100" />
    <el-table-column prop="publish_date" label="发行日期" sortable width="130"/>
    <el-table-column prop="genre" label="流派" width="130"/>
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
    import { ElTable, ElTableColumn, ElRow, ElCol, ElButton, ElIcon } from "element-plus"
    import EditForm from "../components/SongInfoEditForm.vue"

    export default {
        data() {
      return {
        tableData: [
          { song_name: 'ac', singer_name: 'John Doe', composer_name: 'John Doe', publish_date: '2024-07-18', genre:'rock', duration:'11:1'},
          { song_name: 'abc', singer_name: 'Jane Smith', composer_name: 'John Doe', publish_date: '2024-07-18' ,genre:'rock', duration:'11:1'},
          { song_name: 'abc', singer_name: 'Sam Brown', composer_name: 'John Doe', publish_date: '2024-07-17' ,genre:'rock', duration:'11:1'},
        ],
        dialogFromVisible: false,
        currentSong: null
      };
    },
    components: {
      EditForm,
      ElTable,
      ElTableColumn
    },
    mounted() {
    },
    methods: {
    indexMethod(index){
        return index+1;
    },
    handleEdit(row){
        this.dialogFromVisible=true;
        this.currentSong=row.song_name;
        console.log(this.currentSong);
    },
    }
};
</script>