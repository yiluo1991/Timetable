<template>
  <ElDialog :visible.sync="dialogVisible" width="1200px" height="1000px" top="5vh" title="问卷审核信息" :close-on-click-modal="false">
    <div style="overflow: hidden;" v-if="paper">
      <div style="float:left;width:880px;height:75vh">
        <ViewPaper :data="paper" class="desktop"></ViewPaper>
      </div>
      <div style="float:left;width:270px;height:75vh;margin-left:10px">
        <ElForm ref="v" :model="paper">
          <ElFormItem label="当前状态：">
            <ElInput value="待审核" readonly></ElInput>
          </ElFormItem>
          <ElFormItem label="上次审核人：">
            <ElInput :value="paper.auditEmployee ? paper.auditEmployee : '- 无 -'" readonly></ElInput>
          </ElFormItem>
          <ElFormItem label="审核反馈信息：" prop="auditFeedback" :rules="[{ max: 1024, message: '反馈信息最长1024个字符' }]">
            <ElInput :readonly="!auditPermission" type="textarea" resize="none" rows="10" v-model="paper.auditFeedback" placeholder="请输入审核信息" maxlength="1024" show-word-limit></ElInput>
          </ElFormItem>
        </ElForm>
        <div style="text-align:center;" v-if="auditPermission">
          <ElButton type="success" @click="audit(true)" round plain icon="el-icon-check">通过</ElButton>
          <ElButton type="danger" @click="audit(false)" round plain icon="el-icon-close">拒绝</ElButton>
        </div>
      </div>
    </div>
  </ElDialog>
</template>
<script>
import ViewPaper from "../Paper/ViewPaper";
export default {
  components: {
    ViewPaper,
  },
  inject: ["hasPermission"],
  computed: {
    auditPermission() {
      return this.hasPermission("AS_A");
    },
  },
  data() {
    return {
      paper: {},
      dialogVisible: false,
    };
  },
  methods: {
    showDialog(data) {
      this.paper = data;
      this.dialogVisible = true;
    },
    audit(result) {
      if (!this.auditPermission) return;
      this.$axios
        .post(
          this.$baseURL + "/auditsurvey/audit",
          `id=${encodeURIComponent(this.paper.id)}&result=${encodeURIComponent(result)}&feedback=${this.paper.auditFeedback ? encodeURIComponent(this.paper.auditFeedback) : ""}`
        )
        .then((res) => {
          this.dialogVisible = false;
          this.$emit("reload");
          if (!res.data.success) {
          
            this.$message({
              message: res.data.msg,
              type: "error",
              offset: 80,
            });
          }
        });
    },
  },
};
</script>
