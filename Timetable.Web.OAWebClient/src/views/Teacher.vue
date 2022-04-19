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
    <TeacherTable @openEdit="openEdit" ref="table" />
    <TeacherDialog ref="dialog" @reload="reload()" />
  </div>
</template>
<script>
import TeacherDialog from "../components/Teacher/TeacherDialog.vue";
import TeacherTable from "../components/Teacher/TeacherTable.vue";
export default {
  components: {
    TeacherTable,
    TeacherDialog,
  },
  name: "adminteacher",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("T_G");
    },
    hasEditPermission() {
      return this.hasPermission("T_A");
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
