<template>
    <div class="admin-page">
        <BasicToolbar
            :showRefreshButton="hasPermission('SP_G')"
             :showSearchbox="false"
            :showAddButton="hasPermission('SP_A')"
         @openAdd="add" @search="search" @reload="reload"></BasicToolbar>
        <ServicePointTable ref="table" @edit="edit"></ServicePointTable>
        <ServicePointDialog  ref="dialog" @reload="reload"></ServicePointDialog> 
    </div>
</template>
<script>
 import ServicePointTable from '../components/ServicePoint/ServicePointTable'
 import ServicePointDialog from '../components/ServicePoint/ServicePointDialog'
export default {
     inject: ["hasPermission"],
    name:"servicepoint",
     components: {
        ServicePointTable,ServicePointDialog
    }, 
    methods: {
        reload(){
            this.$refs.table.reload()
        },
        add(){
            this.$refs.dialog.showAdd()
        },
        search(e){
            console.log(e)
            this.$refs.table.reload(e)
        },
        edit(e){
            
            this.$refs.dialog.showEdit(e)
        }
    }
}
</script>