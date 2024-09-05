// store/index.js
import { createStore } from 'vuex'
import configure from './configure'
import search from './search' // �����µ�����ģ��
import song from './song'

const store = createStore({
    modules: {
        configure,
        search, // ע������ģ��
        song
    }
})

export default store
