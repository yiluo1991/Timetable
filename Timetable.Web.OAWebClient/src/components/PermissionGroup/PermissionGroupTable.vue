<template>
  <div>
    <ElTable
      style="margin-bottom:20px;"
      ref="table"
      :data="list"
      highlight-current-row
      row-key="Id"
      default-expand-all
      :empty-text="getPermission ? '暂无数据' : '无权访问'"
    >
      <ElTableColumn prop="DisplayName" label="权限页名称">
        <template v-slot:default="{ row }">
          <i v-if="row.Headshot" :class="row.Headshot"></i>
          {{ row.DisplayName }}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="Key" label="标志名"></ElTableColumn>
      <ElTableColumn prop="Url" label="权限页路径"></ElTableColumn>
      <ElTableColumn prop="Headshot" label="小图标类名"></ElTableColumn>
      <ElTableColumn prop="SN" label="排序号" width="100"></ElTableColumn>
      <ElTableColumn v-if="editPermission || deletePermission" prop="Weight" label="操作" width="185">
        <template slot-scope="scope">
          <ElButton
            @click="showEditDialog(scope.row)"
            round
            size="small"
            icon="el-icon-edit"
            plain
            type="warning"
            v-if="editPermission"
            >修改</ElButton
          >
          <ElButton
            type="danger"
            icon="el-icon-delete"
            round
            size="small"
            plain
            v-if="deletePermission"
            @click="deleteitem(scope.row)"
            >删除</ElButton
          >
        </template>
      </ElTableColumn>
    </ElTable>
  </div>
</template>
<script>
export default {
  data() {
    return {
      keyword: "",
      list: []
    };
  },
  computed: {
    deletePermission() {
      return this.hasPermission("PG_D");
    },
    editPermission() {
      return this.hasPermission("PG_M");
    },
    getPermission() {
      return this.hasPermission("PG_G");
    },
  },
  mounted() {
    this.reload();
  },
  inject: ["hasPermission"],
  methods: {
    reload(keyword) {
      if (arguments.length == 1) {
        this.keyword = keyword;
      }
      if (this.getPermission) {
        var url = "/permissiongroup/list";
        if (url) {
          var loading = this.$loading({ text: "正在加载表格数据" });
          this.$axios
            .post(this.$baseURL + url, {
              keyword: this.keyword,
            })
            .then((res) => {
              setTimeout(() => {
                loading.close();
              }, 100);
              this.list = res.data.data;
            });
        }
      } else {
        this.list = [];
      }
    },
    showEditDialog(row) {
      //修改，利用JSON创建数据副本给编辑框修改，避免修改过程中直接修改到表格内容
      this.$emit("onOpenEdit", JSON.parse(JSON.stringify(row)));
    },
    deleteitem(row) {
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
        this.$axios.post(this.$baseURL + "/permissiongroup/delete", "id=" + row.Id).then((res) => {
          if (res.data.success) {
            //执行删除
            this.$message({
              type: "success",
              message: "删除成功!",
            });
          } else {
            this.$message({
              type: "error",
              message: res.data.msg,
            });
          }
          this.reload();
        });
      });
    },
  },
};
</script>
