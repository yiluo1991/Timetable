<template>
    <div class="admin-page">
        <BasicToolbar
            :showRefreshButton="hasPermission('E_G')"
      :showSearchbox="hasPermission('E_G')"
      :showAddButton="hasPermission('E_A')"
         @openAdd="openAdd" @search="search" @reload="reload"></BasicToolbar>
        <EmployeeTable ref="table" v-on:onOpenEdit="onOpenEdit"></EmployeeTable>
        <EmployeeDialog  ref="popup" @reload="reload"></EmployeeDialog>
    </div>
</template>
<script>
import EmployeeTable from '../components/Employee/EmployeeTable' 
import EmployeeDialog from '../components/Employee/EmployeeDialog'
export default {
      inject: ["hasPermission"],
    name:"adminemployee",
     components: {
        EmployeeTable,EmployeeDialog
    }, 
    methods: {
        reload(){
            this.$refs.table.reload()
        },
        openAdd(){
            this.$refs.popup.reload(1)
        },
        search(e){
            console.log(e)
            this.$refs.table.reload(e)
        },
        onOpenEdit(e){
            this.$refs.popup.reload(2,e)
        }
    }
}
</script>