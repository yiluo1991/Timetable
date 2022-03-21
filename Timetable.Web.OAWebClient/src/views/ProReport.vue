<template>
  <div class="admin-page">
    <BasicToolbar :showRefreshButton="hasPermission('TP_G')" :showSearchbox="hasPermission('TP_G')" :showAddButton="false" @search="search" @reload="reload">
      <template v-if="hasPermission('SV_G')">
        
        <ElSelect allow-create="" style="margin-left:10px;width:140px;margin-right:10px" size="medium" v-model="enable" placeholder="">
          <ElOption value="" label="所有可用状态"></ElOption>
          <ElOption value="true" label="可用"></ElOption>
          <ElOption value="false" label="禁用"></ElOption>
        </ElSelect>
      </template>
    </BasicToolbar>
    
   <ProReportTable  ref="table"  @editItem="editItem" @editMate="editMate" ></ProReportTable>
   <ProReportItemDialog @reload="reload" ref="itemDialog"></ProReportItemDialog>
   <ProReportMateDialog ref="mateDialog" @reload="reload"></ProReportMateDialog>

  </div>
</template>

<script>
export default {
  inject: ["hasPermission"],
  components: { 
     ProReportTable:() => import( "../components/ProReport/ProReportTable"),
    ProReportItemDialog:()=>import("../components/ProReport/ProReportItemDialog"),
    ProReportMateDialog:()=>import("../components/ProReport/ProReportMateDialog")
  },
  //name: "adminproreport",
  data() {
    return {
     status: "",
      enable: "",
    };
  },
  methods: {
    search(e) {
          this.$refs.table.reload({
          keyword:e,
          status:this.status,
          enable:this.enable,
        
        });
    },
    reload() {
        this.$refs.table.reload()
    },
    editItem(e) {
      this.$refs.itemDialog.showEditDialog(e);
    },
    editMate(e){
      this.$refs.mateDialog.showDialog(e);
    },
   
  },
};
</script>
