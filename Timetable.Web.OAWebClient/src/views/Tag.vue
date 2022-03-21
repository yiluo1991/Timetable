<template>
  <div class="admin-page">
    <BasicToolbar
      :showRefreshButton="hasPermission('T_G')"
      :showSearchbox="hasPermission('T_G')"
      :showAddButton="hasPermission('T_A')"
      @openAdd="openAdd"
      @search="search"
      @reload="reload"
    ></BasicToolbar>
    <TagTable ref="table"  @openEdit="openEdit"></TagTable>
    <TagDialog ref="dialog" @reload="reload"></TagDialog>
  </div>
</template>

<script>

export default {
  inject: ["hasPermission"],
  components:{
      TagTable:() => import( "../components/Tag/TagTable"),
      TagDialog:()=>import("../components/Tag/TagDialog")
  },
  name: "admintag",
  methods:{
      openAdd(){
  this.$refs.dialog.showAddDialog();
      },
      search(e){
           this.$refs.table.reload(e)
      },
      reload(){
          this.$refs.table.reload()
      },
      openEdit(e){
         this.$refs.dialog.showEditDialog(JSON.parse(JSON.stringify(e)));
      }
  }
};
</script>
