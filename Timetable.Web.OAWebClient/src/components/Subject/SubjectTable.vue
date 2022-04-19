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
        prop="SubjectCode"
        width="200"
        label="科目代码"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="Name"
         min-width="50px"
        label="科目名"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="CollegeName"
        label="开设单位"
        show-overflow-tooltip
      >
       <template v-slot="{ row }">
          <el-tag v-if="row.CollegeName" type="success">{{row.CollegeName}}</el-tag>

          <el-tag v-if="row.DepartmentName" type="primary">{{row.DepartmentName}}</el-tag>
        </template></ElTableColumn>
     
      <ElTableColumn prop="Enable" label="科目类型" width="150">
        <template v-slot="{ row }">
          <el-tag   type="info">{{type[row.SubjectType]}}</el-tag>
         
        </template>
      </ElTableColumn>
       <ElTableColumn prop="Enable" label="必修类型" width="150">
        <template v-slot="{ row }">
          <el-tag   type="info">{{prop[row.SubjectProperty]}}</el-tag> 
        </template>
      </ElTableColumn>
      <ElTableColumn prop="Enable" label="状态" width="150">
        <template v-slot="{ row }">
          <el-tag v-if="row.Enable" type="success">启用</el-tag>
          <el-tag v-if="!row.Enable" type="danger">禁用</el-tag>
        </template>
      </ElTableColumn>
      

      <ElTableColumn prop="IsDefault" label="操作" width="210">
        <template v-slot="{ row }">
          <ElButton
            
            @click="edit(row)"
            type="warning"
            round
            plain
            size="small"
            v-if="hasEditPermission"
            icon="el-icon-edit"
            >修改</ElButton
          ><ElButton
            type="danger"
            icon="el-icon-delete"
            round
            size="small"
            plain
            v-if="hasDeletePermission"
            @click="deleteitem(row)"
            >删除</ElButton
          >
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
      page:1,
      size:15,
      total:0,
      keyword: "",
      type:['未知','通识课','专业课','实践课'],
      prop:['未知','必修','选修']
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("S_G");
    },
    hasEditPermission() {
      return this.hasPermission("S_M");
    },
    hasDeletePermission(){
      return this.hasPermission("S_D");
    }
  },
  mounted() {
    this.reload();
  },
  methods: {
    deleteitem(row) {
      if(!this.hasDeletePermission)return;
      this.$confirm("此操作将永久删除科目, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        this.$axios.post(this.$baseURL + "/subject/delete"+"?id=" + encodeURIComponent( row.SubjectCode)).then((res) => {
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
      this.$emit("openEdit", 
      {
        ...JSON.parse(JSON.stringify(e)),
        OriginSubjectCode:e.SubjectCode,
        CD:"C_"+(e.CollegeId||'')+"D_"+(e.DepartmentId||'')
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
        var url = this.$baseURL + "/subject/list";
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
