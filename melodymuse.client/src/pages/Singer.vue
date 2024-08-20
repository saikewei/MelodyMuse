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
          <button
            class="filter-button"
            :class="{ active: activeLetter === 'all' }"
            @click="setActiveLetter('all'); filterAll()"
          >
            全部
          </button>
          <button
            class="filter-button"
            v-for="letter in alphabet"
            :key="letter"
            :class="{ active: activeLetter === letter }"
            @click="setActiveLetter(letter); filterByLetter(letter)"
          >
            {{ letter }}
          </button>
        </div>
      </div>
      <div class="gender-filter-wrapper">
        <div class="gender-filter">
          <button
            class="filter-button"
            :class="{ active: activeGender === 'all' }"
            @click="setActiveGender('all'); filterAll()"
          >
            全部
          </button>
          <button
            class="filter-button"
            :class="{ active: activeGender === 'male' }"
            @click="setActiveGender('male'); filterByGender('male')"
          >
            男
          </button>
          <button
            class="filter-button"
            :class="{ active: activeGender === 'female' }"
            @click="setActiveGender('female'); filterByGender('female')"
          >
            女
          </button>
        </div>
      </div>
    </div>
    <div class="artist-list">
      <ul>
        <li v-for="artist in sortedArtists" :key="artist.artistId" class="artist-item">
          <a @click="goToArtistPage(artist.artistId)">
            {{ artist.artistName }}
          </a>
        </li>
      </ul>
    </div>
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
      artists: [
        { artistId: 1, artistName: "Adele", gender: "female", artistFansNum: 1200 },
        { artistId: 2, artistName: "Beyoncé", gender: "female", artistFansNum: 1500 },
        { artistId: 3, artistName: "Bruno Mars", gender: "male", artistFansNum: 1400 },
        { artistId: 4, artistName: "Coldplay", gender: "male", artistFansNum: 1800 },
        { artistId: 5, artistName: "Drake", gender: "male", artistFansNum: 2000 },
        { artistId: 6, artistName: "Ed Sheeran", gender: "male", artistFansNum: 1700 },
        { artistId: 7, artistName: "Eminem", gender: "male", artistFansNum: 2200 },
        { artistId: 8, artistName: "Katy Perry", gender: "female", artistFansNum: 1300 },
        { artistId: 9, artistName: "Lady Gaga", gender: "female", artistFansNum: 1600 },
        { artistId: 10, artistName: "Madonna", gender: "female", artistFansNum: 1900 },
        { artistId: 11, artistName: "Michael Jackson", gender: "male", artistFansNum: 2500 },
        { artistId: 12, artistName: "Taylor Swift", gender: "female", artistFansNum: 2300 },
        { artistId: 13, artistName: "The Weeknd", gender: "male", artistFansNum: 2100 },
        { artistId: 14, artistName: "Ariana Grande", gender: "female", artistFansNum: 1400 },
        { artistId: 15, artistName: "Shakira", gender: "female", artistFansNum: 1200 },
      ],
      activeLetter: 'all', // 默认激活“全部”字母
      activeGender: 'all', // 默认激活“全部”性别
    };
  },
  computed: {
    // 按照粉丝数排序
    sortedArtists() {
      return this.artists.sort((a, b) => b.artistFansNum - a.artistFansNum);
    }
  },
  methods: {
    // 获取歌手信息
    async fetchArtists() {
      try {
        const response = await axios.get('https://apifoxmock.com/m1/4804827-4459167-default/api/artist/all');
        this.artists = response.data;
      } catch (error) {
        console.error('获取歌手信息失败:', error);
      }
    },
    filterAll() {
      this.fetchArtists();
    },
    // 根据首字母过滤
    filterByLetter(letter) {
      this.artists = this.artists.filter((artist) =>
        artist.artistName.startsWith(letter)
      );
    },
    // 根据性别过滤
    filterByGender(gender) {
      this.artists = this.artists.filter((artist) => artist.gender === gender);
    },
    // 设置激活的筛选按钮
    setActiveLetter(letter) {
      this.activeLetter = letter;
    },
    setActiveGender(gender) {
      this.activeGender = gender;
    },
    // 跳转到关注的歌手
    goToFollowedArtists() {
      this.$router.push({ name: "FollowedArtist" });
    },
    // 跳转到艺术家详情
    goToArtistPage(artistId) {
      this.$router.push({ name: "SingerDetail", params: { id: artistId } });
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
  font-size: 20px;
  cursor: pointer;
  color: aliceblue;
  border: none;
  border-radius: 10px;
  margin-top: 10px;
  transition: background-color 0.3s ease;
}

.middle-button:hover {
  background-color: #d3ddf4c1;
  color: #284da0c1;
}

.middle-button:active {
  background-color: #8b9dcbc1;
  color: #284da0c1;
}

.filters {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  margin-top: 50px;
  margin-bottom: 20px;
  margin-left: 80px;
}

.filter-wrapper {
  width: 100%;
  display: flex;
  justify-content: flex-start;
  margin-bottom: 10px;
}

.gender-filter-wrapper {
  width: 100%; /* Ensure the width is the same as the alphabet filter */
  display: flex;
  margin-top: 10px;
  justify-content: flex-start; /* Align the buttons to the left */
}

.filter-button {
  margin-right: 1px;
  padding: 5px 10px;
  cursor: pointer;
  background-color: transparent;
  border: none;
  color: #333;
  font-size: 16px;
  transition: color 0.3s ease, transform 0.2s ease;
}

.filter-button.active {
  background-color: #284da0c1; /* 激活状态下的背景色 */
  color: white; /* 激活状态下的文字颜色 */
}

.filter-button:hover {
  color: #8b9dcbc1;
}

.artist-list {
  height: 400px; /* 调整高度，根据需求更改 */
  overflow-y: scroll; /* 添加滚动条 */
  margin-top: 10px;
  padding-right: 5px; /* 留出空间给滚动条 */
}

.artist-list ul {
  display: flex;
  flex-wrap: wrap;
  justify-content: left;
  padding: 0;
  list-style-type: none;
}

.artist-item {
  flex: 0 1 calc(25%); /* 每行 4 个，间隔 10px */
  margin-bottom: 10px;
  margin-top: 10px;
  text-align: center;
  background-color:#fafafaf5;
  height:30px;
}

.artist-item a {
  cursor: pointer;
  color: #333;
  font-size:medium;
}

.artist-item a:hover {
  text-decoration: underline;
  background-color: transparent;
}

.artist-item a:active {
  color: #8b9dcbc1;
  background-color: transparent;
}

::-webkit-scrollbar {
  width: 6px;
}

::-webkit-scrollbar-thumb {
  background-color: #c4c4c4;
  border-radius: 10px;
}

::-webkit-scrollbar-thumb:hover {
  background-color: #999;
}
</style>
