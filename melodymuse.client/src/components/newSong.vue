<template>
  <div class="SongUpload">
    <div class="main-container">
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
          <el-form-item label="艺术家名称" prop="selectedArtist">
            <el-select
              v-model="selectedArtist"
              filterable
              remote
              reserve-keyword
              placeholder="输入艺术家名称"
              :remote-method="fetchArtists"
              @change="onArtistSelect"
              :allow-create="true"
              :default-first-option="false"
              :loading="loadingArtists"
              no-match-text="无匹配数据"
            >
              <el-option
                v-for="artist in artists"
                :key="artist.id"
                :label="`${artist.name} - ${artist.detail}`"
                :value="artist"
              >
                <span>{{artist.id +'-'+artist.name +'-' + artist.detail}}</span>
              </el-option>
            </el-select>
          </el-form-item>
          <el-form-item label="专辑" prop="selectedAlbum">
            <el-select 
              v-model="selectedAlbumId" 
              filterable 
              remote 
              reserve-keyword 
              placeholder="搜索专辑"
              :allow-create="true"
              :default-first-option="false"
              no-match-text="无匹配数据"
            >
              <el-option
                v-for="album in albums"
                :key="album.id"
                :label="`${album.name} - ${album.detail}`"  
                :value="album.id"  
              >
                <span>{{ album.id + '-' + album.name + '-' + album.detail }}</span>
              </el-option>
            </el-select>
          </el-form-item>

          <el-form-item label="歌曲文件" prop="file">
            <el-upload
              class="upload-demo"
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
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import { ElForm, ElFormItem, ElInput, ElUpload, ElButton, ElMessage, ElSelect, ElOption } from 'element-plus';

export default {
  name: 'SongUpload',
  components: {
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
      },
      selectedArtist: null, 
      selectedAlbumId: null, 
      albums: [],
      artists: [],
      loadingArtists: false,
      rules: {
        name: [{ required: true, message: '请输入歌曲名称', trigger: 'blur' }],
        duration: [{ required: true, message: '请输入歌曲时长', trigger: 'blur' }],
        genre: [{ required: true, message: '请输入曲风', trigger: 'blur' }],
        lyrics: [{ required: true, message: '请输入歌词', trigger: 'blur' }],
        file: [{ required: true, message: '请上传歌曲文件', trigger: 'change' }],
      },
    };
  },
  methods: {
    fetchArtists(query) {
      if (query !== '') {
        this.loadingArtists = true;
        axios.get(`http://localhost:7223/search/${query}`)
          .then(response => {
            this.artists = response.data.map(artist => ({
              id: artist.artistId,
              name: artist.artistName,
              detail: `粉丝数: ${artist.artistFansNum} | 歌手介绍: ${artist.artistIntro}`,
            }));
            this.loadingArtists = false;
          })
          .catch(error => {
            ElMessage.error('艺术家列表加载失败');
            console.error(error);
            this.loadingArtists = false;
          });
      } else {
        this.artists = [];
      }
    },
    fetchAlbumsByArtistId(artistId) {
      axios.get(`http://localhost:7223/albums/${artistId}`)
        .then(response => {
          this.albums = response.data.map(album => ({
            id: album.albumId,
            name: album.albumName,
            detail:`发行日期: ${album.albumReleasedate} | 发行公司: ${album.albumCompany}`
          }));
        })
        .catch(error => {
          ElMessage.error('专辑列表加载失败');
          console.error(error);
        });
    },
    onArtistSelect(artist) {
      if (artist && artist.id) {
        this.selectedArtist = artist;
        this.fetchAlbumsByArtistId(artist.id);
      }
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
        if (valid && this.selectedArtist) {
          const formData = new FormData();
          formData.append('SongName', this.songForm.name);
          formData.append('Duration', this.songForm.duration);
          formData.append('SongGenre', this.songForm.genre);
          formData.append('Lyrics', this.songForm.lyrics);
          formData.append('SongFile', this.songForm.file);
          formData.append('AlbumId', this.selectedAlbumId);
          formData.append('ArtistIds', this.selectedArtist.id);
          axios.post('http://localhost:7223/api/submit/uploadSong', formData)
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
          ElMessage.error('请填写完整表单并选择艺术家！');
          return false;
        }
      });
    },
    resetForm() {
      this.$refs.songForm.resetFields();
      this.songForm.fileUrl = '';
      this.selectedAlbum = null;
      this.selectedArtist = null;
      this.artists = [];
      this.albums = [];
    },
  },
};
</script>

<style scoped>
.main-container {
  display: flex;
  width: 100vw; 
  box-sizing: border-box; 
  overflow-x: hidden; 
}

.song-upload {
  width: 60%;
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

