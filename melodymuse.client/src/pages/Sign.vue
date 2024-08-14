<!--这个文件没有被使用！这个文件是最初登录注册在一个页面的版本，现在拆成两个页面，这个文件保留作为参考！！-->

<template>
  <div id="app">
      <component :is="msg">
    </component>
    <div class="title">
        <div class="btn" @click="msg='login'">登录</div>
        <div class="btn" @click="msg='register'">注册</div>
      </div>
  </div>
</template>
 
<script>
//这里的from路径根据自己的布局更改路径
import login from '../components/login.vue'
import register from '../components/register.vue'
export default {
  name: 'login_register',
  data(){
      return{
          msg:"login"
      }
  },
  components: {
    login,
    register
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
</style>