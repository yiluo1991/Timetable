<template>
  <div class="admin-page">
    <BasicToolbar :showRefreshButton="hasGetPermission" addText="创建平台账号" :showSearchbox="hasGetPermission" :showAddButton="false" @search="search" @reload="reload">
        <template #append>
           <ElButton style="float:right;" type="success" round plain size="medium" icon="el-icon-setting" @click="showInfoDialog">
             注册设置
           </ElButton>
        </template>
       
    </BasicToolbar>
    <UserRegisterTable :data='userGroup' @showEditDialog="showEditDialog"  ref="table"/>
     <UserRegisterDialog  ref="dialog" @reload="reload"></UserRegisterDialog>
     <UserRegisterConfigDialog ref="config" @reload="reload"></UserRegisterConfigDialog>
  </div>
</template>
<script>
import UserRegisterDialog from '../components/UserRegister/UserRegisterDialog.vue'
import UserRegisterTable from "../components/UserRegister/UserRegisterTable.vue"; 
import UserRegisterConfigDialog from '../components/UserRegister/UserRegisterConfigDialog.vue'
export default {
  components: {
    UserRegisterTable,
    UserRegisterDialog,
    UserRegisterConfigDialog
  },
  name: "adminuser",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("UR_G");
    },
     hasGetInfoPermission() {
       return this.hasPermission("UR_I");
    }
    
    
  },
  data() {
    return {
      userGroup:"",
      UserGroupId:''
    };
  },
  methods: {
   search(e) {
      this.$refs.table.reload({
        keyword:e
      });
    },
    /**搜索或刷新 */
    reload() {
        this.$refs.table.reload()
    },
 
    /**打开修改窗口 */
    showEditDialog(e) {
      this.$refs.dialog.showEditDialog(e)
    },
    showInfoDialog(e){
 this.$refs.config.show(e)
    }
   
  },
  mounted(){
 
  }
};
</script>
