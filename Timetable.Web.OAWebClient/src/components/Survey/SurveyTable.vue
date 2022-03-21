<template>
  <div>
    <ElTable :data="list" highlight-current-row>
      <ElTableColumn show-overflow-tooltiplabel label="问卷ID" width="100" prop="Id"></ElTableColumn>
      <ElTableColumn show-overflow-tooltiplabel label="问卷名" prop="Name_CN">
        <template v-slot="{ row }">
          <i v-if="row.PermissionLevel > 0" class="fa fa-diamond" aria-hidden="true"></i>
          {{ row.Name_CN }}
        </template>
        <i class="fa fa-diamond" aria-hidden="true"></i>
      </ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="用户 (账号 / 昵称 / 真名)" prop="NickName">
        <template v-slot="{ row }"> {{ row.Mobile }} / {{ row.NickName }} / {{ row.RealName }} </template>
      </ElTableColumn>

      <ElTableColumn width="100" show-overflow-tooltip label="审核状态" prop="PublishState">
        <template v-slot="{ row }">
          <ElTag :type="colors[row.PublishState]" size="small">
            {{ row.PublishState | state }}
          </ElTag>
        </template>
      </ElTableColumn>
      <ElTableColumn width="100" show-overflow-tooltip label="审核人" prop="EmployeeRealName">
        <template v-slot="{ row }">
          <span v-if="row.EmployeeRealName"> {{ row.EmployeeRealName }}</span>
          <span v-else style="color:#aaa">-</span>
        </template>
      </ElTableColumn>
      <ElTableColumn width="100" show-overflow-tooltip label="运行状态" prop="AcceptAnswer">
        <template v-slot="{ row }">
          <ElTag v-if="row.AcceptAnswer" type="success" size="small"> <i class="el-icon-video-play"></i> 运行中 </ElTag>
          <ElTag v-else type="danger" size="small"> <i class="el-icon-video-pause"></i> 已停止 </ElTag>
        </template>
      </ElTableColumn>
      <ElTableColumn width="100" show-overflow-tooltip label="有效答卷数" prop="AcceptAnswer">
        <template v-slot="{ row }">
          <span> {{ summary.filter((s) => s.SubjectId == row.Id)[0].AnswerCount }}份</span>
        </template>
      </ElTableColumn>
      <ElTableColumn show-overflow-tooltiplabel label="权值" width="100" prop="PermissionLevel">
        <template v-slot="{ row }">
          <span v-if="row.PermissionLevel"> {{ row.PermissionLevel }}</span>
          <span v-else style="color:#aaa">-</span>
        </template>
      </ElTableColumn>
      <ElTableColumn width="180" show-overflow-tooltip label="问卷创建时间" prop="CreateTime">
        <template v-slot="{ row }">
          {{ row.CreateTime | datetime }}
        </template>
      </ElTableColumn>
      <ElTableColumn width="180" label="操作" v-if="infoPermission">
        <template v-slot="{ row }">
          <ElButton v-if="infoPermission" @click="showDialog(row.Id)" plain icon="el-icon-view" round size="small" type="primary">查看</ElButton>

          <el-dropdown v-if="denyPermission || stopPermission || deletePermission || setLevelPermission">
            <el-button round size="small" :style="{ marginLeft: infoPermission ? '10px' : '0px' }"> 更多<i class="el-icon-arrow-down el-icon--right"></i> </el-button>

            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item v-if="denyPermission" icon="el-icon-s-release" @click.native="denySurvey(row.Id)">违规处理</el-dropdown-item>
              <el-dropdown-item v-if="setLevelPermission" icon="el-icon-setting" @click.native="SetLevel(row.Id)">设置访问权值</el-dropdown-item>
              <el-dropdown-item v-if="stopPermission" icon="el-icon-video-pause" :disabled="row.AcceptAnswer == false" @click.native="stopSurvey(row.Id)">停止运行</el-dropdown-item>
              <el-dropdown-item v-if="deletePermission" icon="el-icon-delete" @click.native="deleteSurvey(row.Id)">永久删除</el-dropdown-item>
               <el-dropdown-item v-if="addTaskPermission&&row.PublishState==2" icon="el-icon-copy-document" @click.native="addTask(row.Id)">同步数据到CIQA官网
               </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </ElTableColumn>
    </ElTable>
    <BasicPaginationBar :page.sync="page" :size.sync="size" :total="total" @current-change="reload" @size-change="reload"></BasicPaginationBar>
    <ElDialog v-if="dialogVisiable && paper" title="问卷详情" visible width="920px" top="5vh" :close-on-click-modal="false" @close="dialogVisiable = false">
      <div style=" width:880px;height:75vh">
        <ViewPaper :data="paper" class="desktop"></ViewPaper>
      </div>
    </ElDialog>
  </div>
