<template>
  <ElDialog
    top="60px"
    :title="dialogTitle"
    :lock-scroll="true"
    :visible.sync="dialogVisible"
    width="600px"
    :close-on-click-modal="false"
  >
    <div>
      <ElDescriptions direction="vertical" :column="3" border>
        <ElDescriptionsItem label="手机号">
          <span style="color:#FF4949;"> {{ infoData.Mobile }}</span>
        </ElDescriptionsItem>
        <ElDescriptionsItem label="姓名">
           <span style="color:#FF4949;"> {{ infoData.RealName }}</span>
        </ElDescriptionsItem>
         <ElDescriptionsItem label="注册用户组" >
             <span style="color:#FF4949;">{{ infoData.UserGroupName }}</span>
        </ElDescriptionsItem>
        <ElDescriptionsItem label="性别">
          {{ infoData.Sex == 0 ? "男" : "女" }}
        </ElDescriptionsItem>
        <ElDescriptionsItem label="组织机构">
          {{ infoData.OrganizationName }}
        </ElDescriptionsItem>
        <ElDescriptionsItem label="学号/工号">
          {{ infoData.IdentityNumber }}
        </ElDescriptionsItem>
        <ElDescriptionsItem label="申请时间" :span="3">
          {{ infoData.CreateTime | datetime }}
        </ElDescriptionsItem>
          <ElDescriptionsItem label=" 审核状态">
           <el-tag v-if="infoData.UserRegisterState==0" type="info">未审核</el-tag>
           <el-tag v-if="infoData.UserRegisterState==1" type="success">通过</el-tag>
           <el-tag v-if="infoData.UserRegisterState==2" type="danger">拒绝</el-tag>
        </ElDescriptionsItem>
          <ElDescriptionsItem label="审核人"  v-if="infoData.DealTime">
          {{ infoData.Employee}}
        </ElDescriptionsItem>
        <ElDescriptionsItem label="审核时间" v-if="infoData.DealTime" >
         <span >  {{ infoData.DealTime |datetime}}</span>
        </ElDescriptionsItem>
         <ElDescriptionsItem label="审核备注" v-if="infoData.Remark" >
           <pre style="white-space:pre-wrap;">{{infoData.Remark}}</pre>
        </ElDescriptionsItem>
      </ElDescriptions> 
      <ElForm ref="form" :model="editData" v-if="hasEditPermission&&infoData.UserRegisterState==0">
        <ElFormItem prop="Remark" label="审核备注">
          <ElInput v-model="editData.Remark" type="textarea" :rows="3"  resize="none" show-word-limit maxlength="64" placeholder="请输入审核备注"></ElInput>
        </ElFormItem>
      </ElForm>
    </div>
    <span slot="footer"  class="dialog-footer">
      <ElButton @click="dialogVisible = false" icon="el-icon-close"
        >关 闭</ElButton
      >
      <ElButton type="primary" v-if="hasEditPermission&&infoData.UserRegisterState==0" @click="save(true)" icon="el-icon-circle-check"
        >通过</ElButton
      >
      <ElButton type="danger" v-if="hasEditPermission&&infoData.UserRegisterState==0" @click="save(false)" icon="el-icon-circle-close"
        >拒绝</ElButton
      >
    </span>
  </ElDialog>
</template>
<script>
export default {
  components: {},
  
  inject: ["hasPermission"],
  computed: {
    hasEditPermission() {
      return this.hasPermission("UR_A");
    },
    hasGetPermission() {
      return this.hasPermission("UR_G");
    },
  },
  data() {
    return {
      initDialog: false,
      infoData: {},editData:{},
      dialogTitle: "修改",
      dialogVisible: false,
    };
  },

  methods: {
    showSCDialog() {
      this.initDialog = true;
      this.$nextTick(() => {
        this.$refs.scdialog.showDialog();
      });
    },

    showEditDialog: function(row) {
      if (!this.hasGetPermission) return;
      this.dialogTitle = "审核信息";
      this.dialogVisible = true;
      this.infoData = JSON.parse(JSON.stringify(row));
      this.$nextTick(() => {
        this.$refs.form.clearValidate();
      });
    },
    save: function(bool) {
          if(!this.hasEditPermission ||this.infoData.UserRegisterState!=0 )return;
          this.$confirm('此操作将【'+(bool?'通过':'拒接')+'】该注册申请，确定?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
           var loading = this.$loading({
            text: "数据处理中",
          }); 
          this.$axios.post(this.$baseURL + '/userregister/audit', {
            id:this.infoData.Id,
            isAllow:bool,
            Remark: this.editData.Remark
          }).then((res) => {
            this.dialogVisible = false; 
            if(!res.data.success){

            this.$message.error(res.data.msg)
            }
            setTimeout(() => {
              loading.close();
              this.$emit("reload");
            }, 0);
          });
        }).catch(() => {
                
        });
         
         
    },
  },
  mounted() {},
};
</script>
