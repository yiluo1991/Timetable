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
        prop="Id"
        width="100"
        label="ID"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="FullName"
         min-width="50px"
        label="班级全名"
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
     
      <ElTableColumn prop="Grade" label="学年" width="150"></ElTableColumn>
       
      <ElTableColumn prop="AdministrativeClassState" label="状态" width="150">
        <template v-slot="{ row }">
          <el-tag v-if="row.AdministrativeClassState==0" type="success">在读</el-tag>
          <el-tag v-else-if="row.AdministrativeClassState==1" type="info">毕业</el-tag>
           <el-tag v-else-if="row.AdministrativeClassState==2" type="danger">解散</el-tag>
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
      
      prop:['在读','毕业','解散']
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("AC_G");
    },

    hasEditPermission() {
      return this.hasPermission("AC_M");
    },
    hasDeletePermission(){
      return this.hasPermission("AC_D");
    }
  },
  mounted() {
    this.reload();
  },
  methods: {
    deleteitem(row) {
      if(!this.hasDeletePermission)return;
      this.$confirm("此操作将永久删除班级, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        this.$axios.post(this.$baseURL + "/administrativeclass/delete"+"?id=" + encodeURIComponent( row.Id)).then((res) => {
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
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/administrativeclass/list";
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
