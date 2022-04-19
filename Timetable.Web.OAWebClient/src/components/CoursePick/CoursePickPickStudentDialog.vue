<template>
  <ElDialog
    title="选择学生"
    :lock-scroll="false"
    :visible.sync="visiable"
    width="1000px"
     top="50px"
    :close-on-click-modal="false"
  >
    <BasicToolbar
      :showAddButton="false"
      ref="toolbar"
      @search="reload"
      @reload="reload"
    />
     <ElTable
      size="mini"
      height="550px"
      default-expand-all
      row-key="IdentityCode"
      :empty-text="hasGetPermission ? '暂无数据' : '无权访问'"
      :data="tableData"
      highlight-current-row 
    >
    <ElTableColumn
        prop="IdentityCode"
        width="200"
        label="学号"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="Mobile"
        min-width="50"
        label="手机号"
        show-overflow-tooltip
      ></ElTableColumn>
      <ElTableColumn
        prop="RealName"
        min-width="50px"
        label="真名"
        show-overflow-tooltip
      ></ElTableColumn>
        <ElTableColumn label="入学年份" width="100" prop="AdmissionYear"></ElTableColumn>
       <ElTableColumn
        prop="CollegeName"
        label="院系班级"
        show-overflow-tooltip
      >
       <template v-slot="{ row }">
          <el-tag v-if="row.CollegeName" type="success">{{row.CollegeName}}</el-tag>

          <el-tag v-if="row.DepartmentName" type="primary">{{row.DepartmentName}}</el-tag>
           <el-tag v-if="row.AdministrativeClassName" type="warning">{{row.AdministrativeClassName}}</el-tag>
        </template></ElTableColumn>
        
      
      <!-- <ElTableColumn prop="CreateTime" label="创建时间" width="180">
        <template v-slot="{ row }">
          {{ row.CreateTime | datetime }}
        </template>
      </ElTableColumn> -->
      <ElTableColumn prop="Gender" label="性别" width="80">
        <template v-slot="{ row }">
          <el-tag type="info">{{ gender[row.Gender] }}</el-tag>
        </template>
      </ElTableColumn>

      <ElTableColumn prop="StudentState" label="状态" width="80">
        <template v-slot="{ row }">
          <el-tag v-if="row.StudentState==0" type="success">在读</el-tag>
          <el-tag v-else-if="row.StudentState==1" type="warning">毕业</el-tag>
           <el-tag v-else-if="row.StudentState==2" type="info">休学</el-tag>
            <el-tag v-else-if="row.StudentState==3" type="danger">辍学</el-tag>
        </template>
      </ElTableColumn>
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
      page:1,
      size:15,
      total:0,
      keyword: "",
       gender: ["未知", "男", "女"],
      prop:['在读','毕业','解散']
    };
  },
  computed: {
    hasGetPermission() {
      return this.hasPermission("CP_GS");
    },
  },

  methods: {
    show() {
      this.visiable = true; 
      this.$nextTick(()=>{
        this.$refs.toolbar.emptyKeyword();
      })
    },
    reload(e) {
      if (!this.hasGetPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = e;
          this.page=1; 
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/student/list";
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
    pick(e){ 
         this.$emit('pick', {
          IdentityCode:e.IdentityCode,
          StudentText: e.AdministrativeClassName+"-"+ e.RealName
         });
        this.visiable=false;
    }
  },
};
</script>
