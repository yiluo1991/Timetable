<template>
  <ElDialog
    top="60px"
    title="注册设置"
    :lock-scroll="true"
    :visible.sync="dialogVisible"
    width="600px"
    :close-on-click-modal="false"
  >
    <div> 
      <ElForm label-width="100px">
        <ElFormItem label="注册开关：">
          <div style="width:100%;">
          <ElSwitch
            v-model="editData.AllowRegister"
            active-text="开放"
            inactive-text="关闭"
          >
          </ElSwitch>
          </div>
        </ElFormItem>
         <ElFormItem label="通过时短信：">
          <ElSwitch
            v-model="editData.SendSMSOnAllow"
            active-text="发送"
            inactive-text="不发送"
          >
          </ElSwitch>
        </ElFormItem>
         <ElFormItem label="拒绝时短信：">
          <ElSwitch
            v-model="editData.SendSMSOnRefuse"
            active-text="发送"
            inactive-text="不发送"
          >
          </ElSwitch>
        </ElFormItem>
      </ElForm>
    </div>
    <span slot="footer" class="dialog-footer">
      <ElButton @click="dialogVisible = false" icon="el-icon-close"
        >关 闭</ElButton
      >
      <ElButton
        type="primary"
        v-if="hasEditPermission"
        @click="save()"
        icon="el-icon-check"
        >保 存</ElButton
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
      return this.hasPermission("UR_MI");
    },
    hasGetPermission() {
      return this.hasPermission("UR_I");
    },
  },
  data() {
    return {
      editData: {},
      dialogVisible: false,
    };
  },

  methods: {
    show: function(row) {
      if (!this.hasGetPermission) return;
      this.$axios.post(this.$baseURL + "/userregister/info").then((res) => {
        if (res.data.success) {
          this.editData = res.data.data;
          this.dialogVisible = true;
          this.editData = res.data.data;
        }
      });
    },
    save: function() {
      if(!this.hasEditPermission)return;
      this.$axios.post(this.$baseURL + "/userregister/editinfo",this.editData).then((res) => {
        if (res.data.success) {
         this.dialogVisible=false;
        }
      });

    },
  },
  mounted() {},
};
</script>
