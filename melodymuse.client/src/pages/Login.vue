<template>
  <div id="app">
    <login_comp />
    <div class="title">
        <div class="forget-password" @click="$router.push('/ForgottenPassword')">忘记密码 →</div>
        <div class="btn-register" @click="$router.push('/register')">还未注册，点击注册</div>
    </div>
  </div>
</template>
 
<script>
    //避免页面和组件重名导致的递归调用，改为_comp后缀
import login_comp from '../components/login.vue'
export default {
  name: 'Login',
  components: {
    login_comp
  }
}
</script>
 
<style>
.title{
    text-align: center;
    background-color:transparent;
}

.btn-login {  
    position:absolute;
    top:69%; 
    left:29%;
    background-color: transparent;
    border-radius:10px;
    display:inline-block;
    cursor:pointer;
    color:#6a8cdcc1;
    font-family:Arial;
    font-size:20px;
    padding:16px 31px;
    text-decoration:none;
    margin: 10px 20px;  
}
.btn-login:hover {
    color:#284da0c1;
}
.btn-login:active {
    color:#284da0c1;
}
.btn-register {   
    position:absolute;
    top:69%; 
    left:28%;
    background-color: transparent;
    border-radius:10px;
    display:inline-block;
    cursor:pointer;
    color:#6a8cdcc1;
    font-family:Arial;
    font-size:20px;
    padding:16px 31px;
    text-decoration:none;
    margin: 10px 20px;  
}
.btn-register:hover {
    color:#284da0c1;
}
.btn-register:active {
    color:#284da0c1;
}
.forget-password {  
    position:absolute;
    top:65%; 
    left:37%;
    background-color: transparent;
    border-radius:15px;
    display:inline-block;
    cursor:pointer;
    color:grey;
    font-family:Arial;
    font-size:15px;
    padding:16px 31px;
    text-decoration:none;
    margin: 10px 20px;  
}
.forget-password:hover {
    color:black;
}
.forget-password:active {
  color:black;
}
</style>