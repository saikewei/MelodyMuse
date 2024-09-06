<template>
    <div class="swiper">
        <div class="carousel-container" :style="{ transform: `translateX(-${currentIndex * 100}%)` }">
            <div class="carousel-item" v-for="(slide, index) in slides" :key="index">
                <a :href="slide.link">
                    <router-link :to="`/mediaplayer/${slide.songId}/${slide.songList}`">
                        <img :src="slide.src" :alt="slide.alt">
                    </router-link>
                </a>
            </div>
        </div>
        <div class="carousel-controls">
            <button @click="prevSlide" class="arrow left">&#9664;</button>
            <button @click="nextSlide" class="arrow right">&#9654;</button>
        </div>
        <div class="page-indicator">
            <span v-for="(slide, index) in slides" :key="index"
                  class="page-dot"
                  :class="{ active: index === currentIndex }"
                  @click="showSlide(index)">
            </span>
        </div>
    </div>
</template>

<script>
    import { slides } from "../assets/data/swiper";

    export default {
        name: 'Swiper',
        data() {
            return {
                currentIndex: 0,
                slides: [],
                autoPlayInterval: null,
                intervalDuration: 3000
            };
        },
        created() {
            this.slides = slides;
        },
        methods: {
            showSlide(index) {
                this.currentIndex = (index + this.slides.length) % this.slides.length;
            },
            nextSlide() {
                this.showSlide(this.currentIndex + 1);
            },
            prevSlide() {
                this.showSlide(this.currentIndex - 1);
            },
            startAutoPlay() {
                this.autoPlayInterval = setInterval(this.nextSlide, this.intervalDuration);
            },
            stopAutoPlay() {
                clearInterval(this.autoPlayInterval);
            }
        },
        mounted() {
            this.startAutoPlay();
        },
        beforeDestroy() {
            this.stopAutoPlay();
        }
    };
</script>

<style scoped>
    .swiper {
        margin-top: 20px;
        position: relative;
        width: 90vw;
        height: 70vh;
        overflow: hidden;
        z-index: 1;
    }

    .carousel-container {
        display: flex;
        transition: transform 1.0s ease;
        width: 100%;
        height: 100%;
    }

    .carousel-item {
        flex: 0 0 100%;
        height: 100%;
        display: flex;
        align-items: center; /* 垂直居中 */
        justify-content: center; /* 水平居中 */
    }

        .carousel-item img {
            width: 1000px;
            height: 500px; /* 让图片高度填满容器 */
            object-fit: cover; /* 保持图片比例，裁剪多余部分 */
        }

    .carousel-controls {
        position: absolute;
        top: 50%;
        width: 100%;
        display: flex;
        justify-content: space-between;
        transform: translateY(-50%);
    }

        .carousel-controls button {
            background: rgba(0, 0, 0, 0.5);
            color: white;
            border: none;
            padding: 10px;
            cursor: pointer;
            font-size: 18px;
            outline: none;
        }

        .carousel-controls .arrow.left {
            left: 50px;
        }

        .carousel-controls .arrow.right {
            right: 50px;
        }

    .page-indicator {
        position: absolute;
        bottom: 10px;
        left: 50%;
        transform: translateX(-50%);
        display: flex;
    }

    .page-dot {
        width: 10px;
        height: 10px;
        background-color: #ccc;
        border-radius: 50%;
        margin: 0 5px;
        cursor: pointer;
    }

        .page-dot.active {
            background-color: #555;
        }
</style>

