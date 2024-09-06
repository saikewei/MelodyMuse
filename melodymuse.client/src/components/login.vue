<template>
  <div class="container">
    <div class="main">
      <div class="loginbox">
        <div class="loginbox-in">
          <div class="header">MelodyMuse</div>
          <div class="form-wrapper">

            <div class="input-wrapper">
              <!--<span class="iconfont icon-account"></span>-->
              <input type="tel" name="phonenumber" placeholder="手机号码" class="input-item" v-model="username">
            </div>
            <div class="input-wrapper">
              <!--<span class="iconfont icon-key"></span>-->
              <input type="password" name="password" placeholder="密码" class="input-item" v-model="password">
            </div>
            <div class="btn2" @click="login">Login</div> 

          </div>
          <p v-if="loginError" class="error-message">{{ loginError }}</p>
        </div>
      </div>
      <!-- 右侧盒子 -->
      <div class="background">
        
          <div class="title"><h3>欢迎来到MelodyMuse！请先登录</h3></div>
      </div>
    </div>
  </div>
</template>

<script>
import api from '../api/http.js'
import { ElMessage } from 'element-plus';

export default {
  data() {
    return {
      username: '',
      password: '',
      loginError: ''
    };
  },
  methods: {
    async login() {
      if (this.username.trim() === '') {
        this.loginError = '手机号码不能为空。';
        return;
      }

      if (this.password.trim() === '') {
        this.loginError = '密码不能为空。';
        return;
      }

      if (!/^\d{11}$/.test(this.username)) {
        this.loginError = '请输入11位有效的手机号码。';
        return;
      }

      try {
      const response = await api.apiClientWithoutToken.post('/api/account/login', {
        PhoneNumber: this.username,
        Password: this.password
      });

      if (response.status === 200 && response.data.token) {
        ElMessage.success('登录成功');
        localStorage.setItem('token', response.data.token);
        this.$router.push('/');
      } else {
        this.loginError = typeof response.data === 'string' 
          ? response.data 
          : (response.data.msg || '登录失败，请再次尝试');
      }
    } catch (error) {
  if (error.response) {
    // 服务器返回了一个非 2xx 的响应
    console.error('Response error:', error.response);
    if (error.response.data && typeof error.response.data === 'string') {
      this.loginError = error.response.data;
    } else if (error.response.data && error.response.data.msg) {
      this.loginError = error.response.data.msg;
    } else {
      this.loginError = `服务器错误，状态码: ${error.response.status}`;
    }
  } else if (error.request) {
    // 请求已经发出，但没有收到响应
    console.error('Request error:', error.request);
    this.loginError = '请求已发送，但服务器无响应，请稍后重试。';
  } else {
    // 其他类型的错误
    console.error('Other error:', error.message);
    this.loginError = '发生未知错误，请稍后重试。';
  }
}

    }
  },
  name: "Login"
}
</script>

<style scoped>
html, body {
  height: 100%;
  margin: 0;
}
.container {
  height: 100%;
  position:absolute;
  left:0;
  top:0;
  width: 100%;
  background-image: linear-gradient(to right, #6a8cdcc1, white);
  display: flex;
  justify-content: left;
  align-items: center;
}
.main {
  display: flex;
  justify-content:space-evenly; /* 使用 space-between 将元素分散排列，实现图片右对齐效果 */
}
.loginbox {
  display: flex;
  width: 52%;
  height: 60%;
  position: absolute;
  top:20%;
  left:24%;
  box-shadow: 0 12px 16px 0 rgba(0, 0, 0, 0.24), 0 17px 50px 0 rgba(0, 0, 0, 0.19);
  background-image: linear-gradient(to left, #c6d6f7c1, rgba(255, 255, 255, 0.902));
}
.loginbox-in {
  width: 42%;
  border-radius: 15px;
  padding: 0 50px;
  position: absolute;
  left:25%;
  top:10%;
  transform: translateX(-50%);
  display: flex;
  flex-direction: column;
  align-items: center;
}
.header {
  font-size: 36px;
  color: #284da0c1;
  font-weight:bolder;
  text-align: center;
  line-height: 80px;
  margin-top: 20px;
}
.form-wrapper {
  width: 100%;
  padding-top: 30px;
}
.input-wrapper {
  display: flex;
  align-items: center;
  margin-bottom: 25px;
}
.input-item {
  display: block;
  width: calc(100% - 20px);
  margin-left: 10px;
  padding: 12px;
  border: 1px solid rgb(128, 125, 125);
  border-radius: 10px;
  font-size: 15px;
  outline: none;
}
.input-item::placeholder {
  text-transform: uppercase;
}
.btn2 {
  text-align: center;
  padding: 8px;
  width: 100%;
  margin-top: 40px;
  background-color: #6a8cdcc1;
  color: #ffffff;
  border-radius: 10px;
  cursor: pointer;
  font-size: 20px;
}
.btn2:hover {
  background-color: #284da0c1;
}
.btn2:active {
  position: relative;
  top: 1px;
}
.error-message {
  color: rgba(209, 41, 41, 0.815);
  text-align: center;
  margin-top: 10px;
}
.background {
  width: 500px;
  justify-content:center;
  align-items:flex-end;
  background-image: url('src/assets/m.png'); /* 确保存在该图片 */
  background-size: cover;
}
.title {
    position:absolute;
    top: 50%;
    left:46%;
    font-size:22px;
    color:#284da0c1;
}
.title:hover {
  font-size: 20px;
  transition: all 0.4s ease-in-out;
  cursor: pointer;
}

.iconfont {
  font-family: "iconfont" !important;
  font-size: 20px;
  font-style: normal;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  height: 22px;
  color: #4E655D;
  margin-right: 10px;
  margin-top: 3px;
}
input:-webkit-autofill {
  box-shadow: 0 0 0 1000px rgba(255, 255, 255, 0) inset !important; /* 背景透明 */
  -webkit-text-fill-color: #445b53 !important; /* 文本颜色 */
  transition: background-color 5000s ease-in-out 0s;
}

input:-webkit-autofill::first-line {
  font-size: 15px;
  font-weight: bold;
}


</style>
