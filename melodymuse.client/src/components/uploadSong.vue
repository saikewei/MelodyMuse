<template>
  <div class="SongUpload">
    <div class="main-container">
      <TheHeader />
      <div class="sidebar">
        <!-- 这里放列表栏的内容 -->
        <p>列表栏内容</p>
      </div>
      <div class="song-upload">
        <el-form ref="songForm" :model="songForm" :rules="rules" label-width="120px">
          <el-form-item label="歌曲名称" prop="name">
            <el-input v-model="songForm.name"></el-input>
          </el-form-item>
          <el-form-item label="时长" prop="duration">
            <el-input type="number" v-model="songForm.duration"></el-input>
          </el-form-item>
          <el-form-item label="曲风" prop="genre">
            <el-input v-model="songForm.genre"></el-input>
          </el-form-item>
          <el-form-item label="歌词" prop="lyrics">
            <el-input type="textarea" v-model="songForm.lyrics"></el-input>
          </el-form-item>
          <el-form-item label="专辑" prop="album">
            <el-select v-model="selectedAlbum" filterable placeholder="搜索专辑">
              <el-option
                v-for="album in filteredAlbums"
                :key="album.id"
                :label="album.name"
                :value="album.id"
              ></el-option>
            </el-select>
          </el-form-item>
          <el-form-item label="艺术家IDs" prop="artistIds">
            <el-select
              v-model="songForm.artistIds"
              multiple
              filterable
              placeholder="输入艺术家ID"
            >
              <el-option
                v-for="artist in artists"
                :key="artist.id"
                :label="artist.name"
                :value="artist.id"
              ></el-option>
            </el-select>
          </el-form-item>
          <el-form-item label="歌曲文件" prop="file">
            <el-upload
              class="upload-demo"
              action=""
              :before-upload="handleFileUpload"
              :show-file-list="false"
            >
              <template #trigger>
                <el-button size="small" type="primary">点击上传</el-button>
              </template>
              <template #tip>
                <div class="el-upload__tip">只能上传音频文件</div>
              </template>
            </el-upload>
            <div v-if="songForm.fileUrl">
              <audio controls :src="songForm.fileUrl"></audio>
            </div>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="submitForm">提交</el-button>
            <el-button @click="resetForm">重置</el-button>
          </el-form-item>
        </el-form>
      </div>
      <TheFooter />
    </div>
  </div>
</template>

<script>
  import axios from 'axios'
  import TheFooter from "./TheFooter.vue";
  import TheHeader from "./TheHeader.vue";
  import { ElForm, ElFormItem, ElInput, ElUpload, ElButton, ElMessage, ElSelect, ElOption } from 'element-plus';

  export default {
    name: 'SongUpload',
    components: {
      TheFooter,
      TheHeader,
      ElForm,
      ElFormItem,
      ElInput,
      ElUpload,
      ElButton,
      ElMessage,
      ElSelect,
      ElOption,
    },
    data() {
      return {
        songForm: {
          name: '',
          duration: '',
          genre: '',
          lyrics: '',
          file: null,
          fileUrl: '',
          artistIds: [],
        },
        selectedAlbum: null,
        albums: [],
        artists: [],
        rules: {
          name: [{ required: true, message: '请输入歌曲名称', trigger: 'blur' }],
          duration: [{ required: true, message: '请输入歌曲时长', trigger: 'blur' }],
          genre: [{ required: true, message: '请输入曲风', trigger: 'blur' }],
          lyrics: [{ required: true, message: '请输入歌词', trigger: 'blur' }],
          file: [{ required: true, message: '请上传歌曲文件', trigger: 'change' }],
          artistIds: [{ required: true, message: '请输入艺术家ID', trigger: 'change' }],
        },
      };
    },
    computed: {
      filteredAlbums() {
        return this.albums.filter(album =>
          album.name.toLowerCase().includes(this.selectedAlbum?.toLowerCase() || '')
        );
      },
    },
    methods: {
      fetchAlbums() {
        axios.get('http://127.0.0.1:4523/m1/4804827-4459167-default/api/submit/1')
          .then(response => {
            this.albums = response.data;
          })
          .catch(error => {
            ElMessage.error('专辑列表加载失败');
            console.error(error);
          });
      },
      fetchArtists() {
        axios.get('http://127.0.0.1:4523/m1/4804827-4459167-default/api/submit/1')
          .then(response => {
            this.artists = response.data;
          })
          .catch(error => {
            ElMessage.error('艺术家列表加载失败');
            console.error(error);
          });
      },
      handleFileUpload(file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          this.songForm.fileUrl = e.target.result;
          this.songForm.file = file;
        };
        reader.readAsDataURL(file);
        return false;
      },
      submitForm() {
        this.$refs.songForm.validate((valid) => {
          if (valid) {
            const formData = new FormData();
            formData.append('name', this.songForm.name);
            formData.append('duration', this.songForm.duration);
            formData.append('genre', this.songForm.genre);
            formData.append('lyrics', this.songForm.lyrics);
            formData.append('file', this.songForm.file);
            formData.append('artistIds', this.songForm.artistIds);
            formData.append('albumId', this.selectedAlbum);

            axios.post('http://127.0.0.1:4523/m1/4804827-4459167-default/api/submit/uploadSong', formData)
            .then(response => {
              if (response.status === 200) {
                ElMessage.success("成功上传歌曲");
                this.resetForm();
              }
            })
            .catch(error => {
              ElMessage.error('出错了，请过段时间尝试重新提交');
            });
          } else {
            ElMessage.error('请填写完整表单！');
            return false;
          }
        });
      },
      resetForm() {
        this.$refs.songForm.resetFields();
        this.songForm.fileUrl = '';
        this.selectedAlbum = null;
        this.songForm.artistIds = [];
      },
    },
    created() {
      this.fetchAlbums();
      this.fetchArtists();
    },
  };
</script>

<style>
.main-container {
  display: flex;
  width: 100vw; 
  box-sizing: border-box; 
  overflow-x: hidden; 
}

.sidebar {
  width: 20%;
  background-color: #f0f0f0; 
  padding: 20px;
}

.song-upload {
  width: 80%;
  padding: 20px;
  box-sizing: border-box; 
}

.upload-demo {
  display: inline-block;
  width: 100%;
}

.cover-image-preview {
  margin-top: 10px;
  max-width: 100%;
  height: auto;
}
</style>
