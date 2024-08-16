<template>
    <div>
        <div v-if="results.length">
            <ul>
                <li v-for="result in results" :key="result.id">{{ result.name || result.title }}</li>
            </ul>
        </div>
        <div v-else>
          NO
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
    ul {
        list-style: none;
        padding: 0;
    }

    li {
        padding: 8px 0;
        border-bottom: 1px solid #cacaca;
    }
</style>
