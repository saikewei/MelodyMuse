<template>
  <div class="container">
          <div class="login-wrapper">
              <div class="header">MelodyMuse</div>
              <div class="form-wrapper">
                  <input type="tel" name="phonenumber" placeholder="手机号码" class="input-item">               
                  <input type="password" name="password" placeholder="密码" class="input-item">
                  <input type="password" name="repassword" placeholder="再次确认密码" class="input-item">
                  <div class="btn2">Register</div>
              </div>
          </div>
      </div>
</template>
  
<script>
  export default {
      name:"Reg"
  }
</script>

<style scoped>
html {
  height: 100%;
}
body {
  height: 100%;
}
.container {
  /* margin-top: 5%; */
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
  color:#bb8bbe;
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
.input-item:placeholder {
  text-transform: uppercase;
}
.btn2 {
  text-align: center;
  padding: 8px;
  margin: 0 auto;
  width: 100%;
  margin-top: 25px;
  background-color: #c99fcb;
  color: #fff;
  border-radius:10px;
  display:inline-block;
  cursor:pointer;
  font-family:Arial;
  font-size:20px;
  text-decoration:none;
}
.btn2:hover {
    background-color:#bb8bbe;
}
.btn2:active {
    position:relative;
    top:1px;
}
.msg {
  text-align: center;
  line-height: 88px;
}
a {
  text-decoration-line: none;
  color:#bb8bbe;
}

</style>