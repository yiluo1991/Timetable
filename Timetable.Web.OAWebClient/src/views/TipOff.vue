<template>
  <div class="admin-page">

    <BasicToolbar 
    :showSearchbox="hasPermission('TO_G')" 
      :showRefreshButton="hasPermission('TO_G')"
     :showAddButton="false" @search="search" @reload="reload">
     
    <template v-if="hasPermission('TO_G')">
     <ElSelect   style="margin-left:10px;width:140px;" size="medium" v-model="acceptAnswer" placeholder="">
      <ElOption value="" label="所有处理进度"></ElOption>
      <ElOption value="true" label="已完成"></ElOption>
      <ElOption value="false" label="进行中"></ElOption> 
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
    </BasicToolbar>
    <TipOffTable ref="table" @showDialog="showDialog"></TipOffTable>
 
  </div>
</template>
<script>
import TipOffTable from "../components/TipOff/TipOffTable"; 
export default {
  name: "admintipoff",
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
      acceptAnswer:''
    };
  },
  components: {
   TipOffTable
  },
  inject: ["hasPermission"],
  methods: {
    search(e) {
      this.$refs.table.reload({
        keyword:e,
        publishState:this.publishState,
        acceptAnswer:this.acceptAnswer,
        dataRange:this.dataRange
      });
    },
    reload() {
      this.$refs.table.reload();
    },
    showDialog(data) {
      data.originList = JSON.parse(data.originList);
      this.$refs.dialog.showDialog(data);
    },
  },
};
</script>
