<template>
  <div class="container">
    <div class="login-wrapper">
      <div class="header">MelodyMuse</div>
      <div class="form-wrapper">
        <input type="tel" name="phonenumber" placeholder="手机号码" class="input-item" v-model="username">
        <input type="password" name="password" placeholder="密码" class="input-item" v-model="password">
        <div class="btn2" @click="login">Login</div>
      </div>
      <p v-if="loginError" class="error-message">{{ loginError }}</p>
    </div>
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
    async login() {
        // 验证手机号码是否为空
      if (this.username.trim() === '') {
        this.loginError = '手机号码不能为空。';
        return;
      }

      // 验证密码是否为空
      if (this.password.trim() === '') {
        this.loginError = '密码不能为空。';
        return;
      }

      // 验证手机号码长度和格式
      if (!/^\d{11}$/.test(this.username)) {
        this.loginError = '请输入11位有效的手机号码。';
        return;
      }

      try {
        const response = await axios.post('http://127.0.0.1:4523/m1/4804827-4459167-default/api/account/login?apifoxResponseId=487488274', {
          msg: this.username,
          token: this.password
        });

        if (response.status === 200 && response.data.token) {
          alert('Login successful!');
          // Store the token in localStorage or Vuex store
          localStorage.setItem('token', response.data.token);
          // Redirect to the dashboard or another page
          this.$router.push('/dashboard');
        } else {
          this.loginError = response.data.msg || 'Login failed, please try again.';
        }
      } catch (error) {
        if (error.response && error.response.status === 401) {
          this.loginError = error.response.data.msg || 'Login failed, please try again.';
        } else {
          console.error(error);
          this.loginError = 'An error occurred during login. Please try again later.';
        }
      }
    }
  },

      name:"Login"
  }
  
</script>
<style scoped>
html, body {
  height: 100%;
}
.container {
  height: 980px;
  width: 100%;
  background-image: linear-gradient(to right, #e1c1e4, white);
}
.login-wrapper {
  background-color: #fff;
  width: 360px;
  height: 500px;
  border-radius: 15px;
  padding: 0 50px;
  position: absolute;
  border: 1px solid #ccc;
  left: 50%;
  top: 50%;
  transform: translate(-50%, -50%);
}
.header {
  font-size: 36px;
  color: #bb8bbe;
  font-weight: bold;
  text-align: center;
  line-height: 200px;
}
.input-item {
  display: block;
  width: 100%;
  margin-bottom: 25px;
  padding: 12px;
  border: 1px solid rgb(128, 125, 125);
  border-radius: 10px;
  font-size: 15px;
  outline: none;
}
.input-item:placeholder {
  text-transform: uppercase;
}
.btn2 {
  text-align: center;
  padding: 8px;
  width: 100%;
  margin-top: 40px;
  background-color: #c99fcb;
  color: #fff;
  border-radius: 10px;
  cursor: pointer;
  font-size: 20px;
}
.btn2:hover {
  background-color: #bb8bbe;
}
.btn2:active {
  position: relative;
  top: 1px;
}
.error-message {
  color: red;
  text-align: center;
  margin-top: 10px;
}
</style>
