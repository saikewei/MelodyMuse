import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'

import router from './router.js'

const app = createApp(App)
app.use(router)   // 将路由实例对象挂载到应用实例对象上
app.mount('#app')
 