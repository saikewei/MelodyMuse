// store/index.js
import { createStore } from 'vuex'
import configure from './configure'
import search from './search' // 导入新的搜索模块
import song from './song'

const store = createStore({
    modules: {
        configure,
        search, // 注册搜索模块
        song
    }
})

export default store
