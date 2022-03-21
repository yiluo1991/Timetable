<template>
  <ElDialog
    :title="dialogTitle"
    :lock-scroll="false"
    :visible.sync="dialogVisible"
    width="600px"
    close
    :close-on-click-modal="false"
  >
    <div>
      <ElForm ref="form" label-width="100px" @submit.native.prevent :model="editData" :rules="rules">
        <ElFormItem label="父权限页" prop="GroupId">
          <BasicSelectTree
            :width="435"
            ref="select"
            :data="treeData"
            node-key="Id"
            :default-props="{ label: 'DisplayName' }"
            :height="400"
            v-model="editData.GroupId"
            placeholder="请选择父权限页"
          ></BasicSelectTree>
        </ElFormItem>
        <ElFormItem label="权限项名称" prop="DisplayName">
          <ElInput v-model="editData.DisplayName" placeholder="（必填）请填写权限组名称"></ElInput>
        </ElFormItem>
        <ElFormItem label="权限路径" prop="Url">
          <ElInput v-model="editData.Url" placeholder="可填写权限路径"></ElInput>
        </ElFormItem>
        <ElFormItem label="标志名" prop="Key">
          <ElInput v-model="editData.Key" placeholder="可填写标志名"></ElInput>
        </ElFormItem>
        <ElFormItem label="排序号" prop="SN">
          <ElInput v-model.number="editData.SN" placeholder="（必填）请输入排序号，按升序排序"></ElInput>
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
     addPermission() {
      return this.hasPermission("PL_A");
    },
     editPermission() {
      return this.hasPermission("PL_M");
    },
    getPermission() {
      return this.hasPermission("PL_PGG");
    },
  },
  data() {
    return {
      dialogTitle: "修改",
      dialogVisible: false,
      editData: { ParentId: "", DisplayName: "" },
      treeData: [],
      rules: {
        GroupId: [
          { required: true, message: "必须选择权限项所在的父权限页" },
          { type: "number", message: "父权限页Id不正确" },
        ],
        DisplayName: [
          { required: true, message: "权限项名称必填" },
          { min: 1, max: 32, message: "权限项名称长度在1~32字符" },
        ],
        Url: [{ min: 1, max: 256, message: "权限路径长度在1~256字符" }],
        Key: [{ min: 1, max: 64, message: "标志名长度在1~64字符" }],
        SN: [
          { required: true, message: "排序号必填" },
          {
            min: 0,
            max: 9999,
            type: "number",
            message: "排序号必须是0-9999间的整数",
          },
        ],
      },
    };
  },

  methods: {
    showAddDialog: function() {
      this.dialogTitle = "添加"; 
      this.dialogVisible = true;
      this.$nextTick(() => {
        this.editData = {
          DisplayName: "",
          GroupId: undefined,
          Url: "",
          SN: 100,
          Id: 0,
          Key: "",
        };
        this.$refs.form.clearValidate();
      });
    },
    showEditDialog: function(row) {
      this.dialogTitle = "修改";
      if (row.GroupId == null) {
        row.GroupId = undefined;
      } else {
        row.GroupId = -row.GroupId;
      }
      this.dialogVisible = true;
      this.$nextTick(() => {
        this.editData = row;
        this.$refs.form.clearValidate();
      });
    },
    reload: function(type, row) {
      if(!this.getPermission){return;}
      var url = "/Permissiongroup/list";
      if (url) {
        this.$axios.post(this.$baseURL + url, {}).then((res) => {
          this.treeData = res.data.data;
          if (type == 1) {
            this.showAddDialog();
          }
          if (type == 2) {
            this.showEditDialog(row);
          }
        });
      }
    },
    save: function() {
      var that = this;
      this.$refs["form"].validate((result) => {
        if (result) {
          var url;
          if (this.editData.Id == "0") {
            if(!this.addPermission)return;
            url = "/PermissionLine/add";
          } else {
            if(!this.editPermission)return;
            url = "/PermissionLine/edit";
          }
          var loading = this.$loading({ text: "正在处理请求" });
          this.$axios
            .post(this.$baseURL + url, this.editData, () => {})
            .then((res) => {
              if (res.data.success) {
                that.$message({
                  type: "success",
                  message: res.data.msg,
                });
                that.dialogVisible = false;
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
