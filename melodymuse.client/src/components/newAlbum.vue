<template>
<!--存放页面的主要容器-->
<div class="main-container">
  <header>
    <TheHeader/>
  </header>
  <div class="pagecontent-container">
    <!-- 留着放之后的侧边栏 -->
    <div class="sidebar">
      <p>放侧边栏的内容</p>
    </div>

    <!-- 填写专辑有关信息的区域 -->
    <div class="album-upload">
      <!-- 表单组件 -->
      <el-form ref="albumForm" :model="albumForm" :rules="rules" label-width="120px">
        <!-- 制作人ID输入框 -->
        <el-form-item label="制作人ID" prop="producerId">
          <el-input v-model="albumForm.producerId"></el-input>
        </el-form-item>
        <!-- 制作人姓名输入框 -->
        <el-form-item label="制作人姓名" prop="producerName">
          <el-input v-model="albumForm.producerName"></el-input>
        </el-form-item>
        <!-- 发行公司输入框 -->
        <el-form-item label="发行公司" prop="company">
          <el-input v-model="albumForm.company"></el-input>
        </el-form-item>
        <!-- 发行日期选择器 -->
        <el-form-item label="发行日期" prop="releaseDate">
          <el-date-picker v-model="albumForm.releaseDate" type="date" placeholder="选择日期"></el-date-picker>
        </el-form-item>
        <!-- 专辑展示图片上传组件 -->
        <el-form-item label="专辑封面图片" prop="coverImage">
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
  <footer>
    <TheFooter/>
  </footer>
</div>


</template>

<script>

//引入项目展示框
import TheHeader from "./TheHeader.vue"
import TheFooter from "./TheFooter.vue"
//引入其他组件
import axios from "axios"
import {ElForm,ElFormItem,ElInput,ElDatePicker,ElUpload,ElButton,ElMessage} from 'element-plus';

export default {
  name:"UploadAlbum",

  components:{
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

  data(){
    return{
      //创建专辑需要填写的数据
      albumForm:{
        albumName:"",
        artistId:"",
        artistName:"",
        company:"",
        releaseDate:"",
        coverImage:null,
      },
      //验证规则
      rules:{
          name: [{ required: true, message: '请输入专辑名', trigger: 'blur' }], // 专辑名验证规则
          producerId: [{ required: true, message: '请输入制作人ID', trigger: 'blur' }], // 制作人ID验证规则
          producerName: [{ required: true, message: '请输入制作人姓名', trigger: 'blur' }], // 制作人姓名验证规则
          company: [{ required: true, message: '请输入发行公司', trigger: 'blur' }], // 发行公司验证规则
          releaseDate: [{ required: true, message: '请选择发行日期', trigger: 'change' }], // 发行日期验证规则
          coverImage: [{ required: true, message: '请上传专辑展示图片', trigger: 'change' }], // 专辑展示图片验证规则
      },
    };
  },

  //相关方法
  methods:{
    // 文件上传前的处理函数
    handleFileUpload(file) {
      const reader = new FileReader();
      reader.onload = (e) => {
        this.albumForm.coverImageUrl = e.target.result; // 设置图片预览URL
        this.albumForm.coverImage = file; // 设置图片文件
      };
      reader.readAsDataURL(file); // 读取文件为DataURL
      return false; // 阻止默认上传行为
    },
    // 提交表单函数
    submitForm() {
      this.$refs.albumForm.validate((valid) => {
        if (valid) {
          // 表单验证通过，发送HTTP POST请求
          axios.post('http://127.0.0.1:4523/m1/4804827-4459167-default/api/submit/createAlbum')
          .then(response=>{
            if(response.status===200){
              ElMessage.success("成功创建专辑"); // 显示成功消息
            }
          })
          .catch(error=>{
            ElMessage.error('出错了，请过段时间尝试重新提交'); // 显示错误消息
          })
        } else {
          ElMessage.error('请填写完整表单！'); // 显示错误消息
          return false;
        }
      });
    },
    // 重置表单函数
    resetForm() {
      this.$refs.albumForm.resetFields(); // 重置表单字段
      this.albumForm.coverImageUrl = ''; // 清空图片预览URL
    },
  }



}
</script >

<style >

.main-container{
  display: flex;
  flex-direction: column;
  width:100%;
  box-sizing: border-box;
  overflow: hidden;
}

.pagecontent-container{
  display:flex;
  flex-direction: row;
  width: 100%;
}

.sidebar{
  width:20%;
  box-sizing: border-box;
}

.album-upload{
  border: 5px dotted pink;
  width:80%;
  box-sizing: border-box;
}

.cover-image-preview {
  width:300px;
  height:300px;
}


</style>