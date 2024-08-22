import { createStore } from 'vuex'
import configure from './configure'

const store = createStore({
    modules: {
        configure
    }
})

export default store
