<template>
  <ElDialog
    title="选择科目"
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
      row-key="SubjectCode"
      :empty-text="hasGetPermission ? '暂无数据' : '无权访问'"
      :data="tableData"
      highlight-current-row 
    >
     <ElTableColumn
        prop="SubjectCode"
          min-width="50px"
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
          <el-tag v-if="row.CollegeName" size="small" type="success">{{row.CollegeName}}</el-tag>

          <el-tag v-if="row.DepartmentName" size="small" type="primary">{{row.DepartmentName}}</el-tag>
        </template></ElTableColumn>
     
      <ElTableColumn prop="Enable" label="科目类型" width="90">
        <template v-slot="{ row }">
          <el-tag size="small"  type="info">{{type[row.SubjectType]}}</el-tag>
         
        </template>
      </ElTableColumn>
       <ElTableColumn prop="Enable" label="必修类型" width="90">
        <template v-slot="{ row }">
          <el-tag  size="small" type="info">{{prop[row.SubjectProperty]}}</el-tag> 
        </template>
      </ElTableColumn>
      <ElTableColumn prop="Enable" label="状态" width="80">
        <template v-slot="{ row }">
          <el-tag size="small"  v-if="row.Enable" type="success">启用</el-tag>
          <el-tag size="small" v-if="!row.Enable" type="danger">禁用</el-tag>
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
       type:['未知','通识课','专业课','实践课'],
      prop:['未知','必修','选修']
    };
  },
  computed: {
    hasGetPermission() {
      return this.hasPermission("C_GS");
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
        var url = this.$baseURL + "/subject/enablelist";
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
         
         SubjectCode:e.SubjectCode,
          Name:[e.CollegeName,e.DepartmentName,e.Name].filter(s=>s).join('-')
         });
        this.visiable=false;
    }
  },
};
</script>
