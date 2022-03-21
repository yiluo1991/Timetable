<template>
  <ElDialog :title="dialogTitle" :lock-scroll="false" :visible.sync="dialogVisible" width="600px" close :close-on-click-modal="false">
    <div>
      <ElForm ref="form" label-width="100px" @submit.native.prevent :model="editData" :rules="rules">
        <ElFormItem label="关键词" prop="Keyword">
          <ElInput v-model="editData.Keyword" placeholder="可填写学校名"></ElInput>
        </ElFormItem>
        <ElFormItem label="搜索点击量 " prop="Usage">
          <ElInputNumber class="text-left" style="width:100%;text-align:left;" :controls="false" :min="0" :max="2000000000" v-model="editData.Usage"></ElInputNumber>
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
  inject: ["hasPermission"],
  computed: {
    hasAddPermission() {
      return this.hasPermission("T_A");
    },
    hasEditPermission() {
      return this.hasPermission("T_M");
    },
    hasVerifyPermission() {
      return this.hasPermission("T_V");
    },
  },
  data() {
    return {
      editData: {},

      dialogTitle: "修改",
      dialogVisible: false,
      rules: {
        Keyword: [
          { required: true, message: "关键词必填" },
          { max: 32, message: "长度最大32字符" },
          {
            validator: (field, value, cb) => {
              if (this.hasVerifyPermission) {
                this.$axios.post(this.$baseURL + "/tag/CheckTag", `id=${this.editData.Id}&tag=${encodeURIComponent(value)}`).then((res) => {
                  if (res.data) {
                    cb();
                  } else {
                    cb("关键词已存在");
                  }
                });
              }else{
                  cb('您没有验证关键词重复性的权限')
              }
            },
          },
        ],
        Usage: [{ required: true, message: "搜索点击量必填" }],
      },
    };
  },

  methods: {
    showAddDialog: function() {
      this.dialogTitle = "添加关键词";
      this.editData = {
        Id: 0,
        Keyword: "",
        Usage: 0,
      };
      this.dialogVisible = true;
      this.$nextTick(() => {
        this.$refs["form"].clearValidate();
      });
    },
    showEditDialog: function(row) {
      this.dialogTitle = "修改关键词信息";
      this.dialogVisible = true;
      this.editData = row;
      this.$nextTick(() => {
        this.$refs["form"].clearValidate();
      }, 1000);
    },
    save: function() {
      this.$refs["form"].validate((result) => {
        if (result) {
          var url;
          if (this.editData.Id == "0") {
            if (!this.hasAddPermission) return;
            url = "/tag/add";
            if (!this.hasEditPermission) return;
          } else {
            url = "/tag/edit";
          }
          var loading = this.$loading({ text: "正在处理请求" });
          this.$axios
            .post(this.$baseURL + url, this.editData, (data) => {})
            .then((res) => {
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
  },
};
</script>
<style lang="less">
.text-left {
  .el-input__inner {
    text-align: left;
  }
}
</style>
