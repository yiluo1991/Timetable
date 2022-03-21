<template>
  <div class="admin-page">
    <BasicToolbar
      :showRefreshButton="hasPermission('R_G')"
      :showSearchbox="hasPermission('R_G')"
      :showAddButton="hasPermission('R_A')"
      @openAdd="openAdd"
      @search="search"
      @reload="reload"
    ></BasicToolbar>
    <RoleTable ref="table" v-on:onOpenEdit="onOpenEdit"></RoleTable>
    <RoleDialog ref="popup" @reload="reload"></RoleDialog>
  </div>
</template>
<script>
import RoleTable from "../components/Role/RoleTable";
import RoleDialog from "../components/Role/RoleDialog";
export default {
  name: "adminrole",
  inject: ["hasPermission"],
  components: {
    RoleTable,
    RoleDialog,
  },
  methods: {
    reload() {
      this.$refs.table.reload();
    },
    openAdd() {
      this.$refs.popup.showAddDialog();
    },
    search(e) {
      console.log(e);
      this.$refs.table.reload(e);
    },
    onOpenEdit(e) {
      this.$refs.popup.showEditDialog(e);
    },
  },
};
</script>
