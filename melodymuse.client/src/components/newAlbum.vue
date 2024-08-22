<template>
  <!--存放页面的主要容器-->
  <div class="main-container">
    <div class="pagecontent-container">
      <!-- 填写专辑有关信息的区域 -->
      <div class="album-upload">
        <!-- 表单组件 -->
        <el-form ref="albumForm" :model="albumForm" :rules="rules" label-width="120px">
          <!-- 专辑名称输入框 -->
          <el-form-item label="专辑名称" prop="AlbumName">
            <el-input v-model="albumForm.AlbumName"></el-input>
          </el-form-item>
          <!-- 艺人ID输入框 -->
          <el-form-item label="艺人ID" prop="ArtistId">
            <el-input v-model="albumForm.ArtistId"></el-input>
          </el-form-item>
          <!-- 发行公司输入框 -->
          <el-form-item label="发行公司" prop="AlbumCompany">
            <el-input v-model="albumForm.AlbumCompany"></el-input>
          </el-form-item>
          <!-- 发行日期选择器 -->
          <el-form-item label="发行日期" prop="AlbumReleaseDate">
            <el-date-picker v-model="albumForm.AlbumReleaseDate" type="date" placeholder="选择日期"></el-date-picker>
          </el-form-item>
          <!-- 专辑制作人输入框 -->
          <el-form-item label="制作人" prop="AlbumProducer">
            <el-input v-model="albumForm.AlbumProducer"></el-input>
          </el-form-item>
          <!-- 专辑展示图片上传组件 -->
          <el-form-item label="专辑封面图片" prop="AlbumCover">
            <el-upload
              class="upload-demo"
              action=""
              :before-upload="handleFileUpload"
              :show-file-list="false"
            >
              <!-- 上传按钮 -->
              <template #trigger>
                <el-button size="small" type="primary">点击上传</el-button>
              </template>
              <!-- 上传提示信息 -->
              <template #tip>
                <div class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div>
              </template>
            </el-upload>
            <!-- 上传后的图片预览 -->
            <img v-if="albumForm.coverImageUrl" :src="albumForm.coverImageUrl" class="cover-image-preview" />
          </el-form-item>
          <!-- 提交和重置按钮 -->
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
import axios from "axios";
import { ElForm, ElFormItem, ElInput, ElDatePicker, ElUpload, ElButton, ElMessage } from 'element-plus';

export default {
  name: "UploadAlbum",

  components: {
    ElForm,
    ElFormItem,
    ElInput,
    ElDatePicker,
    ElUpload,
    ElButton,
    ElMessage,
  },

  data() {
    return {
      // 创建专辑需要填写的数据
      albumForm: {
        AlbumName: "",
        AlbumReleaseDate: "",
        AlbumCompany: "",
        AlbumProducer: "",
        AlbumCover: null,
        ArtistId: ""
      },
      // 验证规则
      rules: {
        AlbumName: [{ required: true, message: '请输入专辑名称', trigger: 'blur' }],
        ArtistId: [{ required: true, message: '请输入艺人ID', trigger: 'blur' }],
        AlbumCompany: [{ required: true, message: '请输入发行公司', trigger: 'blur' }],
        AlbumReleaseDate: [{ required: true, message: '请选择发行日期', trigger: 'change' }],
        AlbumProducer: [{ required: true, message: '请输入制作人', trigger: 'blur' }],
        AlbumCover: [{ required: true, message: '请上传专辑展示图片', trigger: 'change' }],
      },
    };
  },

  methods: {
    // 文件上传前的处理函数
    handleFileUpload(file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.albumForm.coverImageUrl = e.target.result;
        this.albumForm.AlbumCover = file;
      };
      reader.readAsDataURL(file);
      return false;
    },

    // 提交表单
    async submitForm() {
      this.$refs.albumForm.validate(async (valid) => {
        if (valid) {
          try {
            const formData = new FormData();
            // 格式化日期为 'YYYY-MM-DD'
            this.albumForm.AlbumReleaseDate = this.albumForm.AlbumReleaseDate.toISOString().split('T')[0];
            Object.keys(this.albumForm).forEach((key) => {
              formData.append(key, this.albumForm[key]);
            });
            const response = await axios.post('http://localhost:7223/api/submit/createAlbum', formData, {
              headers: {
                'Content-Type': 'multipart/form-data',
              },
            });
            console.log(response.data);
            ElMessage.success('专辑创建成功');
          } catch (error) {
            console.error(error);
            ElMessage.error('专辑创建失败');
          }
        }
      });
    },

    // 重置表单
    resetForm() {
      Object.keys(this.albumForm).forEach((key) => {
        this.albumForm[key] = '';
      });
      this.albumForm.coverImageUrl = null;
      this.$refs.albumForm.resetFields();
    },
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

.pagecontent-container {
  display: flex;
  flex-direction: row;
  width: 100%;
}

.album-upload {
  width: 60%;
  padding: 20px;
  box-sizing: border-box; 
}

.cover-image-preview {
  width: 300px;
  height: 300px;
}
</style>
