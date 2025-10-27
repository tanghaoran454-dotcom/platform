<template>
  <div class="dashboard">
    <h2>数据大屏</h2>
    <el-row :gutter="20" class="kpi-cards">
      <el-col :span="6">
        <el-card class="kpi-card">
          <div class="kpi-item">
            <div class="kpi-value">{{ statistics.taskCount }}</div>
            <div class="kpi-label">任务总数</div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="kpi-card">
          <div class="kpi-item">
            <div class="kpi-value">{{ statistics.alarmCount }}</div>
            <div class="kpi-label">告警数量</div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="kpi-card">
          <div class="kpi-item">
            <div class="kpi-value">{{ statistics.deviceCount }}</div>
            <div class="kpi-label">在线设备</div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="kpi-card">
          <div class="kpi-item">
            <div class="kpi-value">{{ statistics.completionRate }}%</div>
            <div class="kpi-label">完成率</div>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'

const statistics = ref({
  taskCount: 0,
  alarmCount: 0,
  deviceCount: 0,
  completionRate: 0,
})

onMounted(() => {
  // 模拟数据
  statistics.value = {
    taskCount: 156,
    alarmCount: 23,
    deviceCount: 8,
    completionRate: 87.5,
  }
})
</script>

<style scoped>
.dashboard {
  padding: 0;
}

.kpi-cards {
  margin-top: 24px;
}

.kpi-card {
  cursor: pointer;
  transition: all 0.3s;
}

.kpi-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.12);
}

.kpi-item {
  text-align: center;
}

.kpi-value {
  font-size: 36px;
  font-weight: 600;
  color: #165DFF;
  margin-bottom: 8px;
}

.kpi-label {
  font-size: 14px;
  color: #86909c;
}
</style>

