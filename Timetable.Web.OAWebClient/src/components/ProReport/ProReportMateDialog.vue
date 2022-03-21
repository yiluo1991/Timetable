<template>
  <ElDialog top="50px" :title="dialogTitle" :lock-scroll="false" :visible.sync="dialogVisible" width="900px" close :close-on-click-modal="false">
    <div class="form-no-margin">
      <ElForm size="mini" inline inline-message :show-message="false" hide-required-asterisk ref="form" @submit.native.prevent :model="editData" :rules="rules">
        <el-alert  style="padding:0px 10px;" v-if="editData.WebDataProvider" type="info"   :closable="false">
          部分指标可通过运算进行更新，点击<ElButton type="text" @click="showAutoSummaryDialog"> 打开运算界面 </ElButton>
        </el-alert>

        <ElTable size="mini" :data="editData.Mates">
          <ElTableColumn show-overflow-tooltip label="参数标识名" width="150" prop="metaName"></ElTableColumn>
          <ElTableColumn show-overflow-tooltip label="参数类型" width="150">
            <template v-slot="scope">
              {{ scope.row.type == "areaMap" ? "评分标准" : scope.row.type == "number" ? "数字" : "字符串" }}
            </template>
          </ElTableColumn>
          <ElTableColumn show-overflow-tooltip label="参数说明" prop="desc"></ElTableColumn>

          <ElTableColumn show-overflow-tooltip label="值" width="200">
            <template v-slot="scope">
              <ElFormItem v-if="scope.row.type == 'number'" :rules="[{ required: true }]" :prop="'Mates[' + scope.$index + '].num'">
                <ElInputNumber style="width:100%" :controls="false" :max="999999999999999" :min="-999999999999999" v-model="editData.Mates[scope.$index].num" placeholder="请输入数值"></ElInputNumber>
              </ElFormItem>
              <ElFormItem v-else-if="scope.row.type == 'string'" :rules="[{ required: true }, { max: 128 }]" :prop="'Mates[' + scope.$index + '].str'">
                <ElInput maxlength="128" :rules="[{ required: true }]" style="width:100%" v-model="editData.Mates[scope.$index].str" placeholder="请输入文字"></ElInput>
              </ElFormItem>
              <el-button @click="setValue(scope.$index)" size="mini" type="text" v-else>
                查看和设置值
              </el-button>
            </template>
          </ElTableColumn>
        </ElTable>
      </ElForm>
    </div>
    <span slot="footer" class="dialog-footer">
      <ElButton @click="dialogVisible = false" icon="el-icon-close">取 消</ElButton>
      <ElButton type="primary" @click="save()" icon="el-icon-check">确 定</ElButton>
    </span>

    <ProReportAreaMapDialog @update="updateAreaMap" ref="areaMapDialog"></ProReportAreaMapDialog>

    <ProReportMetaAutoSummaryDialog @change="changeMetas" v-if="editData.WebDataProvider" ref="autoSummaryDialog" :item="item"></ProReportMetaAutoSummaryDialog>
  </ElDialog>
</template>
<script>
export default {
  inject: ["hasPermission"],
  components: {
    ProReportAreaMapDialog: () => import("./ProReportAreaMapDialog"),
    ProReportMetaAutoSummaryDialog: () => import("./ProReportMetaAutoSummaryDialog.vue"),
  },
  computed: {
    getMatePermission() {
      return this.hasPermission("PR_GM");
    },
    updateMatePermission() {
      return this.hasPermission("PR_U");
    },
  },
  data() {
    return {
      editData: {},

      dialogTitle: "调整参数",
      dialogVisible: false,
      item: undefined,
      rules: {},
    };
  },

  methods: {
    showAutoSummaryDialog() {
      this.$refs.autoSummaryDialog.show(this.editData.Mates);
    },
    showDialog: function(item) {
      if (this.getMatePermission && this.updateMatePermission) {
        this.item = JSON.parse(JSON.stringify(item));
        this.$axios.post(this.$baseURL + "/proreport/getmate", "id=" + this.item.Id).then((res) => {
          if (res.data.success) {
            this.editData = res.data.data;
            this.dialogVisible = true;
            this.$nextTick(() => {
              this.$refs["form"].clearValidate();
            });
          } else {
            this.$message({
              type: "error",
              message: res.data.msg,
            });
          }
        });
      } else {
        this.$message({
          message: "权限不足",
          type: "error",
        });
      }
    },
    setValue(index) {
      if (!this.updateMatePermission) {
        return;
      }
      var mapRules = this.editData.Mates[index].mapRules;
      this.$refs.areaMapDialog.showDialog(mapRules, index);
    },
    updateAreaMap(data, index) {
      if (!this.updateMatePermission) {
        return;
      }
      this.editData.Mates[index].mapRules = data;
    },
    save: function() {
      if (!this.updateMatePermission) {
        return;
      }
      this.$refs["form"].validate((result) => {
        if (result) {
          var url = "/proreport/UpdateMate";

          var loading = this.$loading({ text: "正在处理请求" });
          this.$axios.post(this.$baseURL + url + `?id=${this.item.Id}`, this.editData.Mates).then((res) => {
            this.dialogVisible = false;
            if (res.data.success) {
              this.$message({
                type: "success",
                message: res.data.msg,
              });
            } else {
              this.$message({
                type: "error",
                message: res.data.msg,
              });
            }
            loading.close();
            this.$emit("reload");
          });
        } else {
          return false;
        }
      });
    },
    changeMetas(newMetas) {
      newMetas.forEach((meta) => {
        var oldMeta = this.editData.Mates.find((s) => s.metaName == meta.metaName);
        if (oldMeta) {
          if (oldMeta.type == "string") {
            oldMeta.str = meta.str;
          } else if (oldMeta.type == "number") {
              oldMeta.num = meta.num;
          }
        }
      });
      console.log(newMetas);
    },
  },
};
</script>
<style lang="less">
.form-no-margin .el-form--inline .el-form-item {
  margin: 0 !important;
}
</style>
