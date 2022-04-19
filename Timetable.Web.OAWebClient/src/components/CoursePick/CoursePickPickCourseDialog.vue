<template>
  <ElDialog
    title="选择学生"
    :lock-scroll="false"
    :visible.sync="visiable"
    width="1250px"
    top="50px"
    :close-on-click-modal="false"
  >
    <BasicToolbar
      :showAddButton="false"
      ref="toolbar"
      @search="reload"
      @reload="reload"
    >
    <template #default>
        学年：<ElSelect @change="reload(keyword)" style="margin-right:10px" v-model="cid" size="medium">
          <ElOption
            v-for="year in years"
            :key="year.t"
            :label="year.t"
            :value="year.v"
          ></ElOption>
        </ElSelect>
        学期：<ElSelect  @change="reload(keyword)"
          style="margin-right:10px"
          v-model="intFlag"
          size="medium"
        >
          <ElOption label="不限制" :value="undefined"></ElOption>
          <ElOption label="第一学期" :value="'1'"></ElOption>
          <ElOption label="第二学期" :value="'2'"></ElOption>
        </ElSelect>
      </template>
    </BasicToolbar>
    <ElTable
      size="mini"
      height="550px"
      default-expand-all
      row-key="Id"
      :empty-text="hasGetPermission ? '暂无数据' : '无权访问'"
      :data="tableData"
      highlight-current-row
    >
      <ElTableColumn prop="CollegeId" label="开课院系" show-overflow-tooltip>
        <template v-slot="{ row }">
          <el-tag size="small"  v-if="row.CollegeName" type="success">{{
            row.CollegeName
          }}</el-tag>

          <el-tag size="small"  v-if="row.DepartmentName" type="primary">{{
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
      <ElTableColumn prop="Term" label="学期">
        <template v-slot="{ row }">
          {{ row.Year + "学年 第" + row.Term + "学期" }}
        </template>
      </ElTableColumn>
      <ElTableColumn prop="TeacherName" label="授课教师">
        <template v-slot="{ row }">
          <ElTag size="small" type="info">{{ row.TeacherIdentityCode }}</ElTag>
          {{ row.TeacherName }}
        </template>
      </ElTableColumn>
      <ElTableColumn
        prop="CollegeName"
        label="授课班级"
        min-width="100"
        show-overflow-tooltip
      >
        <template v-slot="{ row }">
          <el-tag size="small" v-if="row.AdministrativeClassName" type="warning">{{
            row.AdministrativeClassName
          }}</el-tag>
        </template></ElTableColumn
      >
      <ElTableColumn prop="IsDefault" label="操作" width="110">
        <template v-slot="{ row }">
          <ElButton
            style="margin-right:10px;"
            @click="pick(row)"
            type="warning"
            round
            plain
            size="small"
            icon="el-icon-edit"
            >选中</ElButton
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
  </ElDialog>
</template>
<script>
export default {
  inject: ["hasPermission"],
  data() {
    return {
      visiable: false,
      tableData: [],
       years: [{ t: "不限制", v: undefined }],
      page: 1,
      size: 15,
      total: 0,
      keyword: "", 
      cid:undefined,
      intFlag:undefined
    };
  },
  computed: {
    hasGetPermission() {
      return this.hasPermission("CP_GC");
    },
  },
   mounted() {
    for (var i = 2021; i < 2100; i++) {
      this.years.push({ t: i, v: i.toString() });
    }
  },
  methods: {
    show() {
      this.visiable = true;
      this.$nextTick(() => {
        this.$refs.toolbar.emptyKeyword();
      });
    },
    reload(e) {
      if (!this.hasGetPermission) return;
      this.$nextTick(() => {
        if (arguments.length ==1) {
          this.keyword = e;
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
    pick(e) {
      this.$emit("pick", {
        Id: e.Id,
        CourseText: e.CourseName||e.Name
      });
      this.visiable = false;
    },
  },
};
</script>
