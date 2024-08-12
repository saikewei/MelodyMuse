<template>
  <div class="personal-info">
    <el-card>
      <div slot="header" class="clearfix">
        <span>个人信息</span>
      </div>
      <div class="info-container">
        <div class="left-panel">
          <div class="avatar-section">
            <img :src="avatar" alt="头像" class="avatar" />
            <el-button type="primary" @click="showChangeAvatarDialog">修改头像</el-button>
          </div>
          <el-button type="primary" @click="$router.push('/ForgottenPassword')">修改密码</el-button>
        </div>
        <div class="right-panel">
          <el-form :model="userInfo" label-width="80px">
            <el-form-item label="昵称">
              <el-input v-model="userInfo.nickname"></el-input>
            </el-form-item>
            <el-form-item label="生日">
              <el-date-picker v-model="userInfo.birthday" type="date" placeholder="选择日期"></el-date-picker>
            </el-form-item>
            <el-form-item label="性别">
              <el-radio-group v-model="userInfo.gender">
                <el-radio label="男">男</el-radio>
                <el-radio label="女">女</el-radio>
              </el-radio-group>
            </el-form-item>
            <el-form-item label="地址">
              <el-input v-model="userInfo.address"></el-input>
            </el-form-item>
            <el-form-item label="手机号">
              <el-input v-model="userInfo.phone"></el-input>
            </el-form-item>
            <el-form-item label="邮箱">
              <el-input v-model="userInfo.email" suffix-icon="el-icon-edit" @click="showChangeEmailDialog"></el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="saveInfo">保存</el-button>
            </el-form-item>
          </el-form>
        </div>
      </div>
    </el-card>

    <!-- 修改头像对话框 -->
    <el-dialog title="修改头像" :visible.sync="changeAvatarDialogVisible">
      <input type="file" @change="onAvatarChange" />
      <span>请选择新的头像文件。</span>
      <div slot="footer" class="dialog-footer">
        <el-button @click="changeAvatarDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="changeAvatar">确定</el-button>
      </div>
    </el-dialog>

  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import axios from 'axios';

export default defineComponent({
  data() {
    return {
      avatar: 'https://via.placeholder.com/100',
      userInfo: {
        nickname: 'sky',
        birthday: new Date(2011, 1, 10),
        gender: '男',
        address: '中国',
        phone: '15012345678',
        email: '1954691462@qq.com',
      },
      changeAvatarDialogVisible: false,
      changePasswordDialogVisible: false,
      changeEmailDialogVisible: false,
      newAvatar: null,
      passwordForm: {
        oldPassword: '',
        newPassword: '',
        confirmPassword: '',
      },
      newEmail: '',
      resetError: '',
    };
  },
  methods: {
    showChangeAvatarDialog() {
      this.changeAvatarDialogVisible = true;
    },
    showChangePasswordDialog() {
      this.changePasswordDialogVisible = true;
    },
    showChangeEmailDialog() {
      this.changeEmailDialogVisible = true;
    },
    onAvatarChange(event: Event) {
      const target = event.target as HTMLInputElement;
      if (target.files) {
        this.newAvatar = target.files[0];
      }
    },
    async changeAvatar() {
      if (this.newAvatar) {
        const formData = new FormData();
        formData.append('avatar', this.newAvatar);

        try {
          const response = await axios.post('/api/upload-avatar', formData, {
            headers: {
              'Content-Type': 'multipart/form-data',
            },
          });

          this.avatar = response.data.avatarUrl;
          this.changeAvatarDialogVisible = false;
          this.$message.success('头像修改成功');
        } catch (error) {
          console.error(error);
          this.$message.error('头像修改失败');
        }
      } else {
        this.$message.error('请选择一个头像文件');
      }
    },
    async changePassword() {
      if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
        this.$message.error('新密码与确认密码不匹配');
        return;
      }

      try {
        await axios.post('/api/change-password', this.passwordForm);
        this.changePasswordDialogVisible = false;
        this.$message.success('密码修改成功');
        this.passwordForm = { oldPassword: '', newPassword: '', confirmPassword: '' };
      } catch (error) {
        console.error(error);
        this.$message.error('密码修改失败');
      }
    },
    async changeEmail() {
      if (this.newEmail) {
        try {
          await axios.post('/api/change-email', { email: this.newEmail });

          this.userInfo.email = this.newEmail;
          this.changeEmailDialogVisible = false;
          this.$message.success('邮箱修改成功');
          this.newEmail = '';
        } catch (error) {
          console.error(error);
          this.$message.error('邮箱修改失败');
        }
      } else {
        this.$message.error('请输入新的邮箱');
      }
    },
    async saveInfo() {
      if (!/^\d{11}$/.test(this.userInfo.phone)) {
        this.resetError = '请输入11位有效的手机号码。';
        return;
      }
      try {
        await axios.post('http://127.0.0.1:4523/m2/4804827-4459167-default/modify', this.userInfo);
        this.$message.success('个人信息保存成功');
        this.resetError = '';
      } catch (error) {
        console.error(error);
        this.$message.error('个人信息保存失败');
      }
    },
  },
});
</script>

<style scoped>
.personal-info {
  display: flex;
  justify-content: center;
}
.info-container {
  display: flex;
  width: 100%;
}
.left-panel {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-right: 20px;
}
.avatar-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-bottom: 20px;
}
.avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  margin-bottom: 10px;
}
.right-panel {
  flex: 3;
}
</style>