<template>
  <el-dialog v-model="isVisibleLocal"  @close="cancelHandler">
    <el-form ref="vForm" :model="formData" :rules="rules" label-width="120px">
      <el-form-item label="歌曲名称" prop="SongName">
        <el-input v-model="formData.SongName"></el-input>
      </el-form-item>
      <el-form-item label="歌曲流派" prop="SongGenre">
        <el-input v-model="formData.SongGenre"></el-input>
      </el-form-item>
      <el-form-item label="时长(秒)" prop="Duration">
        <el-input v-model.number="formData.Duration"></el-input>
      </el-form-item>
      <el-form-item label="歌词" prop="Lyrics">
        <el-input v-model="formData.Lyrics" type="textarea"></el-input>
      </el-form-item>
      <el-upload
        class="upload-demo"
        drag
        :auto-upload="false"
        :on-change="handleChange"
      >
        <i class="el-icon-upload"></i>
        <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
      </el-upload>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="cancelHandler">取 消</el-button>
        <el-button type="primary" @click="submitForm">确 定</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script>
import {
  defineComponent,
  toRefs,
  reactive,
  getCurrentInstance,
  watch
} from 'vue';
import { ElDialog, ElForm, ElFormItem, ElInput, ElButton, ElUpload, ElMessage } from 'element-plus';
import api from '../api/http.js';

export default defineComponent({
  components: {
    ElDialog,
    ElForm,
    ElFormItem,
    ElInput,
    ElButton,
    ElUpload
  },
  props: ["isVisible"],
  emits: [ "cancelEvent", "submitSuccessEvent","submitFailEvent"],
  setup(props, { emit }) {


    const state = reactive({
      isVisibleLocal: props.isVisible,
      formData: {
        SongName: "",
        SongGenre: "",
        Duration: null,
        Lyrics: "[00:00.000] 暂无歌词",
        SongFile: "",
      },
      rules: {
        SongName: [{
          required: true,
          message: "字段值不可为空"
        }],
        SongGenre: [{
          required: true,
          message: "字段值不可为空"
        }],
        Duration: [{
          required: true,
          message: "字段值不可为空",
          validator: (rule, value, callback) => {
            if (typeof value !== 'number' || value <= 0 || !Number.isInteger(value)) {
              callback(new Error("请输入正整数"));
            } else {
              callback();
            }
          }
        }],
        Lyrics: [{
          required: true,
          message: "无歌词请输入:[00:00.000] 暂无歌词"
        }],
        SongFile: [{
          required: true,
          message: "请上传歌曲文件",
        }]
      },
    });

    const instance = getCurrentInstance();

    const submitForm = async () => {
      console.log(state.formData)
      instance.proxy.$refs['vForm'].validate(async valid => {
        if (!valid) return;
        try {
          const response = await api.apiClient.post('/api/usersub/uploadsong',state.formData,{
            headers: {
                'Content-Type': 'multipart/form-data',
              },
          });
          console.log('歌曲上传成功', response.data);
          emit("submitSuccessEvent");
        } catch (error) {
          emit("submitFailEvent");
          console.error('上传歌曲失败', error);
        }
      });
    };


    const beforeUpload = (file) => {
      const allowedTypes = ['audio/mp3'];
      const maxSize = 10 * 1024 * 1024; // 10MB
      if (!allowedTypes.includes(file.type)) {
        ElMessage.error("请上传 MP3 格式的歌曲文件");
        return false;
      } else if (file.size > maxSize) {
        ElMessage.error("歌曲文件大小不能超过 10MB");
        return false;
      }
      return true;
    };

    const cancelHandler = () => {
      emit("cancelEvent");
    }

    watch(() => props.isVisible, (newVal) => {
      state.isVisibleLocal = newVal;
    });

    const handleChange = (file) => {
      // 保留最新选择的文件
      state.formData.SongFile = file.raw;
      console.log('Selected file:', file);
    }

    return {
      ...toRefs(state),
      submitForm,
      beforeUpload,
      cancelHandler,
      handleChange
    };
  }
});

</script>

<style scoped>
.el-dialog__header {
  background-color: #f9fafb;
}
.el-dialog__body {
  padding: 20px;
}
.el-dialog__footer {
  padding: 10px 20px;
}
</style>