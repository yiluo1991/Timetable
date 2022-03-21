<template>
  <div class="admin-page">
    <BasicToolbar @openAdd="openAdd" :showRefreshButton="hasGetPermission" addText="创建平台账号" :showSearchbox="hasGetPermission" :showAddButton="hasAddPermission" @search="search" @reload="reload">
      <ElSelect   style="margin:0 15px;width:140px;" size="medium" v-model="UserGroupId" placeholder="">
      <ElOption value="" label="所有用户组"></ElOption>
      <ElOption v-for="item in userGroup" :key="item.Id" :value="item.Id" :label="item.Name" ></ElOption>
    </ElSelect>
    </BasicToolbar>
    <UserTable :data='userGroup' @showEditDialog="showEditDialog"  ref="table"/>
     <UserDialog  ref="dialog" @reload="reload"></UserDialog>
  </div>
</template>
<script>
import UserDialog from '../components/User/UserDialog'
import UserTable from "../components/User/UserTable";
export default {
  components: {
    UserTable,
    UserDialog
  },
  name: "adminuser",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("U_G");
    },
    hasAddPermission() {
      return this.hasPermission("U_A");
    },
    hasEditPermission() {
      return this.hasPermission("U_M");
    },
    hasResetPermission() {
      return this.hasPermission("U_R");
    },
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
        keyword:e,
        UserGroupId:this.UserGroupId,
      });
    },
    /**搜索或刷新 */
    reload() {
        this.$refs.table.reload()
    },
    /**打开添加窗口 */
    openAdd() {
      this.$refs.dialog.showAddDialog();
    },
    /**打开修改窗口 */
    showEditDialog(e) {
      this.$refs.dialog.showEditDialog(e)
    },
    getUserGroup(){
        var url = "/usergroup/list";
        if (url) {
          this.$axios
            .post(this.$baseURL + url)
            .then((res) => {
              this.userGroup = res.data.data;
            });
        }
      }
  },
  mounted(){
    this.getUserGroup()
  }
};
</script>
