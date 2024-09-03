<template>
    <div class="home">
        <TheHeader />
        <Swiper />
        <RecommendedSongs />
    </div>
</template>

<script>
    import Swiper from "../components/Swiper.vue";
    import TheFooter from "../components/TheFooter.vue";
    import TheHeader from "../components/TheHeader.vue";
    import RecommendedSongs from "../components/RecommendedSongs.vue";

    export default {
        name: 'Home',
        components: {
            Swiper,
            TheFooter,
            TheHeader,
            RecommendedSongs
        }
    }
</script>

<style scoped>
    .home {
        min-height: 100vh; /* ȷ��ҳ������ռ���ӿڸ߶� */
        padding-bottom: 80px; /* �����㹻�ռ�� footer������Ϊ�ʺϵĸ߶� */
        box-sizing: border-box; /* ȷ�� padding ������Ԫ�ص��ܸ߶��� */
    }

    .swiper {
        position: relative;
        z-index: 1; /* ȷ���ֲ�ͼ���Ƽ��������� */
        margin-bottom: 20px; /* Ϊ�Ƽ������������ */
    }

    .recommended-songs {
        position: relative;
        z-index: 0; /* ȷ���Ƽ��������ֲ�ͼ�·� */
        margin-bottom: 60px; /* Ϊ footer �����ռ� */
    }

    .the-footer {
        position: fixed;
        bottom: 0;
        left: 50%;
        transform: translateX(-50%);
        width: 100%; /* ȷ�� footer ���ȸ��������ӿڿ��� */
        max-width: 1200px; /* �����ȣ�ȷ�� footer ������� */
        height: 70px; /* ���� footer �ĸ߶� */
        background-color: #f1f1f1; /* ������ɫ */
        z-index: 2; /* ȷ�� footer �������������� */
        text-align: center;
        line-height: 30px; /* ��ֱ���� footer �е��ı� */
        padding: 0 0px; /* ���������ڱ߾� */
        box-sizing: border-box; /* ȷ�� padding ������Ԫ�ص��ܿ����� */
    }
</style>

