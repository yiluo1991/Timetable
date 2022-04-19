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
    <AdministrativeClassTable @openEdit="openEdit" ref="table" />
    <AdministrativeClassDialog ref="dialog" @reload="reload()" />
  </div>
</template>
<script>
import AdministrativeClassDialog from "../components/AdministrativeClass/AdministrativeClassDialog.vue";
import AdministrativeClassTable from "../components/AdministrativeClass/AdministrativeClassTable.vue";
export default {
  components: {
    AdministrativeClassTable,
    AdministrativeClassDialog,
  },
  name: "adminadministrativeclass",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("AC_G");
    },
    hasEditPermission() {
      return this.hasPermission("AC_A");
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
