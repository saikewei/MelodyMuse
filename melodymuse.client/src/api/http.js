import axios from 'axios';

// 创建一个 axios 实例
const apiClient = axios.create({
  baseURL: 'https://localhost:7223', // 替换为你的 API 基础 URL
  timeout: 1000,
});

// 请求拦截器
apiClient.interceptors.request.use(config => {
  const token = localStorage.getItem('token'); // 假设你的 token 存储在 localStorage 中
  if (token) {
    config.headers.Authorization = `Bearer ${token}`; // 将 token 添加到 Authorization 头部
  }
  return config;
}, error => {
  return Promise.reject(error);
});

// 创建不需要 token 的 Axios 实例
const apiClientWithoutToken = axios.create({
  baseURL: 'https://localhost:7223',
  timeout: 1000,
});

export default {apiClient,apiClientWithoutToken};

