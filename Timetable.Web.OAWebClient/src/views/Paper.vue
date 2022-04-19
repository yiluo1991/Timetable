<template>
  <div class="admin-page">
    <BasicToolbar :showAddButton="hasAddPermission" searchText="请输入学年进行查询" @openAdd="openAdd" :showRefreshButton="hasGetPermission" :showSearchbox="hasGetPermission"  @search="search" @reload="reload">
 
    </BasicToolbar>
    <PaperTable    ref="table" @edit="openEdit" />
    <PaperDialog ref="dialog" @reload="reload()"/>
  </div>
</template>
<script>  
import PaperDialog from '../components/Paper/PaperDialog.vue'
import PaperTable from  "../components/Paper/PaperTable.vue";
export default { 
  components: { 
    PaperTable,
    PaperDialog
  },
  name: "adminepaper",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("P_G");
    },
    hasAddPermission(){
      return this.hasPermission("P_A")
    }
  },
  data() {
    return {
   
    };
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
    openEdit(e) {
       this.$refs.dialog.showEditDialog(JSON.parse(JSON.stringify(e)));
    },
  },
};
</script>
