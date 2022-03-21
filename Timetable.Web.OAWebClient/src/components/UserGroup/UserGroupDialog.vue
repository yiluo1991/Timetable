<template>
  <ElDialog
    :title="dialogTitle"
    :visible.sync="dialogVisible"
    width="600px"
    close
    :close-on-click-modal="false"
  >
    <div>
      <ElForm
        ref="form"
        v-bind:model="editRow"
        label-width="100px"
        @submit.native.prevent
      >
        <ElFormItem
          label="用户组名"
          prop="Name"
          :rules="[
            { required: true, message: '用户组名必填' },
            { min: 1, max: 64, message: '用户组名长度应在1~64个字符' },
          ]"
        >
          <ElInput
            v-model="editRow.Name"
            placeholder="请输入用户组名"
          ></ElInput>
        </ElFormItem>
        <ElFormItem label="是否默认" prop="IsDefault">
          <ElSwitch v-model="editRow.IsDefault"> </ElSwitch>
        </ElFormItem>
        <ElFormItem
          label="排序号"
          prop="SortNum"
          :rules="[
            { required: true, message: '请输入权值' },
            {
              min: 0,
              max: 9999,
              type: 'number',
              message: '排序号必须是0-9999间的整数',
            },
          ]"
        >
          <ElInput
            v-model.number="editRow.SortNum"
            placeholder="请输入排序号，越小越靠前"
          ></ElInput>
        </ElFormItem>
        <ElFormItem
          label="权值"
          prop="PermissionLevel"
          :rules="[
            { required: true, message: '请输入权值' },
            {
              min: 0,
              max: 9999,
              type: 'number',
              message: '排序号必须是0-9999间的整数',
            },
          ]"
        >
          <ElInput
            v-model.number="editRow.PermissionLevel"
            placeholder="可输入权值"
          ></ElInput>
        </ElFormItem>
        <ElFormItem label="注册选项">
          <ElSelect v-model="editRow.Registrable" placeholder="请选择">
            <ElOption :value="true" label="开放注册"></ElOption>
            <ElOption :value="false" label="不开放注册"></ElOption>
          </ElSelect>
        </ElFormItem>
        <ElFormItem label="可访问权限">
          <div>
            <ElCheckboxGroup v-model="editRow.Permissions">
              <ElCheckbox
                v-for="(item, index) in treeData"
                :key="index"
                :label="item.value"
                >{{ item.name }}</ElCheckbox
              >
            </ElCheckboxGroup>
          </div>
        </ElFormItem>
      </ElForm>
    </div>
    <span slot="footer" class="dialog-footer">
      <ElButton @click="dialogVisible = false" icon="el-icon-close"
        >取 消</ElButton
      >
      <ElButton type="primary" @click="save()" icon="el-icon-check"
        >确 定</ElButton
      >
    </span>
  </ElDialog>
</template>
<script>
export default {
  data() {
    return {
      tableData: [],
      treeData: [],
      dialogVisible: false,
      dialogTitle: "添加",
      editRow: {},
      Permissions: [],
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasEditPermission() {
      return this.hasPermission("UG_M");
    },
    hasAddPermission() {
      return this.hasPermission("UG_A");
    },
    hasGetPermission() {
      return this.hasPermission("UG_PLG");
    },
  },
  mounted() {
    this.reloadTree();
  },
  methods: {
    reloadTree() {
      this.treeData = [
        { name: "下载答卷数据", value: "Download" },
        { name: "允许分享调查结果时开放自主排序", value: "ShareSortReport" },
        // {name:"允许创建专业问卷",value:"CreateProSurvey"},
        { name: "允许专业报告生成", value: "UseProSurvey" },
        { name: "允许保存专业报告", value: "SaveProSurvey" },
        { name: "允许下载专业报告", value: "DownloadProSurvey" },
       // { name: "允许授权后使用数据在线系统", value: "SPSS" },
      ];
    },
    /**打开添加窗口 */
    showAddDialog() {
      if (!this.hasAddPermission) return;
      this.dialogTitle = "添加";
      this.editRow = {
        Name: "",
        Remark: "",
        Id: "0",
        LineIds: [],
        Permissions: [],
        IsDefault: false,
        SortNum: 100,
        PermissionLevel: 0,
      };
      this.dialogVisible = true;
      this.$nextTick(() => {
        //this.$refs.tree.setCheckedKeys([]);
        this.$refs["form"].clearValidate();
      });
    },
    /**打开修改窗口 */
    showEditDialog(row) {
      if (!this.hasEditPermission) return;
      this.dialogTitle = "修改";
      this.treeData = this.treeData.concat();
      this.editRow = row;
      this.dialogVisible = true;
      this.$nextTick(() => {
        // this.$refs.tree.setCheckedKeys(this.editRow.LineIds);
        this.$refs["form"].clearValidate();
      });
    },
    /**保存 */
    save() {
      var that = this;
      this.$refs["form"].validate((result) => {
        if (result) {
          var url;
          if (this.editRow.Id == "0") {
            if (!this.hasAddPermission) return;
            url = "/usergroup/add";
          } else {
            if (!this.hasEditPermission) return;
            url = "/usergroup/edit";
          }
          var loading = this.$loading({ text: "正在处理请求" });
          this.$axios
            .post(this.$baseURL + url, this.editRow, (data) => {})
            .then((res) => {
              that.dialogVisible = false;
              if (res.data.success) {
                that.$message({
                  type: "success",
                  message: res.data.msg,
                });
              } else {
                that.$message({
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
  },
};
</script>
