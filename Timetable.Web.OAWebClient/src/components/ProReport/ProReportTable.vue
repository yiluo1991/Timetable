<template>
  <div>
    <ElTable :data="list" highlight-current-row row-key="Id">
      <ElTableColumn type="expand">
        <template slot-scope="scope">
          <ElRow :gutter="15">
            <ElCol :span="8" v-for="item in scope.row.TemplateGroupItems" :key="item.Id">
              <div style="padding:5px 0px;">
                <ElCard :body-style="{ padding: '0px' }">
                  <div style="padding: 14px;">
                    <div>
                      <p>
                        <strong :style="{ color: item.Visiable ? 'black' : '#aaa' }"> {{ item.Visiable ? "" : "[不可见]" }} {{ item.Name }} </strong>
                        <span style="color:#aaa"> [排序号:{{ item.SortNum }}]</span>
                      </p>
                    </div>
                    <div class=" clearfix" style="text-align:right;">
                      <el-button size="small" type="text" v-if="editPermission" @click="editItem(item)" class="button">修改</el-button>
                      <el-divider direction="vertical" v-if="updateMatePermission && editPermission"></el-divider>
                      <el-button size="small" type="text" v-if="updateMatePermission" @click="editMate(item)"  class="button">调整参数</el-button>
                    </div>
                  </div>
                </ElCard>
              </div>
            </ElCol>
          </ElRow>
        </template>
      </ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="模板ID" width="100" prop="Id"></ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="问卷模板名" prop="TemplateName"></ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="关键词" width="200" prop="CategoryKeywords"> </ElTableColumn>

      <ElTableColumn width="150" show-overflow-tooltip label="问卷模板可用状态" prop="Enable">
        <template v-slot="{ row }">
          <ElButton v-if="changeEnablePermission" plain @click="changeEnable(row.Id, !row.Enable)" :type="row.Enable ? 'success' : 'danger'" size="mini">
            {{ row.Enable ? "可用" : "禁用" }}
          </ElButton>
          <ElTag v-else :type="row.Enable ? 'success' : 'danger'" size="small">
            {{ row.Enable ? "可用" : "禁用" }}
          </ElTag>
        </template>
      </ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="专业报告模板包" prop="TemplateGroupName"> </ElTableColumn>
    </ElTable>
    <BasicPaginationBar :page.sync="page" :size.sync="size" :total="total" @current-change="reload" @size-change="reload"></BasicPaginationBar>
  </div>
</template>
<script>
export default {
  components: {},
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
      return this.hasPermission("PR_G");
    },
    changeEnablePermission() {
      return this.hasPermission("PR_C");
    },
    editPermission() {
      return this.hasPermission("PR_M");
    },
    getMatePermission() {
      return this.hasPermission("PR_GM");
    },
    updateMatePermission() {
      return this.hasPermission("PR_U");
    },
  },
  methods: {
    editItem(item) {
      this.$emit("editItem", item);
    },
    editMate(item){
      this.$emit('editMate',item);
    },
    changeEnable(id, enable) {
      if (!this.changeEnablePermission) return;
      this.$axios.post(this.$baseURL + "/proreport/ChangeEnable?id=" + id + "&enable=" + enable).then((res) => {
        if (res.data.success) {
          this.reload();
        } else {
          this.$message({
            message: res.data.msg,
            type: "error",
          });
          this.reload();
        }
      });
    },
    reload(e) {
      if (!this.getPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.searchOptions = e;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/proreport/list";
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
          this.paper.basicInfo = {
            Name_CN: this.paper.TemplateName,
            Introduction_CN: this.paper.Introduction,
          };
          this.$emit("showDialog", this.paper);
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
