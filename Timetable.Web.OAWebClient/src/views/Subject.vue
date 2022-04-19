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
    <SubjectTable @openEdit="openEdit" ref="table" />
    <SubjectDialog ref="dialog" @reload="reload()" />
  </div>
</template>
<script>
import SubjectDialog from "../components/Subject/SubjectDialog.vue";
import SubjectTable from "../components/Subject/SubjectTable.vue";
export default {
  components: {
    SubjectTable,
    SubjectDialog,
  },
  name: "adminsubject",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("S_G");
    },
    hasEditPermission() {
      return this.hasPermission("S_A");
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
