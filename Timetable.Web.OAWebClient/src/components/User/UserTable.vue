<template>
  <div>
    <ElTable :data="tableData" highlight-current-row row-key="Id">
      <ElTableColumn prop="Mobile" label="手机号（登录账号）">
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
              <ElImage style="width:20px;height:20px; vertical-align: middle;" :src="row.AvatarUrl" fit="fill" :lazy="true"></ElImage>
              {{ row.Mobile }}
            </span>
          </el-popover>
        </template>
      </ElTableColumn>
      <ElTableColumn prop="Email" label="邮箱"></ElTableColumn>

      <ElTableColumn prop="NickName" label="真实姓名 / 昵称">
        <template v-slot="{ row }"> {{ row.RealName }} / {{ row.NickName }} </template>
      </ElTableColumn>
      <ElTableColumn prop="Province" label="所在地">
        <template v-slot="{ row }"> {{ row.City }} </template>
      </ElTableColumn>
      <ElTableColumn label="创建时间" :width="160">
        <template v-slot="{ row }"> {{ row.CreateTime | datetime }} </template>
      </ElTableColumn>
      <ElTableColumn prop="Status" label="状态" :width="100">
        <template slot-scope="scope">
          <el-tag v-if="!(scope.row.Status & 4)" type="success">启用</el-tag>
          <el-tag v-if="scope.row.Status & 4" type="danger">禁用</el-tag>
        </template>
      </ElTableColumn>
      <ElTableColumn prop="UserGroupId" label="用户组">
        <template slot-scope="scope">
          <div v-for="item in data" :key="item.Id">
            <div v-if="scope.row.UserGroupId==item.Id" >{{item.Name}}</div>
          </div>
        </template>
      </ElTableColumn>
      <ElTableColumn prop="Edit" label="操作" v-if="hasEditPermission || hasResetAvatarUrlPermission || hasResetPasswordPermission" width="185">
        <template slot-scope="scope">
          <ElButton v-if="hasEditPermission" round size="small" icon="el-icon-edit" plain type="warning" @click="showEditDialog(scope.row)">修改</ElButton>
          <el-dropdown v-if="hasResetAvatarUrlPermission || hasResetPasswordPermission">
            <el-button round size="small"> 更多<i class="el-icon-arrow-down el-icon--right"></i> </el-button>

            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item v-if="hasResetPasswordPermission" icon="el-icon-lock" @click.native="resetPassword(scope.row)">重置密码</el-dropdown-item>
              <el-dropdown-item v-if="hasResetAvatarUrlPermission" icon="el-icon-picture-outline" @click.native="resetAvatarUrl(scope.row)">重置头像</el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
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
      return this.hasPermission("U_G");
    },
    hasResetPasswordPermission() {
      return this.hasPermission("U_R");
    },
    hasResetAvatarUrlPermission() {
      return this.hasPermission("U_RA");
    },
    hasEditPermission() {
      return this.hasPermission("U_M");
    },
  },
  props:
    ["data"]
  ,
  mounted() {
    this.reload();
  },
  methods: {
    reload(e) {
      console.log(arguments)
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
            .post(this.$baseURL + "/user/list", {
              keyword: this.searchOptions.keyword,
              cid: this.searchOptions.UserGroupId?parseInt(this.searchOptions.UserGroupId):0,
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
    resetPassword(row) {
      if (!this.hasResetPasswordPermission) {
        return;
      }
      this.$confirm(`您将重置【${row.Mobile}-${row.RealName}】账号的密码，确认?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          var loading = this.$loading();
          this.$axios.post(this.$baseURL + "/user/resetpassword", "id=" + row.Id).then((res) => {
            if (res.data.success) {
              this.$alert(res.data.msg, "重置成功", {
                confirmButtonText: "确定",
              });
            }

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
