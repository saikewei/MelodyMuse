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

.btn {   
    background-color: transparent;
    border-radius:10px;
    display:inline-block;
    cursor:pointer;
    color:#bb8bbe;
    font-family:Arial;
    font-size:20px;
    padding:16px 31px;
    text-decoration:none;
    margin: 10px 20px;  
}
.btn:hover {
    background-color:#f9f7f9;
}
.btn:active {
    position:relative;
    top:1px;
}
</style>