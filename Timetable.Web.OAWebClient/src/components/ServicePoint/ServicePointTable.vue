<template>
  <div>
    <el-table :data="tableData" highlight-current-row row-key="Guid">
      <el-table-column prop="Guid" label="节点实例ID" sortable> </el-table-column>
      <el-table-column prop="Name" label="节点名称" sortable> </el-table-column>
      <el-table-column prop="Host" label="节点地址" sortable></el-table-column>
      <el-table-column prop="Online" label="在线" width="100" sortable>
        <template slot-scope="scope">
          <el-tag :type="scope.row.Online ? 'success' : 'danger'"> {{ scope.row.Online ? "在线" : "离线" }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="Healthy" label="健康程度" width="100" sortable>
        <template slot-scope="scope">
          <el-tag :type="scope.row.Healthy ? 'success' : 'warning'"> {{ scope.row.Healthy ? "健康" : "不健康" }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column prop="Weight" label="权重" width="100" sortable></el-table-column>
      <el-table-column prop="" label="请求比例" width="100">
        <template slot-scope="scope">
          {{ scope.row.Online && scope.row.Healthy ? ((scope.row.Weight * 100) / totalWeight).toFixed(2) + "%" : "-" }}
        </template>
      </el-table-column>
      <el-table-column v-if="hasEditPermission || hasDeletePermission" prop="Weight" label="操作" width="185">
        <template slot-scope="scope">
          <el-button v-if="hasEditPermission" @click="showEditDialog(scope.row)" round size="small" icon="el-icon-edit" plain type="warning">修改</el-button>
          <el-button v-if="hasDeletePermission" type="danger" icon="el-icon-delete" round size="small" plain @click="deleteitem(scope.row)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>
<script>
export default {
  inject: ["hasPermission"],
  computed: {
    hasEditPermission() {
      return this.hasPermission("SP_M");
    },
    hasDeletePermission() {
      return this.hasPermission("SP_D");
    },
  },
  data() {
    return {
      tableData: [],
      totalWeight: undefined,
    };
  },
  methods: {
    reload() {
      var loading = this.$loading({ text: "加载中" });
      this.$axios.post("/api/ServicePoint/List").then((res) => {
        var data = res.data.data;
        loading.close();
        var total = 0;
        data.forEach((row) => {
          if (row.Online && row.Healthy) total += row.Weight;
        });
        this.totalWeight = total;
        this.tableData = data;
      });
    },
    showEditDialog(row) {
      this.$emit("edit", row);
    },
    deleteitem(row) {
      this.$confirm("此操作将永久删除【" + row.Name + "】服务节点, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          this.$axios.post("/api/ServicePoint/delete", "guid=" + row.Guid).then((res) => {
            if (res.data.success) {
              this.$message({
                type: "success",
                message: "删除成功!",
              });
            } else {
              this.$message({
                type: "error",
                message: data.Message,
              });
            }
            this.reload();
          });
        })
        .catch(() => {});
    },
  },
  mounted() {
    this.reload();
  },
};
</script>
