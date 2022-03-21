<template>
  <div>
    <ElTable :data="list" highlight-current-row>
      <ElTableColumn show-overflow-tooltiplabel label="问卷ID" width="100" prop="Id"></ElTableColumn>
      <ElTableColumn show-overflow-tooltiplabel label="问卷名" prop="Name_CN"></ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="用户 (账号 / 昵称 / 真名)" prop="NickName">
        <template v-slot="{ row }"> {{ row.Mobile }} / {{ row.NickName }} / {{ row.RealName }} </template>
      </ElTableColumn>
      <ElTableColumn width="180" show-overflow-tooltip label="问卷创建时间" prop="CreateTime">
        <template v-slot="{ row }">
          {{ row.CreateTime | datetime }}
        </template>
      </ElTableColumn>
      <ElTableColumn width="100" label="操作" v-if="infoPermission">
        <template v-slot="{ row }">
          <ElButton @click="showDialog(row.Id)" plain :icon="auditPermission ? 'el-icon-s-check' : 'el-icon-view'" round size="small" type="warning">{{ auditPermission ? "审核" : "查看" }}</ElButton>
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
      page: 1,
      keyword: "",
      size: 15,
      total: 0,
      list: [],
    };
  },
    inject: ["hasPermission"],
  computed: {
    getPermission() {
      return this.hasPermission("AS_G");
    },
    infoPermission() {
      return this.hasPermission("AS_I");
    },
    auditPermission() {
      return this.hasPermission("AS_A");
    },
  },
  methods: {
    reload(keyword) {
      if (!this.getPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.keyword = keyword;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/auditsurvey/list";
        if (url) {
          this.$axios
            .post(url, {
              keyword: this.keyword,
              page: this.page,
              rows: this.size,
            })
            .then((res) => {
              setTimeout(() => {
                this.$loading().close();
              }, 100);

              this.list = res.data.data;
              this.total = res.data.total;
            });
        }
      });
    },
    showDialog(id) {
       if (!this.auditPermission) return;
      this.$axios.post(this.$baseURL + "/auditsurvey/info?id=" + id).then((res) => {
        if (res.data.success) {
          if (res.data.data.publishState == 1) {
            this.$emit("showDialog", JSON.parse(JSON.stringify(res.data.data)));
          } else {
            this.$message({
              message: "问卷当前不在待审核状态，建议您刷新列表",
              type: "error",
              offset: 80,
            });
          }
        } else {
          this.$message({
            message: res.data.msg,
            type: "error",
            offset: 80,
          });
        }
      });
    },
  },
  mounted() {
    this.reload();
  },
};
</script>
