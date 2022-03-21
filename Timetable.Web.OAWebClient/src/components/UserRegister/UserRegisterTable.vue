<template>
  <div>
    <ElTable :data="tableData" highlight-current-row row-key="Id">
      <ElTableColumn prop="Mobile" label="注册手机号">
        <template v-slot="{ row }">
          <el-popover placement="top-start" trigger="hover">
            <div>
              <p>身份证号：{{ row.IDCard ? row.IDCard : "未填" }}</p>
              <template v-if="row.OrganizationName">
                <p>机构名：{{ row.OrganizationName ? row.OrganizationName : "未填" }}</p>
                <p>机构代码：{{ row.OrganizationCode ? row.OrganizationCode : "未填" }}</p>
              </template>
            </div>
            <span slot="reference">
              <ElTag size="mini" v-if="row.OrganizationName"> 企</ElTag>
              <ElTag size="mini" v-else type="info"> 个</ElTag> 
              {{ row.Mobile }}
            </span>
          </el-popover>
        </template>
      </ElTableColumn> 
      <ElTableColumn prop="RealName" label="真实姓名"> 
      </ElTableColumn>
    <ElTableColumn prop="IdentityNumber" label="学号/工号"> 
      </ElTableColumn>
      <ElTableColumn label="申请时间" :width="160">
        <template v-slot="{ row }"> {{ row.CreateTime | datetime }} </template>
      </ElTableColumn>
      <ElTableColumn prop="UserRegisterState" label="审核状态" :width="100">
        <template slot-scope="scope">
          <el-tag v-if="scope.row.UserRegisterState==0" type="info">未审核</el-tag>
          <el-tag v-if="scope.row.UserRegisterState==1" type="success">通过</el-tag>
           <el-tag v-if="scope.row.UserRegisterState==2" type="danger">拒绝</el-tag>
        </template>
      </ElTableColumn>
      <ElTableColumn prop="UserGroupName" label="用户组"> </ElTableColumn>
      <ElTableColumn prop="Edit" label="操作" v-if="hasEditPermission  ||hasGetPermission" width="185">
        <template slot-scope="scope">
          <ElButton v-if="hasEditPermission&& scope.row.UserRegisterState==0" round size="small" icon="el-icon-edit" plain type="warning" @click="showEditDialog(scope.row)">审核</ElButton>
          <ElButton v-else-if="hasGetPermission" round size="small" icon="el-icon-edit" plain type="primary" @click="showEditDialog(scope.row)">详情</ElButton>
          <ElButton v-if="scope.row.UserRegisterState==0&&hasDeletePermission" plain type="danger" icon="el-icon-delete" round size="small" @click="del(scope.row.Id)">删除</ElButton>
        </template>
      </ElTableColumn>
    </ElTable>
    <BasicPaginationBar :page.sync="page" :size.sync="rows" :total="total" @current-change="reload" @size-change="reload"></BasicPaginationBar>
  </div>
</template>
<script>
export default {
  data() {
    return {
      tableData: [],
      total: 0,
       searchOptions: {
        keyword: "",
        UserGroupId:""
      },
      page: 1,
      rows: 15,
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("UR_G");
    }, 
    hasEditPermission() {
      return this.hasPermission("UR_A");
    },
    hasDeletePermission(){
      return this.hasPermission("UR_D")
    }
  },
  props:
    ["data"]
  ,
  mounted() {
    this.reload();
  },
  methods: {
    reload(e) {
     
      if (!this.hasGetPermission) {
        return;
      }
      /**处理不同的参数传递 */
      if (arguments.length > 0) {
        this.page = 1;
        this.searchOptions = e;
      }
      if (this.hasGetPermission) {
        var loading = this.$loading({ text: "正在加载表格数据" });
        this.$nextTick(() => {
          this.$axios
            .post(this.$baseURL + "/userregister/list", {
              keyword: this.searchOptions.keyword, 
              page: this.page,
              rows: this.rows,
            })
            .then((res) => {
              setTimeout(() => {
                loading.close();
              }, 100);
             
              this.total = res.data.total;
              this.tableData = res.data.data;
            });
        });
      } else {
        this.total = 0;
        this.tableData = [];
      }
    },
    del(id) {
      if (!this.hasDeletePermission) {
        return;
      }
      this.$confirm(`将直接删除该申请且不通知注册用户，确认?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          var loading = this.$loading();
          this.$axios.post(this.$baseURL + "/userregister/delete", "id=" + id).then((res) => { 
            setTimeout(() => {
              loading.close();
              this.reload();
            }, 0);
          });
        })
        .catch(() => {});
    },
    showEditDialog(row) {
      if (!this.hasEditPermission) {
        return;
      }
      this.$emit("showEditDialog", row);
    },

    resetAvatarUrl(row) {
      if (!this.hasResetAvatarUrlPermission) {
        return;
      }
      var loading = this.$loading();
      this.$axios.post(this.$baseURL + "/user/resetAvatarUrl", "id=" + row.Id).then((res) => {
        setTimeout(() => {
          loading.close();
          this.reload();
        }, 0);
      });
    },
  },
};
</script>
