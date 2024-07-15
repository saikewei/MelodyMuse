// store/configure.js
const configure = {
    namespaced: true, // ÆôÓÃÃüÃû¿Õ¼ä
    state: {
        HOST: 'http://127.0.0.1:8888',
        activeName: ''
    },
    getters: {
        activeName: state => {
            let activeName = state.activeName
            if (!activeName) {
                activeName = JSON.parse(window.sessionStorage.getItem('activeName'))
            }
            return activeName
        }
    },
    mutations: {
        setActiveName: (state, activeName) => {
            state.activeName = activeName
            window.sessionStorage.setItem('activeName', JSON.stringify(activeName))
        }
    }
}

export default configure


