<template>
  <div>
    <ElTable
      ref="table"
      :data="list"
      highlight-current-row
      row-key="Id"
      default-expand-all
      :row-class-name="rowClass"
      :empty-text="getPermission?'暂无数据':'无权访问'"
    >
      <ElTableColumn prop="DisplayName" label="权限页/项名"></ElTableColumn>
      <ElTableColumn prop="Key" label="标志名"></ElTableColumn>
      <ElTableColumn prop="Url" label="权限页/权限路径"></ElTableColumn>
      <ElTableColumn prop="SN" label="排序号" width="100"></ElTableColumn>
      <ElTableColumn v-if="editPermission||deletePermission" prop="Weight" label="操作" width="185">
        <template slot-scope="scope" v-if="scope.row.Id>0">
          <ElButton
            @click="showEditDialog(scope.row)"
            round
            size="small"
            v-if="editPermission"
            icon="el-icon-edit"
            plain
            type="warning"
          >修改</ElButton>
          <ElButton
            type="danger"
            icon="el-icon-delete"
            round
            v-if="deletePermission"
            size="small"
            plain
            @click="deleteitem(scope.row)"
          >删除</ElButton>
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
<style lang="less">
.gray-row {
  td {
    background: #f9f9f9 !important;
  }
}
</style>
<script>
export default {
  inject: ["hasPermission"],
  data() {
    return {
      keyword: "",
      list: [],
      page: 1,
      size: 15,
      total: 0
     
    };
  },
  computed: {
      deletePermission() {
      return this.hasPermission("PL_D");
    },
    editPermission() {
      return this.hasPermission("PL_M");
    },
    getPermission() {
      return this.hasPermission("PL_G");
    },
  },
  mounted() {
    this.reload();
  },
  methods: {
    rowClass: function ({ row }) {
      if (row.Id < 0) return "gray-row";
    },
    reload(keyword) {
      if(!this.getPermission)return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = keyword;
          this.page=1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = "/PermissionLine/list";
        if (url) {
          this.$axios
            .post(this.$baseURL + url, {
              keyword: this.keyword,
              page: this.page,
              rows: this.size,
            })
            .then((res) => {
              setTimeout(() => {
                this.$loading().close();
              }, 100);
              this.list = res.data.data;
              this.total = res.data.total;
            });
        }
      });
    },
    showEditDialog(row) {
      if(!this.editPermission)return;
      //修改，利用JSON创建数据副本给编辑框修改，避免修改过程中直接修改到表格内容
      this.$emit("onOpenEdit", JSON.parse(JSON.stringify(row)));
    },
    deleteitem(row) {
      if(!this.deletePermission)return;
      if (row.children.length > 0) {
        this.$message({
          message: "该权限页下有子权限页，请先删除子权限页",
          type: "warning",
        });
        return;
      }
      this.$confirm("此操作将永久删除该权限页, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        this.$axios.post(this.$baseURL + "/permissionline/delete", "id=" + row.Id)
          .then((res) => {
          
              //执行删除
              this.$message({
                type: res.data.success?"success":"error",
                message: res.data.msg,
              });
          
            this.reload();
          });
      });
    },
  },
};
</script>