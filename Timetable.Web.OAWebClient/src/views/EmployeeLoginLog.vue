<template>
  <div class="admin-page">
    <BasicToolbar :showAddButton="false"  :showRefreshButton="hasGetPermission" :showSearchbox="hasGetPermission"  @search="search" @reload="reload">
    
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
        style="margin-right:10px;"
      >
      </ElDatePicker>
    </BasicToolbar>
    <EmployeeLoginLogTable    ref="table"/>
  </div>
</template>
<script>  
import EmployeeLoginLogTable from  "../components/EmployeeLoginLog/EmployeeLoginLogTable.vue";
export default { 
  components: { 
    EmployeeLoginLogTable,
  },
  name: "adminemployeeloginlog",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("EL_G");
    }
  },
  data() {
    return {
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
      dataRange: null,
    };
  },
  methods: {
    search(e) {
      this.$refs.table.reload({
        keyword:e,
        dataRange:this.dataRange
      });
    },
    reload() {
      this.$refs.table.reload();
    },
    /**打开添加窗口 */
    openAdd() {},
    /**打开修改窗口 */
    openEdit(e) {},
  },
};
</script>
