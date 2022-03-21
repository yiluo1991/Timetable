<template>
  <el-dialog
    title="数据同步任务列表"
    :visible.sync="visiable"
    width="1000px"
    @close="visiable = false"
    :close-on-click-modal="false"
  >
    <template #title>
      <span class="el-dialog__title" style="margin-right:20px"
        >数据同步任务列表</span
      >
      <ElSwitch
        @change="change"
        v-model="autoReload"
        active-text="自动刷新"
        inactive-text="停止刷新"
      ></ElSwitch>
    </template>

    <ElTable :data="data" size="mini">
      <ElTableColumn
         
        label="问卷ID"
        width="80"
        prop="SubjectDesignId"
      ></ElTableColumn>
      <ElTableColumn
        label="问卷名"
        width="150"
        show-overflow-tooltip
        prop="Name_CN"
      ></ElTableColumn>

      <ElTableColumn label="已同步/总答卷" align="center" width="150">
        <template v-slot="{ row }">
          <div v-if="row.DataSyncTaskState > 0">
            {{ row.Finished }} / {{ row.Total }}
          </div>
            <div v-else>
          -
          </div>
        </template>
      </ElTableColumn>
      <ElTableColumn label="状态" width="120">
        <template v-slot="{ row }">
          <ElTag type="info" v-if="row.DataSyncTaskState == 0">待启动</ElTag>
          <ElTag type="warning" v-if="row.DataSyncTaskState == 1">运行中</ElTag>
          <ElTag type="success" v-if="row.DataSyncTaskState == 2">完成</ElTag>
          <ElTag type="danger" v-if="row.DataSyncTaskState == 3">失败</ElTag>
          <ElTag type="info" v-if="row.DataSyncTaskState == 4">用户取消</ElTag>
        </template>
      </ElTableColumn>
      <ElTableColumn label="状态描述"  show-overflow-tooltip>
        <template v-slot="{ row }" > 
           <span v-if="row.DataSyncTaskState > 0"> {{ row.StateDescription }} </span>
        </template>
      </ElTableColumn>
      <ElTableColumn label="开始/结束时间" width="140" prop="SubjectDesignId">
        <template v-slot="{ row }">
          <div v-if="row.StartTime">{{ row.StartTime | datetime }}</div>
          <hr style="height:1px;border:none;background:#ddd;margin:0;">
          <div v-if="row.EndTime">{{ row.EndTime | datetime }}</div>
        </template>
      </ElTableColumn>
      <ElTableColumn label="操作" width="120px" prop="SubjectDesignId">
        <template v-slot="{ row }">
          <ElButton
            round
            size="mini"
            plain
            icon="el-icon-circle-close"
            type="warning"
            @click="dealTask(row.Id,'cancel')"
            v-if="row.DataSyncTaskState == 1"
            >取消任务</ElButton
          >
          <ElButton
            round
            size="mini"
            plain
            icon="el-icon-delete"
            type="danger"
            @click="dealTask(row.Id,'delete')"
            v-if="row.DataSyncTaskState != 1"
            >删除记录</ElButton
          >
        </template>
      </ElTableColumn>
    </ElTable>
    <BasicPaginationBar
      :page.sync="page"
      :size.sync="size"
      :total="total"
      
      @size-change="reload"
    ></BasicPaginationBar>

    <span slot="footer">
      <el-button @click="visiable = false">关闭</el-button>
    </span>
  </el-dialog>
</template>
<script>
export default {
  data() {
    return {
      page: 1,
      size: 10,
      autoReload: true,
      total: 0,
     
      visiable: false,
      data: [],
      loadingFlag: false,
    };
  },
watch:{
    page(){
        this.reload();
    }
},
  methods: {
    change(e) {
      this.reload();
    },
    dealTask(id,m){
            this.$axios
            .post(this.$baseURL + `/survey/${m}task`, 'id='+id).then(res=>{
                if(!res.data.success){
                    this.$message.error(res.data.msg)
                } else{
                      this.$message.success(res.data.msg)
                }
                this.reload();
            })
        
    },
    reload() {
        console.log(1)
      this.$nextTick(() => {
     
        if (!this.loadingFlag&&this.visiable) {
               this.loadingFlag = true;
          this.$axios
            .post(this.$baseURL + "/survey/tasklist", {
              page: this.page,
              rows: this.size,
            })
            .then((res) => {
                   this.loadingFlag = false;
              this.data = res.data.data;
              this.total = res.data.total;
              if (this.autoReload) {
                setTimeout(() => {
                  this.reload();
                },1000);
              }
            });
        }
      });
    },
    showDialog() {
      this.visiable = true;
      this.load();
    },
    load() {
      this.page = 1; 
      this.total = 0;
      this.data = [];
      this.reload();
    },
  },
  mounted() {
    this.load();
  },
};
</script>
