<template>
    <div>
      <TheHeader />
      <div class="header">
        <p class="header-text">你关注的歌手将展示在这里</p>
        <button class="middle-button" @click="goToFollowedArtists">click here→</button>
      </div>
      <div class="filters">
        <div class="alphabet-filter">
          <div class="filter-wrapper">
            <button class="filter-button" @click="filterAll">全部</button>
            <button style="margin-left: 10px;" v-for="letter in alphabet" :key="letter" @click="filterByLetter(letter)">
              {{ letter }}
            </button>
          </div>
        </div>
        <div class="gender-filter">
          <div class="filter-wrapper">
            <button class="filter-button" @click="filterAll">全部</button>
            <button @click="filterByGender('male')">男</button>
            <button style="margin-left: 10px;"@click="filterByGender('female')">女</button>
          </div>
        </div>
      </div>
      <div class="top-artists">
        <div
          v-for="artist in topArtists"
          :key="artist.artistId"
          class="artist"
          @click="goToArtistPage(artist.artistId)"
        >
          <img :src="artist.imageUrl" alt="Artist Image" class="artist-image grayscale" />
          <div class="artist-name">{{ artist.artistName }}</div>
        </div>
      </div>
      <div class="artist-list">
        <ul>
          <li v-for="artist in remainingArtists" :key="artist.artistId">
            <a @click="goToArtistPage(artist.artistId)">{{ artist.artistName }}</a>
          </li>
        </ul>
      </div>
      <pagination
        v-if="paginationEnabled"
        :current-page="currentPage"
        :total-items="totalArtists"
        :page-size="pageSize"
        @page-changed="loadPage"
      />
      <TheFooter />
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  import TheFooter from "../components/TheFooter.vue";
  import TheHeader from "../components/TheHeader.vue";
  
  export default {
    name: "singer",
    components: {
      TheHeader,
      TheFooter,
    },
    data() {
      return {
        alphabet: "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split(""),
        artists: [],
        topArtists: [],
        remainingArtists: [],
        currentPage: 1,
        pageSize: 10,
        totalArtists: 0,
        paginationEnabled: true,
      };
    },
    methods: {
      async fetchArtists() {
        try {
          const response = await axios.get('https://apifoxmock.com/m1/4804827-4459167-default/api/artist/all');
          this.artists = response.data;
          this.sortArtistsByFans();
          this.updateArtistDisplay();
        } catch (error) {
          console.error('获取歌手信息失败:', error);
        }
      },
      sortArtistsByFans() {
        this.artists.sort((a, b) => b.artistFansNum - a.artistFansNum);
      },
      updateArtistDisplay() {
        this.topArtists = this.artists.slice(0, 10);
        this.remainingArtists = this.artists.slice(10);
        this.totalArtists = this.remainingArtists.length;
      },
      filterAll() {
        this.fetchArtists();
      },
      filterByLetter(letter) {
        this.artists = this.artists.filter((artist) =>
          artist.artistName.startsWith(letter)
        );
        this.updateArtistDisplay();
      },
      filterByGender(gender) {
        this.artists = this.artists.filter((artist) => artist.gender === gender);
        this.updateArtistDisplay();
      },
      goToFollowedArtists() {
        this.$router.push({ name: "FollowedArtists" });
      },
      goToArtistPage(artistId) {
        this.$router.push({ name: "SingerDetail", params: { id: artistId } });
      },
      loadPage(page) {
        this.currentPage = page;
      },
    },
    mounted() {
      this.fetchArtists();
    },
  };
  </script>
  
  <style scoped>
  .header {
    height: 220px;
    background-color: #284da0c1;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    margin-bottom: 20px;
  }
  
  .header-text {
    color: aliceblue;
    font-size: 30px;
    margin-top: 45px;
    text-align: center;
  }
  
  .middle-button {
    background-color: transparent;
    padding: 10px 20px;
    font-size: 16px;
    cursor: pointer;
    color: aliceblue;
    border: none;
    border-radius: 10px;
    margin-top: 10px;
    transition: background-color 0.3s ease;
  }
  
  .middle-button:hover {
    background-color: #d3ddf4c1;
    color: black;
  }
  
  .middle-button:active {
    background-color: #8b9dcbc1;
    color: black;
  }
  
  .filters {
    display: flex;
    flex-direction: column;
    align-items: center; /* 第一行居中 */
    margin-bottom: 20px;
  }
  
  .filter-wrapper {
    width: 100%;
    display: flex;
    justify-content: flex-start; /* 左端对齐 */
    margin-bottom: 10px;
  }
  
  .filter-button {
    margin-right: 15px; /* 增大按钮间距 */
    padding: 5px 10px;
    cursor: pointer;
    background-color: transparent; /* 去除底色 */
    border: none; /* 去除边框 */
    color: #333; /* 设置文字颜色 */
    font-size: 16px; /* 调整字体大小 */
    transition: color 0.3s ease, transform 0.2s ease; /* 增加平滑过渡效果 */
  }
  
  .filter-button:hover {
    color: #007bff; /* 鼠标悬停时的文字颜色 */
    transform: scale(1.1); /* 鼠标悬停时的放大效果 */
  }
  
  .filter-button:active {
    color: #0056b3; /* 鼠标按下时的文字颜色 */
    transform: scale(0.9); /* 鼠标按下时的缩小效果 */
  }
  
  .top-artists {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20px;
  }
  
  .artist {
    cursor: pointer;
    text-align: center;
  }
  
  .artist-image.grayscale {
    filter: grayscale(100%);
  }
  
  .artist-list ul {
    list-style-type: none;
    padding: 0;
  }
  
  .artist-list li {
    margin-bottom: 10px;
  }
  
  .artist-list a {
    cursor: pointer;
    text-decoration: underline;
  }
  
  .pagination {
    display: flex;
    justify-content: center;
    margin-top: 20px;
  }
  </style>
  