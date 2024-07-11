<template>
    <div class="login-form">
      <h2></h2>
      <form @submit.prevent="login">
        <div class="form-group">
          <label for="username">username</label>
          <input type="text" id="username" v-model="username" required>
        </div>
        <div class="form-group">
          <label for="password">password</label>
          <input type="password" id="password" v-model="password" required>
        </div>
        <button type="submit">login</button>
      </form>
      <p v-if="loginError" class="error-message">{{ loginError }}</p>
      <p class="no-account">还没有账号？ <router-link to="/register">点击注册</router-link></p>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        username: '',
        password: '',
        loginError: ''
      };
    },
    methods: {
      login() {
        // 在这里添加实际的登录逻辑，例如发送请求到后端验证用户信息
        // 这里只是简单示例，假设用户名为 "admin"，密码为 "password"
        if (this.username === 'admin' && this.password === 'password') {
          // 登录成功的处理，可以跳转页面或者设置登录状态等
          alert('登录成功！');
        } else {
          // 登录失败的处理
          this.loginError = '用户名或密码错误，请重试。';
        }
      }
    }
  };
  </script>
  
  <style scoped>
  
  .login-form {
    max-width: 300px;
    margin: auto;
    padding: 20px;
    border: 1px solid #e2cde2;
    border-radius: 5px;
  }
  
  .form-group {
    margin-bottom: 15px;
  }
  
  label {
    display: block;
    margin-bottom: 5px;
    color: #94739a;
  }
  
  input[type="text"],
  input[type="password"],
  button {
    width: 100%;
    padding: 10px;
    font-size: 16px;
  
  }
  
  button {
    margin-top: 15px;
    background-color: #d398e0;
    color: white;
    border: none;
    cursor: pointer;
    border-radius: 5px;
  }
  
  button:hover {
    background-color: #8f4395;
  }
  
  .error-message {
    color: red;
    margin-top: 10px;
  }
  
  .no-account {
    margin-top: 10px;
    color: rgb(136, 113, 141);
  }
  </style>
   