<template>
  <div class="admin-page">
    <BasicToolbar :showAddButton="hasAddPermission" searchText="请输入学年进行查询" @openAdd="openAdd" :showRefreshButton="hasGetPermission" :showSearchbox="hasGetPermission"  @search="search" @reload="reload">
 
    </BasicToolbar>
    <SchoolTermTable    ref="table" />
    <SchoolTermDialog ref="dialog" @reload="reload()"/>
  </div>
</template>
<script>  
import SchoolTermDialog from '../components/SchoolTerm/SchoolTermDialog.vue'
import SchoolTermTable from  "../components/SchoolTerm/SchoolTermTable.vue";
export default { 
  components: { 
    SchoolTermTable,
    SchoolTermDialog
  },
  name: "admineschoolterm",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("ST_G");
    },
    hasAddPermission(){
      return this.hasPermission("ST_A")
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
this.$refs.dialog.showDialog();
      
    },
    /**打开修改窗口 */
    openEdit() {},
  },
};
</script>
