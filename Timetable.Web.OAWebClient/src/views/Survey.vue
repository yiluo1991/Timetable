<template>
  <div class="admin-page">

    <BasicToolbar 
    :showSearchbox="hasPermission('SV_G')" 
      :showRefreshButton="hasPermission('SV_G')"
     :showAddButton="false" @search="search" @reload="reload">
     
    <template v-if="hasPermission('SV_G')">
        <ElSelect size="medium" style="width:140px;"  v-model="publishState" placeholder="">
      <ElOption value="" label="所有审核状态"></ElOption>
      <ElOption value="0" label="从未申请"></ElOption>
      <ElOption value="1" label="待审核"></ElOption>
      <ElOption value="2" label="通过"></ElOption>
      <ElOption value="3" label="不通过"></ElOption>
      <ElOption value="4" label="用户取消"></ElOption>
    </ElSelect>
     <ElSelect   style="margin-left:10px;width:140px;" size="medium" v-model="acceptAnswer" placeholder="">
      <ElOption value="" label="所有运行状态"></ElOption>
      <ElOption value="true" label="运行中"></ElOption>
      <ElOption value="false" label="已停止"></ElOption> 
    </ElSelect>
    <ElSelect   style="margin-left:10px;width:140px;" size="medium" v-model="cid" placeholder="">
      <ElOption value="-1" label="所有访问权值"></ElOption>
      <ElOption value="0" label="无权值"></ElOption>
      <ElOption value="1" label="有权值"></ElOption> 
    </ElSelect>
      <ElDatePicker
        v-model="dataRange"
        type="daterange"
        align="right"
        unlink-panels
        range-separator="至"
        start-placeholder="开始日期"
        end-placeholder="结束日期"
        :picker-options="pickerOptions"
        size="medium"
        value-format="yyyy-MM-dd"
        style="margin-right:10px;width:240px;vertical-align: top;margin-left:10px;"
      >
      </ElDatePicker>
    </template>
   <template #append>
      <ElButton @click="showTaskDialog" type="info" style="float:right;" size="medium" round plain  v-if="hasPermission('ST_G')">数据同步任务列表</ElButton>
   </template>
    </BasicToolbar>
    <SurveyTable ref="table" @showDialog="showDialog"></SurveyTable>
    <SyncTaskDialog ref="taskDialog"></SyncTaskDialog>
  </div>
</template>
<script>
import SurveyTable from "../components/Survey/SurveyTable"; 
import SyncTaskDialog from '../components/Survey/SyncTaskDialog.vue'
export default {
  name: "adminsurvey",
  data() {
    return {
      dataRange: null,
      pickerOptions: {
        shortcuts: [
          {
            text: "最近一周",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "最近一个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "最近三个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              picker.$emit("pick", [start, end]);
            },
          },
        ],
      },
      publishState: "",
      acceptAnswer:'',
      cid:"-1"
    };
  },
  components: {
   SurveyTable,
   SyncTaskDialog
  },
  inject: ["hasPermission"],
  methods: {
    search(e) {
      this.$refs.table.reload({
        keyword:e,
        publishState:this.publishState,
        acceptAnswer:this.acceptAnswer,
        dataRange:this.dataRange,
        cid:this.cid
      });
    },
    reload() {
      this.$refs.table.reload();
    },
    showDialog(data) {
      data.originList = JSON.parse(data.originList);
      this.$refs.dialog.showDialog(data);
    },
    showTaskDialog(){
      this.$refs.taskDialog.showDialog()
    }
  },
};
</script>
