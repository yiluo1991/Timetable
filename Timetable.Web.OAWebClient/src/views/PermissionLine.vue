<template>
  <div class="admin-page">
    <BasicToolbar
      :showRefreshButton="hasPermission('PL_G')"
      :showSearchbox="hasPermission('PL_G')"
      :showAddButton="hasPermission('PL_A')"
      @openAdd="openAdd"
      @search="search"
      @reload="reload"
    ></BasicToolbar>
    <PermissionLineTable ref="table" v-on:onOpenEdit="onOpenEdit"></PermissionLineTable>
    <PermissionLineDialog ref="popup" @reload="reload"></PermissionLineDialog>
  </div>
</template>
<script>
import PermissionLineTable from "../components/PermissionLine/PermissionLineTable";
import PermissionLineDialog from "../components/PermissionLine/PermissionLineDialog";
export default {
  inject: ["hasPermission"],
  name: "adminpermissionline",
  components: {
    PermissionLineTable,
    PermissionLineDialog,
  },
  methods: {
    reload() {
      this.$refs.table.reload();
    },
    openAdd() {
      this.$refs.popup.reload(1);
    },
    search(e) {
      console.log(e);
      this.$refs.table.reload(e);
    },
    onOpenEdit(e) {
      this.$refs.popup.reload(2, e);
    },
  },
};
</script>
