<template>
  <div>
    <ElTable
      default-expand-all
      row-key="SubjectCode"
      :empty-text="hasGetPermission ? '暂无数据' : '无权访问'"
      :data="tableData"
      highlight-current-row
    >
     <ElTableColumn prop="Term" label="学期" width="200">
        <template v-slot="{ row }">
          {{ row.Course.Year + "学年 第" + row.Course.Term + "学期" }}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="CollegeId" label="开课院系 / 科目" show-overflow-tooltip>
        <template v-slot="{ row }">
          <el-tag v-if="row.Course.CollegeName" type="success">{{
            row.Course.CollegeName
          }}</el-tag>

          <el-tag v-if="row.Course.DepartmentName" type="primary">{{
            row.Course.DepartmentName
          }}</el-tag> /
           <el-tag v-if="row.Course.SubjectName" type="info">{{
            row.Course.SubjectName
          }}</el-tag>
        </template></ElTableColumn
      >
      <ElTableColumn
        prop="Course.CourseName"
        width="200"
        label="课程名"
        show-overflow-tooltip
      ></ElTableColumn>
       <ElTableColumn
        prop="Course.TeacherName"
        width="100"
        label="教师"
        show-overflow-tooltip
      ></ElTableColumn>
     
      <ElTableColumn prop="Student" label="学生" width="400">
        <template v-slot="{ row }">
           <el-tag  size="small" type="warning" >{{
           row.Student.FullName
          }}</el-tag>
         <el-tag  size="small" type="info">{{
           row.Student.StudentIdentityCode
          }}</el-tag>
          {{row.Student.RealName}}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="IsDefault" label="操作" width="120">
        <template v-slot="{ row }">
         <ElButton
            size="small"
            v-if="hasDeletePermission"
            icon="el-icon-delete"
            round
            type="danger"
            plain
            @click.native="deleteitem(row)"
            >删除
          </ElButton>
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
      cid:undefined,
      intFlag:undefined
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("CP_G");
    },

    
 
    hasDeletePermission() {
      return this.hasPermission("CP_D");
    },
  },
  mounted() {
    this.reload();
  },
  methods: {
    
    deleteitem(row) {
      if (!this.hasDeletePermission) return;
      this.$confirm("此操作将永久删除该选课信息, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        this.$axios
          .post(
            this.$baseURL +
              "/coursepick/delete" +
              "?id=" +
              encodeURIComponent(row.Id)
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
     
    reload(e, c, i) {
      if (!this.hasGetPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 3) {
          this.keyword = e;
          this.cid = c;
          this.intFlag = i;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/coursepick/list";
        if (url) {
          var data = {
            keyword: this.keyword,
            page: this.page,
            rows: this.size,
            cid: this.cid,
            intFlag: this.intFlag,
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
