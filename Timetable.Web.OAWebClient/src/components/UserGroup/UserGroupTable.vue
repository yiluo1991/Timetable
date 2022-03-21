<template>
  <ElTable :data="tableData" highlight-current-row row-key="Id">
    <ElTableColumn prop="Id" label="ID" width="100" sortable> </ElTableColumn>
    <ElTableColumn prop="Name" label="用户组" sortable> </ElTableColumn>
    <ElTableColumn prop="IsDefault" label="默认权限组" width="150">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.IsDefault" type="success" > <i class="el-icon-star-on"></i> 默认权限组</el-tag>
          <!-- <el-tag v-else type="info">否</el-tag> -->
        </template>
      </ElTableColumn>
      <ElTableColumn prop="SortNum" label="排序号" sortable width="150"> </ElTableColumn>
    <ElTableColumn prop="PermissionLevel" label="权值" sortable width="150"></ElTableColumn>
    <ElTableColumn prop="SortNum" label="注册选项" sortable width="200"> 
        <template v-slot="{row}">
          <el-tag v-if="row.Registrable" type="success">开放注册</el-tag>
          <el-tag v-else type="info">不开放注册</el-tag>
        </template>
    </ElTableColumn>
    <ElTableColumn v-if="hasEditPermission || hasDeletePermission" prop="Edit" label="操作" width="185">
      <template slot-scope="scope">
        <ElButton v-if="hasEditPermission" round size="small" icon="el-icon-edit" plain type="warning" @click="showEditDialog(scope.row)">修改</ElButton>
        <ElButton v-if="hasDeletePermission" type="danger" icon="el-icon-delete" round size="small" plain @click="deleteitem(scope.row)">删除</ElButton>
      </template>
    </ElTableColumn>
  </ElTable>
</template>
<script>
export default {
  data() {
    return {
      tableData: [],
      keyword: "",
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasEditPermission() {
      return this.hasPermission("UG_M");
    },
    hasDeletePermission() {
      return this.hasPermission("UG_D");
    },
    hasGetPermission() {
      return this.hasPermission("UG_G");
    },
  },
  mounted() {
    this.reload();
  },
  methods: {
    /**刷新 */
    reload: function(e) {
      if (!this.hasGetPermission) return;
      if (arguments.length > 0) {
        this.keyword = e;
      }
      var url = "/usergroup/list";
      var loading = this.$loading({ text: "正在加载表格数据" });
      if (url) {
        this.$axios
          .post(this.$baseURL + url, {
            keyword: this.keyword,
          })
          .then((res) => {
            setTimeout(() => {
              loading.close();
            }, 100);
            this.tableData = res.data.data;
          });
      }
    },
    showEditDialog(row) {
      if(!this.hasEditPermission)return;
      //修改，利用JSON创建数据副本给编辑框修改，避免修改过程中直接修改到表格内容
      this.$emit("onOpenEdit", JSON.parse(JSON.stringify(row)));
    },
    deleteitem(row) {
      if(!this.hasDeletePermission)return;
      this.$confirm("此操作将永久删除【管理员】角色, 是否继续??", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        this.$axios.post(this.$baseURL + "/usergroup/delete", "id=" + row.Id).then((res) => {
          //执行删除
          this.$message({
            type: res.data.success ? "success" : "error",
            message: res.data.msg,
          });

          this.reload();
        });
      });
    },
  },
};
</script>
<style scoped></style>
