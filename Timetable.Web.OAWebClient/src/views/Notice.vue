<template>
  <div class="admin-page">
    <BasicToolbar :showAddButton="hasAddPermission" searchText="请输入学年进行查询" @openAdd="openAdd" :showRefreshButton="hasGetPermission" :showSearchbox="hasGetPermission"  @search="search" @reload="reload">
 
    </BasicToolbar>
    <NoticeTable    ref="table" @edit="openEdit" />
    <NoticeDialog ref="dialog" @reload="reload()"/>
  </div>
</template>
<script>  
import NoticeDialog from '../components/Notice/NoticeDialog.vue'
import NoticeTable from  "../components/Notice/NoticeTable.vue";
export default { 
  components: { 
    NoticeTable,
    NoticeDialog
  },
  name: "adminenotice",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("N_G");
    },
    hasAddPermission(){
      return this.hasPermission("N_A")
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
