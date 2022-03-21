<template>
  <div>
    <ElTable :data="list" highlight-current-row>
      <!-- <ElTableColumn label="ID" prop="Id"></ElTableColumn> -->
       <ElTableColumn 
      type="index"
      width="90"></ElTableColumn>
      <ElTableColumn label="关键词" prop="Keyword"></ElTableColumn>
      <ElTableColumn label="搜索点击量" prop="Usage"></ElTableColumn>
      <ElTableColumn label="操作" width="180" v-if="hasDeletePermission||hasEditPermission">
        <template v-slot="scope" >
          <ElButton @click="showEditDialog(scope.row)" round size="small" icon="el-icon-edit" plain type="warning" v-if="hasEditPermission">修改</ElButton>
          <ElButton type="danger" icon="el-icon-delete" round size="small" plain v-if="hasDeletePermission" @click="deleteitem(scope.row)">删除</ElButton>
        </template>
      </ElTableColumn>
    </ElTable>
    <BasicPaginationBar :page.sync="page" :size.sync="size" :total="total" @current-change="reload" @size-change="reload"></BasicPaginationBar>
  </div>
</template>
<script>
export default {
  data() {
    return {
      list: [],
      keyword: "",
      editData: {},
      page: 1,
      size: 10,
      total: 0,
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasEditPermission() {
      return this.hasPermission("T_M");
    },
    hasGetPermission() {
      return this.hasPermission("T_G");
    },
    hasDeletePermission() {
      return this.hasPermission("T_D");
    },
  },
  mounted() {
    this.reload();
  },
  methods: {
      deleteitem(e){
          if(this.hasDeletePermission){
              this.$confirm('确认删除关键字【'+e.Keyword+'】', '确认删除', { 
                  type: 'warning'
              }).then(() => {
                   this.$axios.post(this.$baseURL+'/tag/delete',"id="+e.Id).then(res=>{
                  this.reload();
              })
              }).catch(() => {});


             
          }
      },
      showEditDialog(e){
          this.$emit('openEdit',e)
      },
    reload: function(e) {
      if (!this.hasGetPermission) return;
      if (arguments.length > 0) {
        this.keyword = e;
        this.page = 1;
      }
      this.$nextTick(() => {
        var url = "/tag/list";
        if (url) {
          var loading = this.$loading({ text: "正在加载表格数据" });
          this.$axios
            .post(this.$baseURL + url, {
              keyword: this.keyword,
              page: this.page,
              rows: this.size,
            })
            .then((res) => {
              setTimeout(() => {
                loading.close();
              }, 100);

              this.list = res.data.data;
              this.total = res.data.total;
            });
        }
      });
    },
  },
};
</script>
