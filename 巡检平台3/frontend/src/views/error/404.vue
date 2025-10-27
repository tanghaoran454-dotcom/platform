<template>
  <div class="error-page">
    <h1>404 - 页面未找到</h1>
    <p>抱歉，您访问的页面不存在</p>
    <el-button type="primary" @click="$router.push('/dashboard')">返回首页</el-button>
  </div>
</template>

<script setup lang="ts">
// 404页面
</script>

<style scoped>
.error-page {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

.error-page h1 {
  font-size: 48px;
  color: #1d2129;
  margin-bottom: 16px;
}

.error-page p {
  font-size: 16px;
  color: #86909c;
  margin-bottom: 24px;
}
</style>

