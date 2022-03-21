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
        <ElFormItem label="父权限页" prop="ParentId">
          <BasicSelectTree
            :width="435"
            ref="select"
            :data="treeData"
            node-key="Id"
            :default-props="{ label: 'DisplayName' }"
            :height="400"
            v-model="editData.ParentId"
            placeholder="请选择父权限组"
          ></BasicSelectTree>
        </ElFormItem>
        <ElFormItem label="权限页名称" prop="DisplayName">
          <ElInput v-model="editData.DisplayName" placeholder="（必填）请填写权限页名称"></ElInput>
        </ElFormItem>
        <ElFormItem label="权限页路径" prop="Url">
          <ElInput v-model="editData.Url" placeholder="可填写权限页路径"></ElInput>
        </ElFormItem>
        <ElFormItem label="标志名" prop="Key">
          <ElInput v-model="editData.Key" placeholder="可填写标志名"></ElInput>
        </ElFormItem>
        <ElFormItem label="小图标类名" prop="Headshot">
          <ElInput
            v-model="editData.Headshot"
            placeholder="可填写小图标类名，支持ElementUI和FontAwesome图标类名"
          ></ElInput>
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
  data() {
    return {
      dialogTitle: "修改",
      dialogVisible: false,
      editData: { DisplayName: "", ParentId: undefined },
      treeData: [],
      rules: {
        ParentId: [
          { required: true, message: "必填" },
          { type: "number", message: "父权限页Id不正确" },
        ],
        DisplayName: [
          { required: true, message: "权限页名称必填" },
          { min: 1, max: 32, message: "权限页名称长度在1~32个字符" },
        ],
        Url: [{ min: 1, max: 256, message: "权限页地址长度在1~256个字符" }],
        Key: [{ min: 1, max: 64, message: "权限页标志名非必填，如您填写，长度在1~64个字符" }],
        Headshot: [{ min: 1, max: 64, message: "小图标类名在1~64位" }],
        SN: [
          { required: true, message: "排序号必填" },
          { min: 0, max: 9999, type: "number", message: "排序号必须是0-9999间的整数" },
        ],
      },
    };
  },
  computed: {
     
    getPermission() {
      return this.hasPermission("PG_G");
      
    },
    editPermission(){
       return this.hasPermission("PG_M");
    },
    addPermission(){
       return this.hasPermission("PG_A");
    }
    
  },
  inject: ["hasPermission"],
  methods: {
    showAddDialog: function() {
      this.dialogTitle = "添加";

      this.dialogVisible = true;
      this.$nextTick(() => {
        this.editData = { DisplayName: "", ParentId: 0, Url: "", Headshot: "", SN: 100, Id: 0, Key: "" };
        this.$nextTick(() => {
          this.$refs.form.clearValidate();
        });
      });
    },
    showEditDialog: function(row) {
      console.log(row);
      console.log(this.treeData);
      this.dialogTitle = "修改";

      if (row.ParentId == null) {
        row.ParentId = 0;
      }
      this.dialogVisible = true;
      this.$nextTick(() => {
        this.$refs.select.$refs.tree.remove(row.Id);
        this.editData = row;
        this.$refs.form.clearValidate();
      });
      console.log(row);
    },
    reload: function(type, row) {
      //权限控制，没权限给一个空数据避免出错
      if (this.getPermission) {
        var url = "/permissiongroup/list";
        if (url) {
          this.$axios.post(this.$baseURL + url, {}).then((res) => {
            this.treeData = [{ Id: 0, DisplayName: "根节点", children: JSON.parse(JSON.stringify(res.data.data)) }];
            if (type == 1) {
              this.showAddDialog();
            }
            if (type == 2) {
              this.showEditDialog(row);
            }
          });
        }
      } else {
         this.treeData = [{ Id: 0, DisplayName: "根节点", children: [] }];
        if (type == 1) {
          this.showAddDialog();
        }
        if (type == 2) {
          this.showEditDialog(row);
        }
      }
    },
    save: function() {
      this.$refs["form"].validate((result) => {
        if (result) {
          var loading = this.$loading({ text: "正在处理请求" });
          var url;
          if (this.editData.Id == "0") {
            if(!this.addPermission)return;
            url = "/permissiongroup/add";
          } else {
             if(!this.editPermission)return;
            url = "/permissiongroup/edit";
          }
          this.$axios
            .post(this.$baseURL + url, this.editData, (data) => {})
            .then((res) => {
              loading.close();
              if (res.data.success) {
                this.$message({
                  type: "success",
                  message: res.data.msg,
                });
                this.dialogVisible = false;
              } else {
                this.$message({
                  type: "error",
                  message: res.data.msg,
                });
              }
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
