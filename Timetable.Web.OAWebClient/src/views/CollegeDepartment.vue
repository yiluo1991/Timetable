<template>
  <div class="admin-page">
    <BasicToolbar
      :showAddButton="hasEditPermission"
      searchText="请输入查询关键字"
      @openAdd="openAdd"
      :showRefreshButton="hasGetPermission"
      :showSearchbox="hasGetPermission"
      @search="search"
      @reload="reload"
    >
    </BasicToolbar>
    <CollegeDepartmentTable @openEdit="openEdit" ref="table" />
    <CollegeDepartmentDialog ref="dialog" @reload="reload()" />
  </div>
</template>
<script>
import CollegeDepartmentDialog from "../components/CollegeDepartment/CollegeDepartmentDialog.vue";
import CollegeDepartmentTable from "../components/CollegeDepartment/CollegeDepartmentTable.vue";
export default {
  components: {
    CollegeDepartmentTable,
    CollegeDepartmentDialog,
  },
  name: "admincollegedepartment",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("CD_G");
    },
    hasEditPermission() {
      return this.hasPermission("CD_A");
    },
  },
  data() {
    return {};
  },
  methods: {
    search(e) {
      this.$refs.table.reload(e);
    },
    reload() {
      this.$refs.table.reload();
    },
    /**打开添加窗口 */
    openAdd() {
      this.$refs.dialog.showAddDialog();
    },
    /**打开修改窗口 */
    openEdit(row) {
      this.$refs.dialog.showEditDialog(row);
    },
  },
};
</script>
