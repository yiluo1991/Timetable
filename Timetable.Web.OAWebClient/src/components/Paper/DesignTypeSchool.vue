<template>
  <div>
    <!-- 题目 -->

    <div style="margin-top:10px;" class="noevnet">
      <ElFormItem ref="ss" label="" :prop="`${question.id}`" :rules="testRule">
        <div class="noevent clearfix ti" style="">
          <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
          <div class="qt fl">
            <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
          </div>
        </div>
        <div style="padding:14px 3px;">
          <ElInput readonly  @click.native="showDialog()" :placeholder="lang == 'CN' ? '-  点击选择学校  -' : '-  Please select  -'" v-model="answers[question.id]" resize="none" clearable>
            <ElButton slot="append" @click.stop="showDialog()" icon="el-icon-office-building">选择学校</ElButton>
          </ElInput>
        </div></ElFormItem>
    </div>
    <DesignTypeSchoolDialog v-if="initDialog" @destory="initDialog=false"  ref="dialog" v-model="answers[question.id]"></DesignTypeSchoolDialog>
  </div>
</template>
<script>
import DesignTypeSchoolDialog from './DesignTypeSchoolDialog'
export default {
  components: {
    DesignTypeSchoolDialog
  },
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
    numObj: Object,
    answers: Object,

  },
  data() {
    return { 
    initDialog:false
    };
  },
  watch: {},
  computed: {
    testRule() {
      var rule = [];
      if (this.question.required) {
        rule.push({ required: true, message: "必填" });
      }
      return rule;
    },
  },
  methods: {
    showDialog() {
   
      this.initDialog=true;
      this.$nextTick(()=>{ 
        this.$refs.dialog.showDialog();
      })
     
    },
  },
};
</script>
<style lang="less">
.minipadding{
    width:90vw;
    max-width: 500px;
    
.el-dialog__body{ 
  padding:00px 10px 10px 10px;
}

}</style>
