<template>
  <div class="admin-page">
    <BasicToolbar
      @openAdd="openAdd"
      :showRefreshButton="hasPermission('PG_G')"
      :showSearchbox="hasPermission('PG_G')"
      :showAddButton="hasPermission('PG_A')"
      @search="search"
      @reload="reload"
    ></BasicToolbar>
    <PermissionGroupTable ref="table" v-on:onOpenEdit="onOpenEdit"></PermissionGroupTable>
    <PermissionGroupDialog ref="popup" @reload="reload"></PermissionGroupDialog>
  </div>
</template>
<script>
import PermissionGroupTable from "../components/PermissionGroup/PermissionGroupTable";
import PermissionGroupDialog from "../components/PermissionGroup/PermissionGroupDialog";
export default {
  //name为访问路径去除斜杠，全小写
  name: "adminpermissiongroup",
  components: {
    PermissionGroupTable,
    PermissionGroupDialog,
  },
  inject: ["hasPermission"],
  data() {
    return {};
  },
  methods: {
    reload() {
      this.$refs.table.reload();
    },
    openAdd() {
      this.$refs.popup.reload(1);
    },
    search(e) {
      this.$refs.table.reload(e);
    },
    onOpenEdit(e) {
      this.$refs.popup.reload(2, e);
    },
  },
};
</script>
