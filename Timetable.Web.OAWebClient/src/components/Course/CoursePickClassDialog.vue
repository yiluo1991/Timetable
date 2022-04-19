<template>
  <ElDialog
    title="选择班级"
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
      row-key="Id"
      :empty-text="hasGetAdministrativeClassPermission ? '暂无数据' : '无权访问'"
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
      <ElTableColumn prop="IsDefault" label="操作" width="200">
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
      
      prop:['在读','毕业','解散']
    };
  },
  computed: {
    hasGetAdministrativeClassPermission() {
      return this.hasPermission("SU_GAC");
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
      if (!this.hasGetAdministrativeClassPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = e;
          this.page=1; 
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
    pick(e){
         
         this.$emit('pick', {
          CollegeId:e.CollegeId,
          DepartmentId:e.DepartmentId,
          AdministrativeClassId:e.Id,
          ClassText:[
           // e.CollegeName,e.DepartmentName,
            e.FullName].filter(s=>s).join('-')
         });
        this.visiable=false;
    }
  },
};
</script>
