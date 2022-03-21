<template>
  <div>
    <ElTable :data="list" highlight-current-row>
      <ElTableColumn show-overflow-tooltip label="模板ID" width="100" prop="Id"></ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="模板名" prop="TemplateName">
        <template v-slot="{row}">
          <ElTag type="warning" size="mini" title="专业模板" v-if="row.IsProTemplate">Pro</ElTag>  {{row.TemplateName}}
        </template>
      </ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="关键词" prop="CategoryKeywords"> </ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="申请附言" width="100" prop="Apply"> </ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="审核意见" width="100" prop="DenyFeedback"></ElTableColumn>
      <ElTableColumn show-overflow-tooltip width="100" label="排序号" prop="SortNum"></ElTableColumn>
      <ElTableColumn show-overflow-tooltip width="100" label="热度" prop="Useage"></ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="用户 (账号 / 昵称 / 真名)" prop="NickName">
        <template v-slot="{ row }"> {{ row.Mobile }} / {{ row.NickName }} / {{ row.RealName }} </template>
      </ElTableColumn>

      <ElTableColumn width="100" show-overflow-tooltip label="审核状态" prop="SubjectTemplateStatus">
        <template v-slot="{ row }">
          <ElTag :type="colors[row.SubjectTemplateStatus]" size="small">
            {{ row.SubjectTemplateStatus | state }}
          </ElTag>
        </template>
      </ElTableColumn>
      <ElTableColumn width="100" show-overflow-tooltip label="可用状态" prop="Enable">
        <template v-slot="{ row }">
          <ElTag   :type="row.Enable ? 'success' : 'danger'" size="small">
            {{ row.Enable ? "可用" : "禁用" }}
          </ElTag>
          
        </template>
      </ElTableColumn>
      <ElTableColumn width="100" show-overflow-tooltip label="审核人" prop="auditEmployee">
        <template v-slot="{ row }">
          <span v-if="row.LastAuditEmployeeName"> {{ row.LastAuditEmployeeName }}</span>
          <span v-else style="color:#aaa">-</span>
        </template>
      </ElTableColumn>

      <ElTableColumn width="180" label="操作" v-if="infoPermission || deletePermission">
        <template v-slot="{ row }">
          <ElButton v-if="infoPermission" @click="showDialog(row.Id)" plain icon="el-icon-view" round size="small" type="primary">查看</ElButton>

          <ElButton v-if="deletePermission" icon="el-icon-delete" plain round size="small" type="danger" @click.native="deleteSurvey(row.Id)">删除</ElButton>
        </template>
      </ElTableColumn>

    </ElTable>
    <BasicPaginationBar :page.sync="page" :size.sync="size" :total="total" @current-change="reload" @size-change="reload"></BasicPaginationBar>
    
  </div>
</template>
<script>
 
export default {
  components: {
 
  },
  data() {
    return {
      page: 1,
     
      paper: undefined,
      searchOptions: {
        publishState: "",
        acceptAnswer: "",
        keyword: "",
      },
      colors: ["warning", "success", "danger"],
      size: 15,
      total: 0,
      list: [],
    };
  },
  inject: ["hasPermission"],
  filters: {
    state(v) {
      switch (v) {
        case 0:
          return "待审核";
        case 1:
          return "审核通过";
        case 2:
          return "不通过";
      }
    },
  },
  computed: {
    getPermission() {
      return this.hasPermission("TP_G");
    },
    infoPermission() {
      return this.hasPermission("TP_I");
    },
    deletePermission() {
      return this.hasPermission("TP_D");
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
        var url = this.$baseURL + "/template/list";
        if (url) {
          var data = {
            keyword: this.searchOptions.keyword,
            intFlag: this.searchOptions.status,
            boolFlag: this.searchOptions.enable,
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
    showDialog(id) {
      if (!this.infoPermission) return;
      this.$axios.post(this.$baseURL + "/template/info?id=" + id).then((res) => {
        if (res.data.success) {
          this.paper = res.data.data;
          this.paper.originList = JSON.parse(res.data.data.originList);
          this.paper.basicInfo={
              Name_CN:this.paper.TemplateName,
              Introduction_CN:this.paper.Introduction
          }
          this.$emit('showDialog',this.paper)
        } else {
          this.$message({
            message: res.data.msg,
            type: "error",
            offset: 80,
          });
        }
      });
    },

    deleteSurvey(id) {
      if (!this.deletePermission) return;
      this.$confirm("将删除模板，确认操作?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          this.$axios.post(this.$baseURL + "/template/delete", "id=" + id).then((res) => {
            this.reload();
          });
        })
        .catch(() => {});
    },
  },
  mounted() {
    this.reload();
  },
};
</script>

<style>
.nowrap div {
  white-space: nowrap !important;
}
</style>
