<template>
    <el-form :model="formData" ref="vForm" :rules="rules" label-position="left" label-width="100px"
      size="default" @submit.prevent>
      <div class="static-content-item">
        <div>编辑歌曲信息</div>
      </div>
      <div class="static-content-item">
        <el-divider direction="horizontal"></el-divider>
      </div>
      <el-row>
        <el-col :span="12" class="grid-cell">
          <el-form-item label="歌曲名称" prop="songName" class="required label-right-align">
            <el-input v-model="formData.songName" type="text" clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12" class="grid-cell">
          <el-form-item label="歌曲流派" prop="songGenre" class="label-right-align">
            <el-input v-model="formData.songGenre" type="text" clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="24" class="grid-cell">            
          <el-form-item label="发行日期" prop="songDate" class="label-right-align">
            <el-date-picker v-model="formData.songDate" type="date" class="full-width-input"
              format="YYYY-MM-DD" value-format="YYYY-MM-DD" clearable></el-date-picker>
          </el-form-item>
          <div class="static-content-item">
            <el-divider direction="horizontal"></el-divider>
          </div>
        </el-col>
        <el-col :span="24" class="grid-cell">
          <el-form-item label="歌词" prop="lyrics" class="label-right-align">
            <el-input type="textarea" v-model="formData.lyrics" rows="3"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <div class="static-content-item">
        <el-divider direction="horizontal"></el-divider>
      </div>      
    </el-form>
      <div class="dialog-footer">
        <el-button type="primary" @click="submitForm">
          确认更改
        </el-button>
        <el-button @click="cancelHandler">取消</el-button>
      </div>
  </template>
  
  <script>
    import {
      defineComponent,
      toRefs,
      reactive,
      getCurrentInstance,
    }
    from 'vue'
    import axios from 'axios';
    import { format } from 'date-fns';

    export default defineComponent({
  props: ["song_id"],
  emits: ["cancelEvent", "submitEvent"],
  setup(props, { emit }) {
    const state = reactive({
      formData: {
        songName: "",
        songGenre: "",
        songDate: null,
        lyrics: "",
      },
      rules: {
        songName: [{
          required: true,
          message: '字段值不可为空',
        }],
      },
    });

    const instance = getCurrentInstance();

    const submitForm = async () => {
      instance.proxy.$refs['vForm'].validate(async valid => {
        if (!valid) return;
        // 提交表单
        try {
          const response = await axios.put(`https://localhost:7223/api/songedit/${props.song_id}`, state.formData);
          console.log('歌曲信息更新成功', response.data); 
          cancelHandler();
        } catch (error) {
          console.error('更新歌曲信息失败', error);
        }
        emit("submitEvent");
      });
    };

    const cancelHandler = () => {
      emit("cancelEvent");
    };

    const getSongInfoById = async () => {
      try {
        const response = await axios.get(`https://localhost:7223/api/songedit/${props.song_id}`);
        state.formData.songName = response.data.songName;
        state.formData.songGenre = response.data.songGenre;
        state.formData.songDate = format(new Date(response.data.songDate), 'yyyy-MM-dd');
        state.formData.lyrics = response.data.lyrics;
      } catch (error) {
        console.error('获取歌曲数据失败', error);
        cancelHandler();
      }
    };

    return {
      ...toRefs(state),
      submitForm,
      cancelHandler,
      getSongInfoById
    };
  },
  created() {
    this.getSongInfoById();
  },
});
</script>

<style lang="scss">
/* your styles here */
</style>
  
  <style lang="scss">
    .el-input-number.full-width-input,
    .el-cascader.full-width-input {
      width: 100% !important;
    }
    
    .el-form-item--medium {
      .el-radio {
        line-height: 36px !important;
      }
      
      .el-rate {
        margin-top: 8px;
      }
    }
    
    .el-form-item--small {
      .el-radio {
        line-height: 32px !important;
      }
      
      .el-rate {
        margin-top: 6px;
      }
    }
    
    .el-form-item--mini {
      .el-radio {
        line-height: 28px !important;
      }
      
      .el-rate {
        margin-top: 4px;
      }
    }
    
    .clear-fix:before,
    .clear-fix:after {
      display: table;
      content: "";
    }
    
    .clear-fix:after {
      clear: both;
    }
    
    .float-right {
      float: right;
    }
    
  </style>
  
  <style lang="scss" scoped>
    div.table-container {
      table.table-layout {
        width: 100%;
        table-layout: fixed;
        border-collapse: collapse;
  
        td.table-cell {
          display: table-cell;
          height: 36px;
          border: 1px solid #e1e2e3;
        }
      }
    }
    
    div.tab-container {}
    
    .label-left-align :deep(.el-form-item__label) {
      text-align: left;
    }
    
    .label-center-align :deep(.el-form-item__label) {
      text-align: center;
    }
    
    .label-right-align :deep(.el-form-item__label) {
      text-align: right;
    }
    
    .custom-label {}
    
    .static-content-item {
      min-height: 20px;
      display: flex;
      align-items: center;
  
      :deep(.el-divider--horizontal) {
        margin: 0;
      }
    }
    
  </style>