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
          <el-form-item label="歌曲名称" prop="input12931" class="required label-right-align">
            <el-input v-model="formData.input12931" type="text" clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12" class="grid-cell">
          <el-form-item label="歌曲流派" prop="input23031" class="label-right-align">
            <el-input v-model="formData.input23031" type="text" clearable></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="24" class="grid-cell">
          <el-row>
            <el-col :span="12" class="grid-cell">
              <el-form-item label="歌手" prop="input21642" class="required label-right-align">
                <el-input v-model="formData.input21642" type="text" clearable></el-input>
              </el-form-item>
            </el-col>
            <el-col :span="12" class="grid-cell">
              <el-form-item label="作词人" prop="input71230" class="required label-right-align">
                <el-input v-model="formData.input71230" type="text" clearable></el-input>
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item label="发行日期" prop="date67505" class="label-right-align">
            <el-date-picker v-model="formData.date67505" type="date" class="full-width-input"
              format="YYYY-MM-DD" value-format="YYYY-MM-DD" clearable></el-date-picker>
          </el-form-item>
          <div class="static-content-item">
            <el-divider direction="horizontal"></el-divider>
          </div>
        </el-col>
        <el-col :span="24" class="grid-cell">
          <el-form-item label="歌词" prop="textarea21654" class="label-right-align">
            <el-input type="textarea" v-model="formData.textarea21654" rows="3"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
      <div class="static-content-item">
        <el-divider direction="horizontal"></el-divider>
      </div>      
    </el-form>
      <div class="dialog-footer">
        <el-button type="primary" @click="dialogFormVisible = false">
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
      getCurrentInstance
    }
    from 'vue'

    export default defineComponent({
      components: {},
      props: ["song_name"],
      setup() {
        const state = reactive({
          formData: {
            input12931: "",
            input23031: "",
            input21642: "",
            input71230: "",
            date67505: null,
            textarea21654: "",
          },
          rules: {
            input12931: [{
              required: true,
              message: '字段值不可为空',
            }],
            input21642: [{
              required: true,
              message: '字段值不可为空',
            }],
            input71230: [{
              required: true,
              message: '字段值不可为空',
            }],
          },
        })
        const instance = getCurrentInstance()
        const submitForm = () => {
          instance.proxy.$refs['vForm'].validate(valid => {
            if (!valid) return
            //TODO: 提交表单
          })
        }
        const resetForm = () => {
          instance.proxy.$refs['vForm'].resetFields()
        }
        return {
          ...toRefs(state),
          submitForm,
          resetForm
        }
      },
      emits:["cancelEvent"],
      mounted() {
        this.formData.input12931=this.song_name;
      },
      methods:{
        cancelHandler(){
          this.$emit("cancelEvent");
        }
      },
  
    })
    
  </script>
  
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