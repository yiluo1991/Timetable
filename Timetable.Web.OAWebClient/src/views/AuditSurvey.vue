<template>
  <div class="admin-page">
    <BasicToolbar   :showRefreshButton="hasPermission('AS_G')" :showSearchbox="hasPermission('AS_G')" :showAddButton="false" @search="search" @reload="reload">


    </BasicToolbar>
    <AuditSurveyTable ref="table" @showDialog="showDialog"></AuditSurveyTable>
    <AuditDialog ref="dialog" @reload="reload"></AuditDialog>
  </div>
</template>
<script>
import AuditSurveyTable from '../components/AuditSurvey/AuditSurveyTable';
import AuditDialog from '../components/AuditSurvey/AuditDialog'
export default {
  name: "adminauditsurvey",
  components: {
      AuditSurveyTable,
      AuditDialog
  },
  inject: ["hasPermission"],
  methods: {
    search(e) {
        this.$refs.table.reload(e)
    },
    reload() {
        this.$refs.table.reload()
    },
    showDialog(data){
      data.originList=JSON.parse(data.originList);
      this.$refs.dialog.showDialog(data);
    }
  },
};
</script>
