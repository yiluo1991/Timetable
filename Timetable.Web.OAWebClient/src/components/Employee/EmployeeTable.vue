<template>
  <div>
    <ElTable
      ref="table"
      :data="tableData"
      highlight-current-row
      row-key="Id"
      default-expand-all
      :row-class-name="rowClass"
    >
      <ElTableColumn prop="LoginName" label="登录名" show-overflow-tooltip> </ElTableColumn>
      <ElTableColumn prop="RealName" label="真实姓名" show-overflow-tooltip></ElTableColumn>
      <ElTableColumn prop="Email" label="邮箱" show-overflow-tooltip></ElTableColumn>
      <ElTableColumn prop="Mobile" label="联系电话" show-overflow-tooltip></ElTableColumn>
      <ElTableColumn prop="Address" label="联系地址" show-overflow-tooltip></ElTableColumn>
      <ElTableColumn prop="Enable" label="启用">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.Enable" type="success">启用</el-tag>
          <el-tag v-if="!scope.row.Enable" type="danger">禁用</el-tag>
        </template>
      </ElTableColumn>
      <ElTableColumn prop="EmployeeRoleNames" label="角色" show-overflow-tooltip>
        <template slot-scope="scope">
          {{ scope.row.EmployeeRoleNames != null ? scope.row.EmployeeRoleNames.join("，") : "" }}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="Creator" label="创建人" show-overflow-tooltip></ElTableColumn>
      <ElTableColumn prop="CreateTime" label="创建时间" show-overflow-tooltip>
        <template slot-scope="scope">
          {{ scope.row.CreateTime | date }}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="Edit" label="操作" width="175" v-if="hasDeletePermission||hasEditPermission||hasResetPermission">
        <template slot-scope="scope" v-if="scope.row.Id > 0">
          <el-button v-if="hasEditPermission" @click="showEditDialog(scope.row)" round size="small" icon="el-icon-edit" plain type="warning"
            >修改</el-button
          >
          <el-dropdown v-if="hasDeletePermission||hasResetPermission">
            <el-button round size="small"> 更多<i class="el-icon-arrow-down el-icon--right"></i> </el-button>
            
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item v-if="hasResetPermission" icon="el-icon-lock" @click.native="resetpassword(scope.row)">重置密码</el-dropdown-item>
              <el-dropdown-item v-if="hasDeletePermission" icon="el-icon-delete" @click.native="deleteitem(scope.row)">删除</el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </ElTableColumn>
    </ElTable>
     <BasicPaginationBar
      :page.sync="page"
      :size.sync="size"
      :total="total"
      @current-change="reload"
      @size-change="reload"
    ></BasicPaginationBar>
  </div>
</template>
<script>
export default {
  data() {
    return {
      keyword: "",
      editData: {},
      tableData: [],
      page: 1,
      size: 10,
      total: 0,
    };
  }, 
   inject: ["hasPermission"],
  computed: {
    hasEditPermission() {
      return this.hasPermission("E_M");
    },
    hasGetPermission() {
      return this.hasPermission("E_G");
    },
    hasDeletePermission() {
      return this.hasPermission("E_D");
    },
      hasResetPermission() {
      return this.hasPermission("E_R");
    },
  },
  mounted() {
    this.reload();
  },
  methods: {
    rowClass: function(row) {
      if (row.Id < 0) return "gray-row";
    },
    reload: function(e) {
      if(!this.hasGetPermission)return;
        if(arguments.length>0){
            this.keyword=e;
            this.page=1;
        }
     this.$nextTick(()=>{
          var url = "/employee/list";
      if (url) {
        var loading = this.$loading({ text: "正在加载表格数据" });
        this.$axios
          .post(this.$baseURL + url, {
            keyword: this.keyword,
            page: this.page,
            rows: this.size,
          })
          .then((res) => {
            setTimeout(() => {
              loading.close();
            }, 100);
            
            this.tableData = res.data.data;
            this.total = res.data.total;
          });
      }
     })
    },
    showEditDialog(row) {
      if(!this.hasEditPermission)return;
      //修改，利用JSON创建数据副本给编辑框修改，避免修改过程中直接修改到表格内容
    
      var r=JSON.parse(JSON.stringify(row));
      delete(r.Creator);
       if(r.PermissionEndTime){
r.PermissionEndTime=new Date(r.PermissionEndTime);
       }
      
      this.$emit("onOpenEdit", r);
    },
    resetpassword: function(row) {
      if(!this.hasResetPermission)return;
      this.$confirm("此操作将重置【" + row.LoginName + "- " + row.RealName + "】的密码?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          var url = "/employee/resetPassword";
          if (url) {
            this.$axios
              .post(this.$baseURL + url,"id="+row.Id)
              .then((res) => {
                setTimeout(() => {
                  this.$loading().close();
                }, 100);
                if (res.data.success) {
                  
                   this.$alert(res.data.msg, "重置成功", {
                    confirmButtonText: "确定",
                  });
                } else {
                  this.$message({
                    type: "error",
                    message: res.data.msg,
                  });
                }
                this.reload();
              });
          }
        })
        .catch(() => {});
    },
    deleteitem(row) {
      if(!this.hasDeletePermission)return;
      this.$confirm("此操作将永久删除【" + row.LoginName + "- " + row.RealName + "】?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        //执行删除
         this.$axios.post(this.$baseURL + "/employee/delete", "id=" + row.Id).then((res) => {
          //执行删除
          this.$message({
            type: res.data.success ? "success" : "error",
            message: res.data.msg,
          });

          this.reload();
        });
      });
    },
  },
};
</script>
