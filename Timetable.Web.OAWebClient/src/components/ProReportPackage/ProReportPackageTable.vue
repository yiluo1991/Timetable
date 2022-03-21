<template>
  <div>
    <ElTable :data="list" highlight-current-row row-key="Id">
      <ElTableColumn
        show-overflow-tooltip
        label="归档ID"
        width="80"
        prop="Id"
      ></ElTableColumn>
      <ElTableColumn
        show-overflow-tooltip
        label="归档包名称"
        prop="PackageName"
      >
        <template v-slot="{ row }">
           <ElButton
            v-if="editPermission"
            type="text"
            icon="el-icon-edit"
            @click="editName(row)"
          ></ElButton>
          {{ row.PackageName }}
          </template
      ></ElTableColumn>
      <ElTableColumn
        show-overflow-tooltip
        label="问卷模板名"
        prop="TemplateName"
        width="200"
      >
        <template v-slot="{ row }">
          {{ row.TemplateName }} [ID:{{ row.SubjectTemplateId }}]
        </template>
      </ElTableColumn>
        <ElTableColumn
        show-overflow-tooltip
        label="报告模板名"
        prop="TemplateName"
        width="150"
      >
        <template v-slot="{ row }">
          {{ row.GroupItemName }} 
        </template>
      </ElTableColumn>
       <ElTableColumn
        show-overflow-tooltip
        label="归档创建人"
        width="100"
        prop="RealName"
      ></ElTableColumn>
      <ElTableColumn label="创建时间" width="180">
        <template v-slot="{ row }">
          {{ row.CreateTime | datetime }}
        </template>
      </ElTableColumn>
      <ElTableColumn label="归档报告数" width="90">
        <template v-slot="{ row }">
          {{ row.PapersCount }}
        </template>
      </ElTableColumn>
      <ElTableColumn label="状态" width="120">
        <template v-slot="{ row }">
          <ElTag type="info" v-if="row.DataSyncTaskState == 0">待启动</ElTag>
          <div v-if="row.DataSyncTaskState == 1">
            <ElTag type="warning">运行中</ElTag>
            <el-progress
              type="circle"
              style="vertical-align:middle;margin-left:3px;"
              :width="30"
              :percentage="
                ((row.SucceedCount + row.FailedCount) / row.Total) * 100
              "
              :show-text="false"
            ></el-progress>
          </div>
          <ElTag type="success" v-if="row.DataSyncTaskState == 2">完成</ElTag>
          <ElTag type="danger" v-if="row.DataSyncTaskState == 3">失败</ElTag>
          <ElTag type="info" v-if="row.DataSyncTaskState == 4">用户取消</ElTag>
        </template>
      </ElTableColumn>
      <ElTableColumn label="状态描述" width="160" show-overflow-tooltip>
        <template v-slot="{ row }">
          <span v-if="row.DataSyncTaskState > 0">
            {{ row.StateDescription }}
          </span>
          <span v-else style="color:#aaa"> -- 无 -- </span>
        </template>
      </ElTableColumn>
      <ElTableColumn label="开始/结束时间" width="140" prop="SubjectDesignId">
        <template v-slot="{ row }">
          <div v-if="row.StartTime" style="font-size:12px">{{ row.StartTime | datetime }}</div>
          <hr
            v-if="row.StartTime || row.EndTime"
            style="height:1px;border:none;background:#ddd;margin:0;"
          />
          <span v-else style="color:#aaa"> -- 无 -- </span>
          <div v-if="row.EndTime" style="font-size:12px">{{ row.EndTime | datetime }}</div>
        </template>
      </ElTableColumn>
      <ElTableColumn label="操作" width="240px" prop="SubjectDesignId" v-if="deletePermission||cancelPermission||getPaperListPermission">
        <template v-slot="{ row }">
          <ElButton
            round
            size="mini"
            plain
            icon="el-icon-circle-close"
            type="warning"
            @click="dealTask(row.Id, 'canceltask')"
            v-if="row.DataSyncTaskState == 1&&cancelPermission"
            >取消任务</ElButton
          >
          <ElButton
            round
            size="mini"
            plain
            icon="el-icon-delete"
            type="danger"
            @click="dealTask(row.Id, 'delete')"
            v-if="row.DataSyncTaskState != 1&&deletePermission"
            >删除记录</ElButton
          >
          <ElButton
            round
            size="mini"
            plain
            icon="el-icon-folder-opened
