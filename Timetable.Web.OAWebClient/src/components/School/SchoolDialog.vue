<template>
  <ElDialog :title="dialogTitle" :lock-scroll="false" :visible.sync="dialogVisible" width="600px" close :close-on-click-modal="false">
    <div>
      <ElForm ref="form" label-width="100px" @submit.native.prevent :model="editData" :rules="rules">
        <ElFormItem label="省份" prop="Province">
          <ElSelect style="width:100%;" v-model="editData.Province" filterable allow-create default-first-option placeholder="可选择或填写省份">
            <ElOption v-for="item in provinces" :key="item" :label="item" :value="item"> </ElOption>
          </ElSelect>
        </ElFormItem>
        <ElFormItem label="学校名" prop="Name">
          <ElInput v-model="editData.Name" placeholder="可填写学校名"></ElInput>
        </ElFormItem>
        <ElFormItem  label="排序号 " prop="SortNum">
          <ElInputNumber class="text-left" style="width:100%;text-align:left;" :controls="false" :min="0" :max="9999" v-model="editData.SortNum" placeholder="排序号"></ElInputNumber>
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
    provincePermission() {
      return this.hasPermission("SC_GP");
    },
  },
  data() {
    return {
      editData: {},
      roles: [],
      provinces: [],
      dialogTitle: "修改",
      dialogVisible: false,
      rules: {
        Province: [
          { required: true, message: "省份必选，如选项中不存在，请自行填写" },
          { min: 1, max: 16, message: "长度最大16字符" },
        ],
        Name: [
          { required: true, message: "学校名必填" },
          {
            validator: (rule, value, callback) => {
              var url = "/school/checkschoolname";
              this.$axios.post(this.$baseURL + url, "name=" + encodeURIComponent(value) + "&id=" + this.editData.Id).then((res) => {
                if (res.data) {
                  callback();
                } else {
                  callback(new Error("学校名已存在"));
                }
              });
            },
          },
          { min: 1, max: 24, message: "学校名长度最大24字符" },
        ],

        SortNum: [{ required: true, message: "排序号必填" }],
      },
    };
  },

  methods: {
    refreshProvinces() {
      if (this.provincePermission) {
        this.$axios.post(this.$baseURL + "/school/provincelist").then((res) => {
          this.provinces = res.data.data;
        });
      } else {
        this.provinces = [];
      }
    },
    showAddDialog: function() {
      this.refreshProvinces();
      this.dialogTitle = "添加学校信息";
      this.editData = {
        Id: 0,
        Name: "",
        Province: "",
        SortNum: 100,
      };
      this.dialogVisible = true;
      this.$nextTick(() => {
        this.$refs["form"].clearValidate();
      });
    },
    showEditDialog: function(row) {
      this.refreshProvinces();
      this.dialogTitle = "修改学校信息";
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
            url = "/school/add";
          } else {
            url = "/school/edit";
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
.text-left{
   .el-input__inner {
       text-align: left;
   }
}
</style>