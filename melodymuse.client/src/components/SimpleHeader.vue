<template>
    <nav class="the-header">
        <div class="header-logo" @click="goHome">
            <img :src="Images.logoUrl" alt="Home Icon" style="height: 40px; width: 40px;">
            <img :src="Images.nameUr1" alt="Home Icon" style="height: 40px; width: 90px;">
            {{ currentPage }}
        </div>
    </nav>
</template>

<script>
    import { mapGetters } from 'vuex'
    import logoUrl from '../assets/logo2.jpg'
    import nameUr1 from '../assets/name1.jpg'
    import { navMsg } from '../assets/data/header'

    export default {
        props: ["currentPage"],
        data() {
            return {
                Images: {
                    logoUrl,nameUr1
                },
                navMsg: [],
                searchQuery: ''
            }
        },
        computed: {
            ...mapGetters('configure', ['activeName']) // ָ��ģ������
        },
        created() {
            this.navMsg = navMsg
        },
        methods: {
            goHome() {
                this.$router.push({ path: '/' })
            },
        }
    }
</script>


<style scoped>
   .the-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    padding: 0px 30px; /* �����ڱ߾��Ը����Ӿ�Ч�� */
    box-sizing: border-box;
    background-color:#ffffff;
    box-shadow: 0 4px 8px #cacaca; /* ������Ӱ��ǿ�� */
    position: fixed;
    top: 0;
    left: 0;
    z-index: 1000; /* ȷ��ͷ�������������Ϸ� */
}

.header-logo {
    display: flex;
    align-items: center;
}


.navbar {
    list-style: none;
    padding: 0;
    margin: 0;
    display: flex;
    gap: 30px; /* ���ӵ�����֮��ļ�� */
}

.navbar li {
    cursor: pointer;
    padding: 20px 20px; /* �����ڱ߾�������������� */
    position: relative;
    display: inline-block; /* ʹ���Ƚ�Ϊ���ݿ��� */
    font-weight: 500;
    color: #808080; /* ������ɫ */
    line-height: 17px; /* ����Ϊ�������ĸ߶� */
    transition: color 0.3s ease; /* ƽ����ɫ����Ч�� */
}

/* �߹�Ч�� */
    .navbar li::after {
        content: '';
        position: absolute;
        bottom: 0; /* ȷ������ײ� */
        left: 0;
        width: 0%; /* Ĭ�����ظ߹��� */
        height: 5px; /* �߹����ĸ߶� */
        background-color: #284da0c1; /* ��ɫ�߹��� */
    }

/* ����״̬����ʽ */
.navbar li.active::after {
    width: 100%; /* ����״̬�¸߹�������Ϊ�ı����� */
}

    .navbar li.active {
        color: #284da0c1; 
    }

/* ��ͣ״̬����ʽ */
    .navbar li:hover {
        color: #284da0c1;
    }

.navbar-search input {
    padding: 8px;
    border-radius: 20px; /* ����Բ�� */
    border: 1.5px solid #cacaca; 
    outline: none;
    width: 300px; /* ����������Ŀ��� */
    height: 35px; /* ����������ĸ߶� */
    transition: border-color 0.3s ease; /* ƽ���߿���ɫ����Ч�� */
}

    .navbar-search input:focus {
        border-color: #284da0c1; /* �۽�ʱ�߿���ɫ�仯 */
    }
</style>
