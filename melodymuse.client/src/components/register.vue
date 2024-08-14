<template>
  <div class="container">
    <div class="main">
      <div class="registerbox">
        <div class="registerbox-in">
          <div class="header">MelodyMuse</div>
          <div class="form-wrapper">

            <div class="input-wrapper">
              <!--<span class="iconfont icon-account"></span>-->
              <input type="tel" name="phonenumber" placeholder="手机号码" class="input-item" v-model="phoneNumber">
            </div>
            <div class="input-wrapper">
              <!--<span class="iconfont icon-key"></span>-->
              <input type="password" name="password" placeholder="密码" class="input-item" v-model="password">
            </div>
            <div class="input-wrapper">
              <!--<span class="iconfont icon-key"></span>-->
              <input type="password" name="confirmPassword" placeholder="确认密码" class="input-item" v-model="confirmPassword">
            </div>
            <div class="btn2" @click="register">Register</div> 
            
          </div>
          <p v-if="registerError" class="error-message">{{ registerError }}</p>
        </div>
      </div>
      <!-- 右侧盒子 -->
      <div class="background">
        <div class="title">欢迎来到MelodyMuse！请注册您的新账户</div>
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
        const response = await axios.post ('https://localhost:7223/api/account/register', {
          username: this.username,


          password: this.password
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
/* 保持与登录页面相同的样式 */
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
  justify-content: space-evenly;
}
.registerbox {
  display: flex;
  width: 870px;
  height: 550px;
  position: relative;
  top: 20%;
  left: 30%;
  box-shadow: 0 12px 16px 0 rgba(0, 0, 0, 0.24), 0 17px 50px 0 rgba(0, 0, 0, 0.19);
  background-image: linear-gradient(to left, #c6d6f7c1, rgba(255, 255, 255, 0.902));
}
.registerbox-in {
  width: 360px;
  border-radius: 15px;
  padding: 0 50px;
  position: absolute;
  left: 25%;
  top: 10%;
  transform: translateX(-50%);
  display: flex;
  flex-direction: column;
  align-items: center;
}
.header {
  font-size: 36px;
  color: #284da0c1;
  font-weight: bolder;
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
  justify-content: center;
  align-items: flex-end;
  background-image: url('src/assets/logo1.png');
  background-size: cover;
}
.title {
  position:absolute;
  top: 650px;
  left:820px;
  font-size: 22px;
  color: #284da0c1;
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
  box-shadow: 0 0 0 1000px rgba(255, 255, 255, 0) inset !important;
  -webkit-text-fill-color: #445b53 !important;
  transition: background-color 5000s ease-in-out 0s;
}
input:-webkit-autofill::first-line {
  font-size: 15px;
  font-weight: bold;
}
</style>
