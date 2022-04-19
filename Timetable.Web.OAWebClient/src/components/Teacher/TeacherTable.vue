<template>
  <div>
    <ElTable
      default-expand-all
      row-key="SubjectCode"
      :empty-text="hasGetPermission ? '暂无数据' : '无权访问'"
      :data="tableData"
      highlight-current-row
    >
      <ElTableColumn
        prop="IdentityCode"
        width="200"
        label="教工号"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="Mobile"
        width="200"
        label="手机号"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="RealName"
        min-width="50px"
        label="真名"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn prop="CreateTime" label="创建时间" width="200">
        <template v-slot="{ row }">
          {{ row.CreateTime | datetime }}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="Gender" label="性别" width="150">
        <template v-slot="{ row }">
          <el-tag type="info">{{ gender[row.Gender] }}</el-tag>
        </template>
      </ElTableColumn>

      <ElTableColumn prop="Enable" label="状态" width="150">
        <template v-slot="{ row }">
          <el-tag v-if="row.Enable" type="success">启用</el-tag>
          <el-tag v-if="!row.Enable" type="danger">禁用</el-tag>
        </template>
      </ElTableColumn>

      <ElTableColumn prop="IsDefault" label="操作" width="200">
        <template v-slot="{ row }">
          <ElButton
            style="margin-right:10px;"
            @click="edit(row)"
            type="warning"
            round
            plain
              v-if="hasEditPermission"
            size="small"
            icon="el-icon-edit"
            >修改</ElButton
          >

          <el-dropdown v-if="hasDeletePermission || hasResetPermission">
            <el-button round size="small">
              更多<i class="el-icon-arrow-down el-icon--right"></i>
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                v-if="hasResetPermission"
                icon="el-icon-lock"
                @click.native="resetpassword(row)"
                >重置密码</el-dropdown-item
              >
              <el-dropdown-item
                v-if="hasDeletePermission"
                icon="el-icon-delete"
                @click.native="deleteitem(row)"
                >删除</el-dropdown-item
              >
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
      tableData: [],
      page: 1,
      size: 15,
      total: 0,
      keyword: "",
      gender: ["未知", "男", "女"],
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("T_G");
    },

    hasEditPermission() {
      return this.hasPermission("T_M");
    },
    hasResetPermission() {
      return this.hasPermission("T_R");
    },
    hasDeletePermission() {
      return this.hasPermission("T_D");
    },
  },
  mounted() {
    this.reload();
  },
  methods: {
     resetpassword: function(row) {
      if(!this.hasResetPermission)return;
      this.$confirm("此操作将重置【" +row.RealName + "】的密码?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          var loading=this.$loading();
          var url = "/teacher/resetPassword";
          if (url) {
            this.$axios
              .post(this.$baseURL + url+"?id="+encodeURIComponent( row.IdentityCode))
              .then((res) => {
              
                  loading.close();
             
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
              });
          }
        })
        .catch(() => {});
    },
    deleteitem(row) {
      if (!this.hasDeletePermission) return;
      this.$confirm("此操作将永久删除教师账号, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        this.$axios
          .post(
            this.$baseURL +
              "/teacher/delete" +
              "?id=" +
              encodeURIComponent(row.IdentityCode)
          )
          .then((res) => {
            //执行删除
            this.$message({
              type: res.data.success ? "success" : "error",
              message: res.data.msg,
            });

            this.reload();
          });
      });
    },
    edit(e) {
      this.$emit("openEdit", {
        ...JSON.parse(JSON.stringify(e)),
        OriginIdentityCode: e.IdentityCode,
      });
    },
    reload(e) {
      if (!this.hasGetPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = e;
            this.page=1; 
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/teacher/list";
        if (url) {
          var data = {
            keyword: this.keyword,
            page: this.page,
            rows: this.size,
          };

          this.$axios.post(url, data).then((res) => {
            setTimeout(() => {
              this.$loading().close();
            }, 100);

            this.tableData = res.data.data;
            this.total = res.data.total;
          });
        }
      });
    },
  },
};
</script>
