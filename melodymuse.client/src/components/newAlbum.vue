<template>
  <div class="CreateAlbum">
    
    <div class="main-container">
      <TheHeader />
      <div class="sidebar">
        <!-- 这里放列表栏的内容 -->
        <p>列表栏内容</p>
      </div>
      <div class="album-upload">
        <el-form ref="albumForm" :model="albumForm" :rules="rules" label-width="120px">
          <el-form-item label="专辑名" prop="name">
            <el-input v-model="albumForm.name"></el-input>
          </el-form-item>
          <el-form-item label="制作人ID" prop="producerId">
            <el-input v-model="albumForm.producerId"></el-input>
          </el-form-item>
          <el-form-item label="制作人姓名" prop="producerName">
            <el-input v-model="albumForm.producerName"></el-input>
          </el-form-item>
          <el-form-item label="发行公司" prop="company">
            <el-input v-model="albumForm.company"></el-input>
          </el-form-item>
          <el-form-item label="发行日期" prop="releaseDate">
            <el-date-picker v-model="albumForm.releaseDate" type="date" placeholder="选择日期"></el-date-picker>
          </el-form-item>
          <el-form-item label="专辑展示图片" prop="coverImage">
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
                <div class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div>
              </template>
            </el-upload>
            <img v-if="albumForm.coverImageUrl" :src="albumForm.coverImageUrl" class="cover-image-preview" />
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
  import { ElForm, ElFormItem, ElInput, ElDatePicker, ElUpload, ElButton, ElMessage } from 'element-plus';

  export default {
    name: 'AlbumUpload',
    components: {
      TheFooter,
      TheHeader,
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
        albumForm: {
          name: '',
          producerId: '',
          producerName: '',
          company: '',
          releaseDate: '',
          coverImage: null,
          coverImageUrl: '',
        },
        rules: {
          name: [{ required: true, message: '请输入专辑名', trigger: 'blur' }],
          producerId: [{ required: true, message: '请输入制作人ID', trigger: 'blur' }],
          producerName: [{ required: true, message: '请输入制作人姓名', trigger: 'blur' }],
          company: [{ required: true, message: '请输入发行公司', trigger: 'blur' }],
          releaseDate: [{ required: true, message: '请选择发行日期', trigger: 'change' }],
          coverImage: [{ required: true, message: '请上传专辑展示图片', trigger: 'change' }],
        },
      };
    },
    methods: {
      handleFileUpload(file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          this.albumForm.coverImageUrl = e.target.result;
          this.albumForm.coverImage = file;
        };
        reader.readAsDataURL(file);
        return false; 
      },
      submitForm() {
        this.$refs.albumForm.validate((valid) => {
          if (valid) {

            axios.post('http://127.0.0.1:4523/m1/4804827-4459167-default/api/submit/createAlbum')
            .then(response=>{
              if(response.status===200){
                ElMessage.success("成功创建专辑");
              }
            })
            .catch(error=>{
              ElMessage.error('出错了，请过段时间尝试重新提交');
            })

          } else {
            ElMessage.error('请填写完整表单！');
            return false;
          }
        });
      },
      resetForm() {
        this.$refs.albumForm.resetFields();
        this.albumForm.coverImageUrl = '';
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


.sidebar {
  width: 20%;
  background-color: #f0f0f0; 
  padding: 20px;
}

.album-upload {
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
