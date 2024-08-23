// store/index.js
import { createStore } from 'vuex'
import configure from './configure'
import search from './search' // 导入新的搜索模块

const store = createStore({
    modules: {
        configure,
        search // 注册搜索模块
    }
})

export default store
