const song = {
    state: {
        listOfSongs: [],     //当前歌曲列表
        isPlay: false,       //是否播放中
        url: '',             //歌曲地址
        id: '',    //歌曲id
        playButtonUrl: '/play.png', //播放状态的图标
        duration: 0,         //音乐时长
        curTime: 0,          //当前音乐的播放位置
        changeTime: 0,       //指定播放时刻
        title: '',           //歌名
        artist: '',          //歌手名
        picUrl: '',          //歌曲图片
        lyric: [],           //未处理的歌词数据
        tempList: {},        //单个歌单信息或歌手信息
        listIndex: null,     //当前歌曲在歌单中的位置
        volume: 50,           //音量
        songInfoLists: []
    },
    getters: {
        listOfSongs: state => {
            let listOfSongs = state.listOfSongs;
            if (!listOfSongs.length) {
                listOfSongs = JSON.parse(window.sessionStorage.getItem('listOfSongs') || '[]');
            }
            return listOfSongs;
        },
        isPlay: state => {
            let isPlay = state.isPlay;
            if (!isPlay) {
                isPlay = JSON.parse(window.sessionStorage.getItem('isPlay') || 'false');
            }
            return isPlay;
        },
        url: state => {
            let url = state.url;
            if (!url) {
                url = JSON.parse(window.sessionStorage.getItem('url') || '""');
            }
            return url;
        },
        id: state => {
            let id = state.id;
            if (!id) {
                id = JSON.parse(window.sessionStorage.getItem('id') || '""');
            }
            return id;
        },
        playButtonUrl: state => {
            let playButtonUrl = state.playButtonUrl;
            if (!playButtonUrl) {
                playButtonUrl = JSON.parse(window.sessionStorage.getItem('playButtonUrl') || '"/play.png"');
            }
            return playButtonUrl;
        },
        duration: state => {
            let duration = state.duration;
            if (!duration) {
                duration = JSON.parse(window.sessionStorage.getItem('duration') || '0');
            }
            return duration;
        },
        curTime: state => {
            let curTime = state.curTime;
            if (!curTime) {
                curTime = JSON.parse(window.sessionStorage.getItem('curTime') || '0');
            }
            return curTime;
        },
        changeTime: state => {
            let changeTime = state.changeTime;
            if (!changeTime) {
                changeTime = JSON.parse(window.sessionStorage.getItem('changeTime') || '0');
            }
            return changeTime;
        },
        title: state => {
            let title = state.title;
            if (!title) {
                title = JSON.parse(window.sessionStorage.getItem('title') || '""');
            }
            return title;
        },
        artist: state => {
            let artist = state.artist;
            if (!artist) {
                artist = JSON.parse(window.sessionStorage.getItem('artist') || '""');
            }
            return artist;
        },
        picUrl: state => {
            let picUrl = state.picUrl;
            if (!picUrl) {
                picUrl = JSON.parse(window.sessionStorage.getItem('picUrl') || '""');
            }
            return picUrl;
        },
        lyric: state => {
            let lyric = state.lyric;
            if (!lyric) {
                lyric = JSON.parse(window.sessionStorage.getItem('lyric') || '[]');
            }
            return lyric;
        },
        tempList: state => {
            let tempList = state.tempList;
            if (!tempList) {
                tempList = JSON.parse(window.sessionStorage.getItem('tempList') || '{}');
            }
            return tempList;
        },
        listIndex: state => {
            let listIndex = state.listIndex;
            if (listIndex === null) {
                listIndex = JSON.parse(window.sessionStorage.getItem('listIndex') || 'null');
            }
            return listIndex;
        },
        volume: state => {
            let volume = state.volume;
            if (!volume) {
                volume = JSON.parse(window.sessionStorage.getItem('volume') || '50');
            }
            return volume;
        },
        songInfoLists: state => {
            let songInfoLists = state.songInfoLists;
            if (!songInfoLists.length) {
                songInfoLists = JSON.parse(window.sessionStorage.getItem('songInfoLists') || '[]');
            }
            return songInfoLists;
        }
    },
    mutations: {
        setListOfSongs: (state, listOfSongs) => {
            state.listOfSongs = listOfSongs;
            window.sessionStorage.setItem('listOfSongs', JSON.stringify(listOfSongs));
        },
        setIsPlay: (state, isPlay) => {
            state.isPlay = isPlay;
            window.sessionStorage.setItem('isPlay', JSON.stringify(isPlay));
        },
        setUrl: (state, url) => {
            state.url = url;
            window.sessionStorage.setItem('url', JSON.stringify(url));
        },
        setId: (state, id) => {
            state.id = id;
            window.sessionStorage.setItem('id', JSON.stringify(id));
        },
        setPlayButtonUrl: (state, playButtonUrl) => {
            state.playButtonUrl = playButtonUrl;
            window.sessionStorage.setItem('playButtonUrl', JSON.stringify(playButtonUrl));
        },
        setDuration: (state, duration) => {
            state.duration = duration;
            window.sessionStorage.setItem('duration', JSON.stringify(duration));
        },
        setCurTime: (state, curTime) => {
            state.curTime = curTime;
            window.sessionStorage.setItem('curTime', JSON.stringify(curTime));
        },
        setChangeTime: (state, changeTime) => {
            state.changeTime = changeTime;
            window.sessionStorage.setItem('changeTime', JSON.stringify(changeTime));
        },
        setTitle: (state, title) => {
            state.title = title;
            window.sessionStorage.setItem('title', JSON.stringify(title));
        },
        setArtist: (state, artist) => {
            state.artist = artist;
            window.sessionStorage.setItem('artist', JSON.stringify(artist));
        },
        setPicUrl: (state, picUrl) => {
            state.picUrl = picUrl;
            window.sessionStorage.setItem('picUrl', JSON.stringify(picUrl));
        },
        setLyric: (state, lyric) => {
            state.lyric = lyric;
            window.sessionStorage.setItem('lyric', JSON.stringify(lyric));
        },
        setTempList: (state, tempList) => {
            state.tempList = tempList;
            window.sessionStorage.setItem('tempList', JSON.stringify(tempList));
        },
        setListIndex: (state, listIndex) => {
            // 更新 listIndex
            state.listIndex = listIndex;
            window.sessionStorage.setItem('listIndex', JSON.stringify(listIndex));

            // 更新 id 为对应位置的歌曲 ID
            if (state.listOfSongs && state.listOfSongs.length > 0) {
                const songAtIndex = state.listOfSongs[listIndex]; // 获取对应位置的歌曲
                if (songAtIndex && songAtIndex.songId) {
                    state.id = songAtIndex.songId; // 更新 id 为该歌曲的 ID
                    window.sessionStorage.setItem('id', songAtIndex.songId); // 同时保存到 sessionStorage
                }
            }
        },

        setVolume: (state, volume) => {
            state.volume = volume;
            window.sessionStorage.setItem('volume', JSON.stringify(volume));
        },
        setSongInfoLists: (state, songInfoLists) => {
            state.songInfoLists = songInfoLists;
            window.sessionStorage.setItem('songInfoLists', JSON.stringify(songInfoLists));
        },
        // 添加歌曲到列表
        // 添加歌曲到列表
        addSongToList: (state, songId) => {
            state.listOfSongs  = JSON.parse(window.sessionStorage.getItem('listOfSongs') || '[]');
            // 如果歌曲列表中已存在该歌曲 ID，则不再重复添加
            let list = state.listOfSongs;
            const songExists = state.listOfSongs.includes(songId);

            if (!songExists) {
                list.push(songId);
                state.listOfSongs = list;
                window.sessionStorage.setItem('listOfSongs', JSON.stringify(state.listOfSongs));

                // 更新当前播放列表的索引
                state.listIndex = state.listOfSongs.length - 1;
                window.sessionStorage.setItem('listIndex', JSON.stringify(state.listIndex));
            }
        }
    },
    actions: {
        // 添加歌曲到列表
        addSong({ commit }, song) {
            commit('addSongToList', song);
        }
    }
}

export default song;
