import { createRouter, createWebHashHistory } from "vue-router"
 
import login from "./components/login.vue"
import register from "./components/register.vue"
//添加页面时需要在这里导入组件



const router = createRouter({
    history: createWebHashHistory(),
    routes: [
        { path: '/login', component: login },
        { path: '/register', component: register },
        //添加页面时需要在这里创建对象
    ]
})
 
export default router