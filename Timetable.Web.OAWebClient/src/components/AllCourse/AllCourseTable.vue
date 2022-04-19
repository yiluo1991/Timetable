<template>
  <div>
    <ElTable
      default-expand-all
      row-key="Id"
      :empty-text="hasGetPermission ? '暂无数据' : '无权访问'"
      :data="tableData"
      highlight-current-row
    >
      <ElTableColumn prop="CollegeId" label="开课院系" show-overflow-tooltip>
        <template v-slot="{ row }">
          <el-tag  v-if="row.CollegeName" type="success">{{
            row.CollegeName
          }}</el-tag>

          <el-tag v-if="row.DepartmentName" type="primary">{{
            row.DepartmentName
          }}</el-tag>
        </template></ElTableColumn
      >
       <ElTableColumn
        prop="Name"
        width="200"
        label="科目"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="CourseName"
        width="200"
        label="课程全名"
        show-overflow-tooltip
      ></ElTableColumn>
       <ElTableColumn
        prop="Address"
        width="160"
        label="上课地点"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn prop="Term" label="学期">
        <template v-slot="{ row }">
          {{ row.Year + "学年 第" + row.Term + "学期" }}
        </template>
      </ElTableColumn>
 <ElTableColumn prop="TeacherName" label="授课教师">
        <template v-slot="{ row }">
          <ElTag size="small" type="info">{{row.TeacherIdentityCode}}</ElTag>
          {{   row.TeacherName }}
        </template>
      </ElTableColumn>
      <ElTableColumn
        prop="CollegeName"
        label="授课班级"
        min-width="100"
        show-overflow-tooltip
      >
        <template v-slot="{ row }">
          <el-tag  v-if="row.AdministrativeClassName" type="warning">{{
            row.AdministrativeClassName
          }}</el-tag>
        </template></ElTableColumn
      >
      <ElTableColumn prop="IsDefault" label="操作" width="120">
        <template v-slot="{ row }">
          <ElButton
            size="small"
            v-if="hasGetPermission"
            icon="el-icon-view"
            round
            type="success"
            plain
            @click.native="view(row)"
            >查看排课
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
      cid:undefined,
      intFlag:undefined
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("C_GAC");
    },
  },
  mounted() {
    this.reload();
  },
  methods: { 
    view(row){
      if(this.hasGetPermission){
      this.$emit('view',JSON.parse(JSON.stringify(row)))
      }
     
    },
    reload(e,c,i) {
      if (!this.hasGetPermission) return;
      this.$nextTick(() => {
        if (arguments.length ==3) {
          this.keyword = e;
          this.cid=c;
          this.intFlag=i;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/course/alllist";
        if (url) {
          var data = {
            keyword: this.keyword,
            page: this.page,
            rows: this.size,
            cid:this.cid,
            intFlag:this.intFlag
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
