<template>
    <div class="results-container">
        <div v-if="results.length" class="results-list">
            <ul>
                <li v-for="result in results" :key="result.artistId" class="result-item">
                    <span class="artist-name">{{ result.artistName }}</span> -
                    <span class="artist-intro">{{ result.artistIntro }}</span>
                </li>
            </ul>
        </div>
        <div v-else class="no-results">
            没有找到相关结果
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            results: {
                type: Array,
                required: true
            }
        }
    }
</script>

<style scoped>
    .results-container {
        margin: 20px;
    }

    .results-list ul {
        list-style: none;
        padding: 0;
    }

    .result-item {
        padding: 10px;
        border-bottom: 1px solid #cacaca;
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .artist-name {
        font-weight: bold;
    }

    .no-results {
        padding: 20px;
        text-align: center;
        font-size: 16px;
        color: #666;
    }
</style>
