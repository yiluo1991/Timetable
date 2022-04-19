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
    <StudentTable @openEdit="openEdit" ref="table" />
    <StudentDialog ref="dialog" @reload="reload()" />
  </div>
</template>
<script>
import StudentDialog from "../components/Student/StudentDialog.vue";
import StudentTable from "../components/Student/StudentTable.vue";
export default {
  components: {
    StudentTable,
    StudentDialog,
  },
  name: "adminstudent",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("SU_G");
    },
    hasEditPermission() {
      return this.hasPermission("SU_A");
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
