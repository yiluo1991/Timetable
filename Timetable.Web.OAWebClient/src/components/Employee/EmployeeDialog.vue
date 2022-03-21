<template>
  <ElDialog :title="dialogTitle" :lock-scroll="false" :visible.sync="dialogVisible" width="600px" close :close-on-click-modal="false">
    <div>
      <ElForm ref="form" label-width="100px" @submit.native.prevent :model="editData" :rules="rules">
        <ElFormItem label="登录名" prop="LoginName">
          <ElInput v-model="editData.LoginName" placeholder="（必填）请填写登录名" :disabled="disabled"></ElInput>
        </ElFormItem>
        <ElFormItem label="真实姓名" prop="RealName">
          <ElInput v-model="editData.RealName" placeholder="（必填）请填写真实姓名"></ElInput>
        </ElFormItem>
        <ElFormItem label="邮箱" prop="Email">
          <ElInput v-model="editData.Email" placeholder="可填写邮箱"></ElInput>
        </ElFormItem>
        <ElFormItem label="联系电话" prop="Mobile">
          <ElInput v-model="editData.Mobile" placeholder="可填写手机号"></ElInput>
        </ElFormItem>
        <ElFormItem label="启用" prop="Enable">
          <el-select v-model="editData.Enable" style="width:100%;" filterable placeholder="请选择是否启用">
            <el-option label="启用" :value="true"></el-option>
            <el-option label="禁用" :value="false"></el-option>
          </el-select>
        </ElFormItem>
        <ElFormItem label="联系地址" prop="Address">
          <ElInput v-model="editData.Address" placeholder="可填写联系地址"></ElInput>
        </ElFormItem>
        <ElFormItem label="角色" prop="RoleIds">
          <el-select v-model="editData.RoleIds" style="width:100%;" placeholder="请选择" multiple>
            <el-option v-for="item in roles" :key="item.Id" :label="item.Name" :value="item.Id"> </el-option>
          </el-select>
        </ElFormItem>
         <ElFormItem label="自动停用" prop="PermissionEndTime">
           
           <ElDatePicker style="width:100%" v-model="editData.PermissionEndTime"  type="datetime" value-format="yyyy-MM-dd HH:mm:ss" placeholder="（可选）可选择自动停用该账户的时间"></ElDatePicker>
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
      editData: {},
      roles: [],
      disabled: false,
      dialogTitle: "修改",
      dialogVisible: false,
      rules: {
        RoleIds:[{required:true,message:'角色必选'}],
        LoginName: [
          { required: true, message: "登录名必填" },
          { min: 5, max: 32, message: "用户名长度为5~32字符" },
          {
            validator: (rule, value, callback) => {
              if (this.disabled) {
                callback();
              } else {
                console.log(this)
                if (this.hasVerifyPermission) {
                  var url = "/employee/checkLoginName";
                  this.$axios.post(this.$baseURL + url, "loginname=" + encodeURIComponent(value)).then((res) => {
                    if (res.data) {
                      callback();
                    } else {
                      callback(new Error("用户名已存在"));
                    }
                  });
                } else {
                  callback(new Error("您无权使用用户名验证接口"));
                }
              }
            },
          },
        ],
        RealName: [
          { required: true, message: "必须填写用户姓名" },
          { min: 1, max: 128, message: "姓名长度为1~128字符" },
        ],
        Email: [
          { type: "email", message: "邮箱格式有误" },
          { min: 1, max: 128, message: "邮箱长度在1~128位" },
        ],
        Mobile: [{ pattern: /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/, message: "联系电话格式有误，支持国内固定电话和手机" }],
        Enable: [{ required: true, message: "必须选择" }],
        SN: [
          { required: true, message: "排序号必填" },
          { min: 0, max: 9999, type: "number", message: "排序号必须是0-9999间的整数" },
        ],
        Address: [{ min: 1, max: 128, message: "地址长度为1~128字符" }],
      },
    };
  },
  inject:['hasPermission'],
computed: {
      hasEditPermission() {
        return this.hasPermission("E_M");
      },
      hasAddPermission() {
        return this.hasPermission("E_A");
      },
      hasGetPermission() {
        return this.hasPermission("E_RG");
      },
      hasVerifyPermission() {
        return this.hasPermission("E_V");
      },
    },
  methods: {
   
    
    reload(type, row) { 
      if(!this.hasGetPermission)return;
      this.$axios
        .post(this.$baseURL + "/role/list", {
          extend: "s",
        })
        .then((res) => {
          
          this.roles = res.data.data;
          if (type == 1) {
            this.showAddDialog();
          }
          if (type == 2) {
            this.showEditDialog(row);
          }
        });
    },
    showAddDialog: function() {
      if(!this.hasAddPermission)return;
      this.dialogTitle = "添加管理账号";
      this.editData = {
        LoginName: "",
        RealName: "",
        Email: "",
        Mobile: "",
        Enable: true,
        Address: "",
        Id: 0,
        RoleIds: [],
      };

      this.disabled = false;
      this.dialogVisible = true;
      this.$nextTick(() => {
        this.$refs["form"].clearValidate();
      });
    },
    showEditDialog: function(row) {
      if(!this.hasEditPermission)return;
     
      this.dialogTitle = "修改管理账号";
      var e = row;

      this.disabled = true;
      e.RoleIds = e.EmployeeRoleIds;
      this.dialogVisible = true;
      this.editData = e;
      this.$nextTick(() => {
        this.$refs["form"].clearValidate();
      }, 1000);
    },
    save: function() {
      this.$refs["form"].validate((result) => {
        if (result) {
          var url;
          if (this.editData.Id == "0") {
            if(!this.hasAddPermission)return;
            url = "/employee/add";
          } else {
            if(!this.hasEditPermission)return;
            url = "/employee/edit";
          }
          var loading = this.$loading({ text: "正在处理请求" });
          this.$axios
            .post(this.$baseURL + url, this.editData, () => {})
            .then((res) => {
              this.dialogVisible = false;
              if (res.data.success) {
                if (this.editData.Id == "0") {
                  this.$alert(res.data.msg, "添加成功", {
                    confirmButtonText: "确定",
                  });
                }
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