</template>
<script>
import ViewPaper from "../Paper/ViewPaper";
export default {
  components: {
    ViewPaper,
  },
  data() {
    return {
      page: 1,
      dialogVisiable: false,
      paper: undefined,
      searchOptions: {
        publishState: "",
        acceptAnswer: "",
        keyword: "",
        cid: "-1",
      },
      colors: ["default", "warning", "success", "danger", "info"],
      size: 15,
      total: 0,
      list: [],
      summary: [],
    };
  },
  inject: ["hasPermission"],
  filters: {
    state(v) {
      switch (v) {
        case 0:
          return "从未申请";
        case 1:
          return "待审核";

        case 2:
          return "通过";
        case 3:
          return "不通过";
        case 4:
          return "用户取消";
      }
    },
  },
  computed: {
    getPermission() {
      return this.hasPermission("SV_G");
    },
    infoPermission() {
      return this.hasPermission("SV_I");
    },
    denyPermission() {
      return this.hasPermission("SV_MA");
    },
    stopPermission() {
      return this.hasPermission("SV_MR");
    },
    setLevelPermission() {
      return this.hasPermission("SV_SPL");
    },

    deletePermission() {
      return this.hasPermission("SV_D");
    },
    addTaskPermission() {
      return this.hasPermission("ST_A");
    },
  },
  methods: {
    reload(e) { 
      if (!this.getPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.searchOptions = e;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/survey/list";
        if (url) {
          var data = {
            keyword: this.searchOptions.keyword,
            intFlag: this.searchOptions.publishState,
            boolFlag: this.searchOptions.acceptAnswer,
            page: this.page,
            rows: this.size,
            cid: parseInt(this.searchOptions.cid),
          };

          if (this.searchOptions.dataRange && this.searchOptions.dataRange.length > 0) {
            data.datestart = this.searchOptions.dataRange[0];
            data.dateend = this.searchOptions.dataRange[1];
          }

          this.$axios.post(url, data).then((res) => {
            setTimeout(() => {
              this.$loading().close();
            }, 100);

            this.list = res.data.data.list;
            this.summary = res.data.data.summary;
            this.total = res.data.total;
          });
        }
      });
    },
    showDialog(id) {
      if (!this.infoPermission) return;
      this.$axios.post(this.$baseURL + "/survey/info?id=" + id).then((res) => {
        if (res.data.success) {
          this.paper = res.data.data;
          this.paper.originList = JSON.parse(res.data.data.originList);
          this.dialogVisiable = true;
        } else {
          this.$message({
            message: res.data.msg,
            type: "error",
            offset: 80,
          });
        }
      });
    },
    denySurvey(id) {
      if (!this.denyPermission) return;
      this.$prompt("请输入审核不通过（违规处理）原因：", "违规处理原因", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        inputPattern: /^[.]{0,1024}/,
        inputErrorMessage: "原因最长1024个字符",
      })
        .then(({ value }) => {
          this.$axios.post(this.$baseURL + "/survey/deny", "id=" + id + "&feedback=" + encodeURIComponent(value)).then((res) => {
            this.reload();
          });
        })
        .catch(() => {});
    },
    SetLevel(id) {
      if(!this.setLevelPermission)return;
      this.$prompt("请输入访问权值：", "设置访问权值", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        inputPattern: /^\d{0,9}$/,
        inputErrorMessage: "请输入权值，最小值为0，最大不超过9位数",
        inputPlaceholder: "请输入权值，如输入0或不填，可取消权值。",
      })
        .then(({ value }) => {
          if (value == undefined || value == "") {
            value = 0;
          }
          this.$axios.post(this.$baseURL + "/survey/SetPermissionLevel", "id=" + id + "&permissionLevel=" + encodeURIComponent(value)).then((res) => {
            this.reload();
          });
        })
        .catch(() => {});
    },
    stopSurvey(id) {
      if (!this.stopPermission) return;
      this.$confirm("管理后台不提供运行问卷的功能，停止后只能由用户再次运行问卷，是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          this.$axios.post(this.$baseURL + "/survey/stop", "id=" + id).then((res) => {
            this.reload();
          });
        })
        .catch(() => {});
    },
    deleteSurvey(id) {
      if (!this.deletePermission) return;
      this.$confirm("将彻底删除问卷，删除后用户无法在回收站找回，确认操作?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          this.$axios.post(this.$baseURL + "/survey/delete", "id=" + id).then((res) => {
            this.reload();
          });
        })
        .catch(() => {});
    },
    addTask(id){
      this.$confirm('此操作将创建数据同步任务，具体进度可在【数据同步任务列表】中查看，是否继续?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          var loading= this.$loading({ text: "正在创建任务" });
          this.$axios.post(this.$baseURL + "/survey/addtask", "id=" + id).then((res) => {
            if(res.data.success){
              this.$message.success(res.data.msg)
            }else{
               this.$message.error(res.data.msg)
            }
            this.reload();
            loading.close();
          });
        }).catch(() => {
             
        });
    }
  },
  mounted() {
    this.reload();
  },
};
</script>
