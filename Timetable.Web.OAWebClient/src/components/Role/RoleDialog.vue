<template>
  <ElDialog :title="dialogTitle" :visible.sync="dialogVisible" width="600px" close :close-on-click-modal="false">
    <div>
      <ElForm ref="form" v-bind:model="editRow" label-width="100px" @submit.native.prevent>
        <ElFormItem
          label="角色名"
          prop="Name"
          :rules="[
            { required: true, message: '角色名必填' },
            { min: 1, max: 64, message: '角色名长度应在1~64个字符' },
          ]"
        >
          <ElInput v-model="editRow.Name" placeholder="请输入角色名"></ElInput>
        </ElFormItem>
        <ElFormItem label="备注" prop="Remark" :rules="{ min: 1, max: 128, message: '角色名长度应在1~128个字符' }">
          <ElInput v-model="editRow.Remark" placeholder="可输入备注"></ElInput>
        </ElFormItem>
        <ElFormItem label="可访问权限">
          <ElScrollbar class="xhidden" style="height:280px;overflow:hidden;border:1px solid #ddd;border-radius:5px;">
            <ElTree ref="tree" :data="treeData" width="100" show-checkbox node-key="Id" :props="{ children: 'children', label: 'DisplayName' }"> </ElTree>
          </ElScrollbar>
        </ElFormItem>
      </ElForm>
    </div>
    <span slot="footer" class="dialog-footer">
      <ElButton @click="dialogVisible = false" icon="el-icon-close">取 消</ElButton>
      <ElButton type="primary" @click="save()" icon="el-icon-check">确 定</ElButton>
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
    };
  },
  inject: ["hasPermission"],
  computed: {
    hasEditPermission() {
      return this.hasPermission("R_M");
    },
    hasAddPermission() {
      return this.hasPermission("R_A");
    },
    hasGetPermission() {
      return this.hasPermission("R_PLG");
    },
  },
  mounted() {
    this.reloadTree();
  },
  methods: {
    reloadTree() {
      if (!this.hasGetPermission) return;
      var url = "/permissionline/tree";
      if (url) {
        this.$axios.post(this.$baseURL + url, {}).then((res) => {
          this.treeData = res.data.data;
        });
      }
    },
    /**打开添加窗口 */
    showAddDialog() {
      if (!this.hasAddPermission) return;
      this.dialogTitle = "添加";
      this.treeData = this.treeData.concat();
      this.editRow = { Name: "", Remark: "", Id: "0", LineIds: [] };
      this.dialogVisible = true;
      this.$nextTick(() => {
        this.$refs.tree.setCheckedKeys([]);
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
        this.$refs.tree.setCheckedKeys(this.editRow.LineIds);
        this.$refs["form"].clearValidate();
      });
    },
    /**保存 */
    save() {
      var that = this;
      this.$refs["form"].validate((result) => {
        this.editRow.LineIds = this.$refs.tree.getCheckedKeys().filter((s) => s > 0);
        if (result) {
          var url;
          if (this.editRow.Id == "0") {
            if (!this.hasAddPermission) return;
            url = "/role/add";
          } else {
            if (!this.hasEditPermission) return;
            url = "/role/edit";
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
