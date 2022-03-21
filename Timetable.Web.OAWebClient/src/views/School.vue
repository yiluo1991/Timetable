<template>
  <div class="admin-page">
    <BasicToolbar
      :showRefreshButton="hasPermission('SC_G')"
      :showSearchbox="hasPermission('SC_G')"
      :showAddButton="hasPermission('SC_A')"
      @openAdd="openAdd"
      searchText="省份 / 学校名"
      @search="search"
      @reload="reload"
    ></BasicToolbar>
     <SchoolTable ref="table" v-on:onOpenEdit="onOpenEdit"></SchoolTable>
     <SchoolDialog ref="dialog" @reload="reload"></SchoolDialog>
  </div>
</template>
<script>
import SchoolTable from '../components/School/SchoolTable';
import SchoolDialog from '../components/School/SchoolDialog'
var text=``
export default {
  name:"adminschool",
    components: {
        SchoolTable,
        SchoolDialog
    },
  inject: ["hasPermission"],
  methods: {
    openAdd() {
        this.$refs.dialog.showAddDialog();
    },
    search(e) {
          this.$refs.table.reload(e);
    },
    reload() {
         this.$refs.table.reload();
    },
    onOpenEdit(e){
         this.$refs.dialog.showEditDialog(JSON.parse(JSON.stringify(e)));
    }
  },
  data () {
      return {
          show:false,
        list:text.split('\n')
      }
  }
};
</script>