"           @click="showPaperListDialog(row)"
            type="primary" 
            v-if="row.DataSyncTaskState == 2&& getPaperListPermission"
            >查看报告</ElButton
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
  destroyed() {
    //ProreportPackage.vue页面name，keepalive不生效，可以触发
    this.destroyed = true;
  },

  components: {},
  data() {
    return {
      timer: undefined,
      page: 1,
      paper: undefined,
      keyword: "",
      size: 15,
      destroyed: false,
      total: 0,
      list: [],
    };
  },
  inject: ["hasPermission"],

  computed: {
    getPermission() {
      return this.hasPermission("PRP_G");
    },
    editPermission() {
      return this.hasPermission("PRP_M");
    },
    cancelPermission() {
      return this.hasPermission("PRP_C");
    },
    deletePermission() {
      return this.hasPermission("PRP_D");
    },
    getPaperListPermission(){
      return this.hasPermission("PRP_PLG")
    }
  },
  methods: {
    dealTask(id, m) {
      var url;
      var fn = () => {
        this.$axios
          .post(this.$baseURL + `/proreportpackage/${m}`, "id=" + id)
          .then((res) => {
            if (!res.data.success) {
              this.$message.error(res.data.msg);
            } else {
              this.$message.success(res.data.msg);
            }
            this.reload();
          });
      };
      if (
        (m == "canceltask" && this.cancelPermission) ||
        (m == "delete" && this.deletePermission)
      ) {
        if (m == "delete") {
          this.$confirm("此操作将永久删除该归档, 是否继续?", "提示", {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning",
          })
            .then(() => {
              fn()
            })
            .catch(() => {});
        } else {
           fn()
        }
      }
    },
    acceptChange() {
      if (!this.getPermission) return;

      this.$nextTick(() => {
        var data = {
          keyword: this.keyword,
          page: this.page,
          rows: this.size,
        };

        this.$axios
          .post(this.$baseURL + "/proreportpackage/list", data)
          .then((res) => {
            var list = res.data.data.list;

            list.forEach((item) => {
              var target = this.list.find((s) => s.Id == item.Id);
              if (target) {
                (target.DataSyncTaskState = item.DataSyncTaskState),
                  (target.EndTime = item.EndTime);
                target.StartTime = item.StartTime;
                target.FailedCount = item.FailedCount;
                target.Total = item.Total;
                target.PapersCount = item.PapersCount;
                target.SucceedCount = item.SucceedCount;
                target.StateDescription = item.StateDescription;
              }
            });
            if (!this.destroyed) {
              this.timer = setTimeout(() => {
                this.acceptChange();
              }, 2000);
            }
          });
      });
    },
    showPaperListDialog(row){
      if(!this.getPaperListPermission)return;
      window.open('/proreport/'+row.Id)
    },
    reload(e) {
      if (!this.getPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = e;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/proreportpackage/list";
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
            this.list = res.data.data.list;
            this.total = res.data.total;
          });
        }
      });
    },
    editName(row){
      if(!this.editPermission) return;
       this.$prompt('请输入新的归档名称', '修改归档名', {
          confirmButtonText: '确定',
          closeOnClickModal:false,
          cancelButtonText: '取消',
          inputPattern: /^.{1,128}$/,
          inputErrorMessage: '请输入归档名称,最多128个字符'
        }).then(({ value }) => {
          var formData=new FormData();
          formData.append('name',value);
          formData.append('id',row.Id);
          this.$axios.post(this.$baseURL+'/proreportpackage/editpackagename',formData).then(res=>{
            if(res.data.success){
              this.reload();
            }
          })
        }).catch(() => {
               
        });
    }
  },
  mounted() {
    this.reload();
    this.timer = setTimeout(() => {
      this.acceptChange();
    }, 2000);
  },
};
</script>

<style>
.nowrap div {
  white-space: nowrap !important;
}
</style>
