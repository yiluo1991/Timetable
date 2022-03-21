<template>
<div>
  <ElTable :data="tableData" highlight-current-row row-key="Id">
    <ElTableColumn :width="180" prop="Mobile" label="手机号（登录账号）"></ElTableColumn>
    <ElTableColumn :width="180" prop="RealName" label="真实姓名"></ElTableColumn>
    <ElTableColumn :width="180" prop="IP" label="IP地址"> </ElTableColumn>
    <ElTableColumn :width="180" prop="LoginTime" label="记录时间"><template v-slot="{row}">
      {{row.LoginTime|datetime}}
      </template> </ElTableColumn>
    <ElTableColumn prop="Remark" label="记录内容"> </ElTableColumn>
  </ElTable>
   <BasicPaginationBar :page.sync="page" :size.sync="size" :total="total" @current-change="reload" @size-change="reload"></BasicPaginationBar>
   
</div>
</template>
<script>
export default {
  data() {
    return {
      tableData: [],
      total: 0,
      searchOptions: {
        keyword: "",
      },
      page: 1,
      size: 15, 
    };
  },
   inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("UL_G");
    }
  },
  mounted() {
    this.reload();
  },
  methods: {
    reload(e) {
      if (!this.hasGetPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.searchOptions = e;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/UserLoginLog/list";
        if (url) {
          var data = {
            keyword: this.searchOptions.keyword,
            page: this.page,
            rows: this.size,
          };

          if (this.searchOptions.dataRange && this.searchOptions.dataRange.length > 0) {
            data.datestart = this.searchOptions.dataRange[0];
            data.dateend = this.searchOptions.dataRange[1];
          }

          this.$axios.post(url, data).then((res) => {
            setTimeout(() => {
              this.$loading().close();
            }, 100);

            this.tableData = res.data.data; 
            this.total = res.data.total;
          });
        }
      });
    },
  },
};
</script>
