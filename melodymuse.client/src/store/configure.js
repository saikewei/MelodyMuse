const configure = {
    namespaced: true, // ���������ռ�
    state: {
        HOST: 'http://127.0.0.1:8888',
        activeName: '',
        showAside: false,               //�Ƿ���ʾ�����еĸ����б�
    },
    getters: {
        activeName: state => {
            let activeName = state.activeName;
            if (!activeName) {
                activeName = JSON.parse(window.sessionStorage.getItem('activeName'));
            }
            return activeName;
        },
        showAside: state => {
            let showAside = state.showAside
            if (!showAside) {
                showAside = JSON.parse(window.sessionStorage.getItem('showAside'))
            }
            return showAside
        },
    },
    mutations: {
        setActiveName: (state, activeName) => {
            state.activeName = activeName;
            window.sessionStorage.setItem('activeName', JSON.stringify(activeName));
        },
        setShowAside: (state, showAside) => {
            state.showAside = showAside
            window.sessionStorage.setItem('showAside', JSON.stringify(showAside))
        },
    }
}

export default configure;
