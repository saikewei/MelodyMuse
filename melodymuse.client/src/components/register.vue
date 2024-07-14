<template>
  <div class="container">
    <div class="login-wrapper">
      <div class="header">MelodyMuse</div>
      <div class="form-wrapper">
        <input type="tel" name="phonenumber" placeholder="手机号码" class="input-item" v-model="username">               
        <input type="password" name="password" placeholder="密码" class="input-item" v-model="password">
        <input type="password" name="repassword" placeholder="再次确认密码" class="input-item" v-model="confirmPassword">
        <div class="btn2" @click="register">Register</div>
        <p v-if="registerError" class="error-message">{{ registerError }}</p>
      </div>
    </div>
  </div>
</template>

<script>

export default {
  data() {
    return {
      username: '',
      password: '',
      confirmPassword: '',
      registerError: ''
    };
  },
  methods: {
    async register() {
      // 验证手机号码长度和格式
      if (!/^\d{11}$/.test(this.username)) {
        this.registerError = '请输入11位有效的手机号码。';
        return;
      }

      // 验证密码是否为空
      if (this.password.trim() === '') {
        this.registerError = '密码不能为空。';
        return;
      }

      // 验证两次输入的密码是否一致
      if (this.password !== this.confirmPassword) {
        this.registerError = '两次输入的密码不一致。';
        return;
      }

      try {
        const response = await axios.post ('http://127.0.0.1:4523/m2/4804827-4459167-default/192699877', {
          msg: this.username,


          token: this.password
        });

        if (response.status === 200) {
          alert('Registration successful!');
          // Redirect to the login page
          this.$router.push('/login');
        } else {
          this.registerError = response.data.msg || 'Registration failed, please try again.';
        }
      } catch (error) {
        console.error(error);
        this.registerError = 'An error occurred during registration. Please try again later.';
      }
    }
  },
  name: "Reg"
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
  margin-bottom: 18px;
  border: 0;
  padding: 10px;
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
  margin-top: 25px;
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
.msg {
  text-align: center;
  line-height: 88px;
}
a {
  text-decoration-line: none;
  color: #bb8bbe;
}
</style>
