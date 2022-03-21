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
      <ElTableColumn prop="Province" label="省份" width="300"></ElTableColumn>
      <ElTableColumn prop="Name" label="学校名" ></ElTableColumn>
      <ElTableColumn width="100" prop="SortNum" label="排序号"></ElTableColumn>
      <ElTableColumn v-if="editPermission || deletePermission" prop="Weight" label="操作" width="185">
        <template slot-scope="scope">
          <ElButton @click="showEditDialog(scope.row)" round size="small" icon="el-icon-edit" plain type="warning" v-if="editPermission">修改</ElButton>
          <ElButton type="danger" icon="el-icon-delete" round size="small" plain v-if="deletePermission" @click="deleteitem(scope.row)">删除</ElButton>
        </template>
      </ElTableColumn>
    </ElTable>
    <BasicPaginationBar :page.sync="page" :size.sync="size" :total="total" @current-change="reload" @size-change="reload"></BasicPaginationBar>
  </div>
</template>
<script>
export default {
  data() {
    return {
      keyword: "",
      list: [],
      page: 1,
      size: 30,
      total: 0,
    };
  },
  computed: {
    deletePermission() {
      return this.hasPermission("SC_D");
    },
    editPermission() {
      return this.hasPermission("SC_M");
    },
    getPermission() {
      return this.hasPermission("SC_G");
    },
  },
  mounted() {
    this.reload();
  },
  inject: ["hasPermission"],
  methods: {
    
    reload(keyword) {
      if (!this.getPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = keyword;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = "/school/list";
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
      //修改，利用JSON创建数据副本给编辑框修改，避免修改过程中直接修改到表格内容
      this.$emit("onOpenEdit", JSON.parse(JSON.stringify(row)));
    },
    deleteitem(row) {
      this.$confirm("此操作将永久删除该学校信息, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        this.$axios.post(this.$baseURL + "/school/delete", "id=" + row.Id).then((res) => {
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
