<template>
  <div class="admin-page">
    <BasicToolbar
      :showRefreshButton="hasPermission('UG_G')"
      :showSearchbox="hasPermission('UG_G')"
      :showAddButton="hasPermission('UG_A')"
      @openAdd="openAdd"
      @search="search"
      @reload="reload"
    ></BasicToolbar>
    <RoleTable ref="table" v-on:onOpenEdit="onOpenEdit"></RoleTable>
    <RoleDialog ref="popup" @reload="reload"></RoleDialog>
  </div>
</template>
<script>
import RoleTable from "../components/UserGroup/UserGroupTable";
import RoleDialog from "../components/UserGroup/UserGroupDialog";
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
