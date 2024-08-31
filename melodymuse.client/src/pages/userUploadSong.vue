<template>
  <div>
    <el-button @click="showUploadWindow">上传歌曲</el-button>
    <UploadSong :isVisible="uploadWindowsVisible" @cancelEvent="handleCancelEvent" @submitSuccessEvent="handleSubmitSuccess" @submitFailEvent="handleSubmitFail"/>
  </div>
</template>

<script>
import UploadSong from '../components/userNewSong.vue';
import { ElButton, ElMessage } from 'element-plus';

export default {
  components: {
    UploadSong,
    ElButton,
    ElMessage,
  },
  data() {
    return {
      uploadWindowsVisible: false,
    };
  },
  methods: {
    showUploadWindow() {
      this.uploadWindowsVisible =!this.uploadWindowsVisible;
    },
    handleCancelEvent() {
      this.uploadWindowsVisible = false;
    },
    handleSubmitSuccess() {
      this.uploadWindowsVisible = false;
      ElMessage({
        message:"上传成功",
        type:"success",
      })
    },
    handleSubmitFail() {
      ElMessage({
        message:"上传失败",
        type:"error",
      })
    }
  }
};
</script>

<style scoped>

</style>