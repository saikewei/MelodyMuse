<template>
  <div class="personal-info">
    <el-card>
      <div slot="header" class="clearfix">
        <span>个人信息</span>
      </div>
      <div class="info-container">
        <div class="left-panel">
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
              <el-form-item label="年龄">
              <el-input :value="calculateAge(userInfo.birthday) + '岁'" disabled></el-input>
            </el-form-item>
            <el-form-item label="性别">
              <el-radio-group v-model="userInfo.gender">
                <el-radio label="男">男</el-radio>
                <el-radio label="女">女</el-radio>
              </el-radio-group>
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
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import axios from 'axios';

export default defineComponent({
  data() {
    return {
      //avatar: 'https://via.placeholder.com/100',
      userInfo: {
        userId: '',
        nickname: '',
        password: '',
        birthday: '',
        gender: '',
        phone: '',
        email: '',
        status:'',
      },
      //changeAvatarDialogVisible: false,
      changePasswordDialogVisible: false,
      changeEmailDialogVisible: false,
      //newAvatar: null,
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

    //利用出生日期计算年龄
    calculateAge(birthday: string) {
      if (!birthday) return '';
      const birthDate = new Date(birthday);
      const today = new Date();
      let age = today.getFullYear() - birthDate.getFullYear();
      const monthDifference = today.getMonth() - birthDate.getMonth();
      if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }
      return age;
    },

    async fetchUserInfo(userId: string) {
      try {
        const response = await axios.get(`https://localhost:7223/api/users/${userId}`);
        this.userInfo = {
          userId: response.data.UserId,
          nickname: response.data.UserName,
          password:response.data.Password,
          birthday: new Date(response.data.UserBirthday),
          sex: response.data.UserSex,
          phone: response.data.UserPhone,
          email: response.data.UserEmail,
          status: response.data.UserStatus,
        };
        //this.avatar = response.data.UserAvatar || 'https://via.placeholder.com/100'; // Assuming UserAvatar field
      } catch (error) {
        console.error('获取用户信息失败:', error);
      }
    },

    async updateUserInfo() {
      try {
        const updatedInfo = {
          UserId: this.userInfo.userId,
          UserName: this.userInfo.nickname,
          Password: this.userInfo.password,
          UserBirthday: this.userInfo.birthday.toISOString(),
          UserSex: this.userInfo.gender,
          UserPhone: this.userInfo.phone,
          UserEmail: this.userInfo.email,
          UserAge: this.calculateAge(this.userInfo.birthday.toISOString()), 
          UserStatus: this.userInfo.status,
        };
        await axios.put(`https://localhost:7223/api/users/${this.userInfo.userId}`, updatedInfo);
        this.$message.success('信息更新成功');
      } catch (error) {
        console.error('Failed to update user info:', error);
        this.$message.error('信息更新失败');
      }
    },
    /*showChangeAvatarDialog() {
      this.changeAvatarDialogVisible = true;
    },
    */
    showChangePasswordDialog() {
      this.changePasswordDialogVisible = true;
    },
    showChangeEmailDialog() {
      this.changeEmailDialogVisible = true;
    },
    /*onAvatarChange(event: Event) {
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
          await axios.post(`/api/users/${this.userId}/avatar`, formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
          });
          this.$message.success('头像更新成功');
          this.changeAvatarDialogVisible = false;
        } catch (error) {
          console.error('Failed to change avatar:', error);
          this.$message.error('头像更新失败');
        }
      }
    },
      */
    
    async saveInfo() {
      await this.updateUserInfo();
    }
  },
  mounted() {
    const userId = this.$route.params.userId;  
      // 处理获取到的用户信息
      this.fetchUserInfo(userId); // 使用获取到的 userId
    }

    /*放在template中的修改头像部分
    1.修改密码上方：<div class="avatar-section">
            <img :src="avatar" alt="头像" class="avatar" />
            <el-button type="primary" @click="showChangeAvatarDialog">修改头像</el-button>
          </div>
    2.template最后
     <!-- 修改头像对话框-->
   <el-dialog title="修改头像" :visible.sync="changeAvatarDialogVisible">
      <input type="file" @change="onAvatarChange" />
      <span>请选择新的头像文件。</span>
      <div slot="footer" class="dialog-footer">
        <el-button @click="changeAvatarDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="changeAvatar">确定</el-button>
      </div>
    </el-dialog>
    */ 
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
  position:relative;
  top:20px;
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
