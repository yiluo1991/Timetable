<template>
  <div class="admin-page">
    <BasicToolbar
      :showRefreshButton="hasPermission('PRP_G')"
      addText="添加新归档"
      :showSearchbox="hasPermission('PRP_G')"
      :showAddButton="hasPermission('PRP_A')"
      @search="search"
      @reload="reload"
      @openAdd="openAdd"
    >
    </BasicToolbar>

    <ProReportPackageTable @showPaperListDialog="showPaperListDialog"
      ref="table" 
    ></ProReportPackageTable>
    <ProReportPackageAddDialog @reload="reload" ref="addDialog"></ProReportPackageAddDialog>
  </div>
</template>

<script>
export default {
  inject: ["hasPermission"],
  components: {
    ProReportPackageTable: () =>
      import("../components/ProReportPackage/ProReportPackageTable"),
      ProReportPackageAddDialog: () =>
      import("../components/ProReportPackage/ProReportPackageAddDialog"),
  },
  name: "adminproreportpackage",
  data() {
    return {
      status: "",
      enable: "",
    };
  },
  methods: {
    search(e) {
      this.$refs.table.reload(e);
    },
    reload() {
      this.$refs.table.reload();
    },
    openAdd(){
      this.$refs.addDialog.openDialog();
    },
     showPaperListDialog(row){
      console.log(row)
    }
  },
};
</script>
