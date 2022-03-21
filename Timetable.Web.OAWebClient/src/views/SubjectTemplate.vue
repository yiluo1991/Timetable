<template>
  <div class="admin-page">
    <BasicToolbar :showRefreshButton="hasPermission('TP_G')" :showSearchbox="hasPermission('TP_G')" :showAddButton="false" @search="search" @reload="reload">
      <template v-if="hasPermission('SV_G')">
        <ElSelect size="medium" style="width:140px;" v-model="status" placeholder="">
          <ElOption value="" label="所有审核状态"></ElOption>
          <ElOption value="0" label="待审核"></ElOption>
          <ElOption value="1" label="审核通过"></ElOption>
          <ElOption value="2" label="不通过"></ElOption>
        </ElSelect>
        <ElSelect allow-create="" style="margin-left:10px;width:140px;margin-right:10px" size="medium" v-model="enable" placeholder="">
          <ElOption value="" label="所有可用状态"></ElOption>
          <ElOption value="true" label="可用"></ElOption>
          <ElOption value="false" label="禁用"></ElOption>
        </ElSelect>
      </template>
    </BasicToolbar>
    
   <SubjectTemplateTable ref="table" @showDialog="showDialog"></SubjectTemplateTable>
   <SubjectTemplateDialog ref="dialog" @reload="reload()"></SubjectTemplateDialog>
  </div>
</template>

<script>
export default {
  inject: ["hasPermission"],
  components: {
     SubjectTemplateTable:() => import( "../components/SubjectTemplate/SubjectTemplateTable"),
     SubjectTemplateDialog:()=>import('../components/SubjectTemplate/SubjectTemplateDialog')
  },
  name: "admintemplate",
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
    showDialog(e) {
  
       this.$refs.dialog.showDialog(e);
    },
  },
};
</script>
