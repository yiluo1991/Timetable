<template>
  <div>
    <ElTable
      default-expand-all
      row-key="SID"
      :empty-text="hasGetPermission ? '暂无数据' : '无权访问'"
      :data="tableData"
      highlight-current-row
      :row-class-name="rowClass"
    >
      <ElTableColumn
        prop="Name"
        label="名称"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="ContactName"
        label="联系人"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="ContactPhone"
        label="联系电话"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn prop="Enable" label="状态">
        <template v-slot="{ row }">
          <el-tag v-if="row.Enable" type="success">启用</el-tag>
          <el-tag v-if="!row.Enable" type="danger">禁用</el-tag>
        </template>
      </ElTableColumn>
      <ElTableColumn
        prop="Remark"
        label="备注"
        show-overflow-tooltip
      ></ElTableColumn>

      <ElTableColumn prop="IsDefault" label="操作" width="120">
        <template v-slot="{ row }">
          <ElButton
            style="margin-right:10px;"
            @click="edit(row)"
            type="warning"
            round
            plain
            size="small"
            icon="el-icon-edit"
            >修改</ElButton
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
      tableData: [],

      keyword: "",
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("CD_G");
    },

    hasEditPermission() {
      return this.hasPermission("CD_M");
    },
  },
  mounted() {
    this.reload();
  },
  methods: {
    rowClass: function({ row }) {
      if (row.SID.startsWith('C_')) return "gray-row";
    },
    edit(e) {
      this.$emit("openEdit", JSON.parse(JSON.stringify(e)));
    },
    reload(e) {
      if (!this.hasGetPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = e;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/CollegeDepartment/list";
        if (url) {
          var data = {
            keyword: this.keyword,
          };

          this.$axios.post(url, data).then((res) => {
            setTimeout(() => {
              this.$loading().close();
            }, 100);

            this.tableData = res.data.data;
          });
        }
      });
    },
  },
};
</script>
