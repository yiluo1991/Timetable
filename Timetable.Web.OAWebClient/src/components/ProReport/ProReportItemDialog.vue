<template>
  <ElDialog :title="dialogTitle" :lock-scroll="false" :visible.sync="dialogVisible" width="600px" close :close-on-click-modal="false">
    <div>
      <ElForm ref="form" label-width="100px" @submit.native.prevent :model="editData" :rules="rules">
         <ElFormItem label="名称" prop="Name">
          <ElInput v-model="editData.Name" placeholder="填写模板名称"></ElInput>
        </ElFormItem>
      
        <ElFormItem  label="排序号 " prop="SortNum">
          <ElInputNumber class="text-left" style="width:100%;text-align:left;" :controls="false" :min="0" :max="9999" v-model="editData.SortNum" placeholder="排序号"></ElInputNumber>
        </ElFormItem>
        <ElFormItem label="可见状态" prop="Visiable">
          <ElSelect v-model="editData.Visiable"  >
            <ElOption  :value="true" label="可见"></ElOption>
             <ElOption :value="false" label="不可见"></ElOption>
          </ElSelect>
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
    editPermission() {
      return this.hasPermission("PR_M");
    },
  },
  data() {
    return {
      editData: {},
     
      dialogTitle: "修改报告模板信息",
      dialogVisible: false,
      rules: { 
        SortNum: [{ required: true, message: "排序号必填" }],
        Name:[{required:true,message:'名称必填'},{max:128,message:'最长128字符'}],
        Visiable:[{required:true,message:'请选择可见性'}]
      },
    };
  },

  methods: {
    
    
    showEditDialog: function(row) {
       
      this.dialogVisible = true;
      this.editData = JSON.parse(JSON.stringify(row)) ;
      this.$nextTick(() => {
        this.$refs["form"].clearValidate();
      });
    },
    save: function() {
      if(!this.editPermission)return;
      this.$refs["form"].validate((result) => {
        if (result) {
          var   url = "/proreport/edititem";
        
          var loading = this.$loading({ text: "正在处理请求" });
          this.$axios
            .post(this.$baseURL + url,`id=${this.editData.Id}&sortnum=${this.editData.SortNum}&name=${encodeURIComponent(this.editData.Name)}&visiable=${this.editData.Visiable}`)
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