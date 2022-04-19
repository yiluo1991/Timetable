<template>
  <div>
    <ElTable :data="tableData" highlight-current-row row-key="Id">
      <ElTableColumn prop="Title" label="标题" show-overflow-tooltip>
         
      </ElTableColumn>
      <ElTableColumn prop="Content" label="内容" show-overflow-tooltip>
       
      </ElTableColumn>
       <ElTableColumn prop="SN" label="排序号" width="100" show-overflow-tooltip>
       
      </ElTableColumn>
        <ElTableColumn prop="CreateTime" label="创建时间" width="170" show-overflow-tooltip>
          
      </ElTableColumn>
      <ElTableColumn prop="Enable" label="启用" width="100">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.Enable" type="success">启用</el-tag>
          <el-tag v-if="!scope.row.Enable" type="danger">禁用</el-tag>
        </template>
      </ElTableColumn>
      <ElTableColumn prop="edit" label="操作" width="180">
        <template v-slot="{ row }">
          <ElButton
            v-if="hasEditPermission"
            round
            size="small"
            icon="el-icon-edit"
            plain
            type="warning"
            @click="edit(row)"
            >修改</ElButton
          >
          <ElButton
            v-if="hasDeletePermission"
            type="danger"
            icon="el-icon-delete"
            round
            size="small"
            plain
            @click="del(row)"
            >删除</ElButton
          >
        </template>
      </ElTableColumn>
    </ElTable>
    <BasicPaginationBar
      :page.sync="page"
      :size.sync="size"
      :total="total"
      @current-change="reload"
      @size-change="reload"
    ></BasicPaginationBar>
  </div>
</template>
<script>
export default {
  data() {
    return {
      tableData: [],
      total: 0,
      keyword: "",
      page: 1,
      size: 15,
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("N_G");
    },
hasEditPermission(){
  return this.hasPermission("N_M")
},
    hasDeletePermission() {
      return this.hasPermission("N_D");
    },
  },
  mounted() {
    this.reload();
  },
  methods: {
    edit(row){
      this.$emit('edit',row)
    },
    del(row) {
      if (this.hasDeletePermission) {
        this.$confirm("确认删除该公告？", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning",
        })
          .then(() => {
            var url = this.$baseURL + "/notice/Delete?id=" + row.Id;
            this.$loading({ text: "处理中" });
            this.$axios.post(url).then((res) => {
              setTimeout(() => {
                this.$loading().close();
              }, 100);
              this.reload();
              if (!res.data.success) {
                this.$message.error(res.data.msg);
              }
            });
          })
          .catch(() => {});
      }
    },
    reload(e) {
      if (!this.hasGetPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = e;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/notice/list";
        if (url) {
          var data = {
            keyword: this.keyword,
            page: this.page,
            rows: this.size,
          };

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
