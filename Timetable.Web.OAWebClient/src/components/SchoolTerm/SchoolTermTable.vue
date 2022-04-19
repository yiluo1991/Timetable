<template>
  <div>
    <ElTable :data="tableData" highlight-current-row row-key="Id">
      <ElTableColumn prop="Term" label="学期">
        <template v-slot="{ row }">
          {{ row.Year + "学年 第" + row.Term + "学期" }}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="CustomStart" label="学期开始日期">
        <template v-slot="{ row }">
          {{ row.CustomStart | date }}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="CustomEnd" label="学期结束日期">
        <template v-slot="{ row }">
          {{ row.CustomEnd | date }}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="IsDefault" label="当前学期">
        <template v-slot="{ row }" >
           <i  v-if="row.IsDefault"  style="color:green" class="el-icon-circle-check "></i> 
          
        </template>
      </ElTableColumn>
      <ElTableColumn prop="IsDefault" label="操作">
        <template v-slot="{ row }">
          <ElButton
            style="margin-right:10px;"
            @click="setDefault(row)"
            type="warning"
            round
            plain
            size="small"
            icon="el-icon-s-flag"
              v-if="hasSetDefaultPermission"
            >设为当前学期</ElButton
          >

          <ElDropdown v-if="hasDeletePermission || hasClearPermission">
            <ElButton round size="small">
              更多<i class="el-icon-arrow-down el-icon--right"></i>
            </ElButton>
            <ElDropdownMenu slot="dropdown">
              <ElDropdownItem
                v-if="hasClearPermission"
                icon="el-icon-s-release"
                @click.native="clear(row)"
                >清空学年排课和选课信息</ElDropdownItem
              >
              <ElDropdownItem
                v-if="hasDeletePermission"
                icon="el-icon-delete"
                @click.native="del(row)"
                >删除学年</ElDropdownItem
              >
            </ElDropdownMenu>
          </ElDropdown>
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
      total: 0,
      keyword: "",
      page: 1,
      size: 15,
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("ST_G");
    },
    hasSetDefaultPermission() {
      return this.hasPermission("ST_S");
    },
    hasDeletePermission() {
      return this.hasPermission("ST_D");
    },
    hasClearPermission() {
      return this.hasPermission("ST_C");
    },
  },
  mounted() {
    this.reload();
  },
  methods: {
    setDefault(row) {
      if (this.hasSetDefaultPermission) {
        this.$confirm(
          "确认切换到该学期？切换之后排课、查课等操作将关联该学期。",
          "提示",
          {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning",
          }
        )
          .then(() => {
            var url = this.$baseURL + "/SchoolTerm/SetDefault?id="+row.Id;
             this.$loading({ text: "处理中" });
              this.$axios.post(url).then((res) => {
                  setTimeout(() => {
                    this.$loading().close();
                  }, 100); 
                  this.reload();
                if(!res.data.success){
                  this.$message.error(res.data.msg);
                }
                  
              });
          })
          .catch(() => {});
      }
    },
    clear(row){
       if (this.hasSetDefaultPermission) {
        this.$confirm(
          "确认清空【"+row.Year + "学年 第" + row.Term + "学期 】数据排课选课数据？",
          "提示",
          {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning",
          }
        )
          .then(() => {
            var url = this.$baseURL + "/SchoolTerm/Clear?id="+row.Id;
             this.$loading({ text: "处理中" });
              this.$axios.post(url).then((res) => {
                  setTimeout(() => {
                    this.$loading().close();
                  }, 100); 
                  this.reload();
                if(!res.data.success){
                  this.$message.error(res.data.msg);
                }
                  
              });
          })
          .catch(() => {});
      }
    },
    del(row){
       if (this.hasDeletePermission) {
        this.$confirm(
          "确认删除【"+row.Year + "学年 第" + row.Term + "学期 】？",
          "提示",
          {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning",
          }
        )
          .then(() => {
            var url = this.$baseURL + "/SchoolTerm/Delete?id="+row.Id;
             this.$loading({ text: "处理中" });
              this.$axios.post(url).then((res) => {
                  setTimeout(() => {
                    this.$loading().close();
                  }, 100); 
                  this.reload();
                if(!res.data.success){
                  this.$message.error(res.data.msg);
                }
                  
              });
          })
          .catch(() => {});
      }
    },
    reload(e) {
      if (!this.hasGetPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = e;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/SchoolTerm/list";
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
