<template>
  <div>
 
      <ElFormItem  label="" :prop="`${question.id}`" :rules="rules">
            <div class="noevent clearfix ti">
      <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
      <div class="qt fl">
        <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
      </div>
    </div>
        <DesignTypeInlineSingleOptions  :colors="['#8DcDFF', '#5DBDFF', '#409EFF']" icon="el-icon-success"  void-icon-class="el-icon-circle-check"  :lang="lang" v-model="answers[question.id]" :question="question" ></DesignTypeInlineSingleOptions>
      </ElFormItem>
  </div>
</template>
<script>
import DesignTypeInlineSingleOptions from '../Paper/DesignTypeInlineSingleOptions'
export default {
  components: {
    DesignTypeInlineSingleOptions
  }, 
  
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
    numObj: Object,
    answers:Object
  },
  computed: {
    rules() {
      return [{ required: this.question.required, message: this.lang == "CN" ? "该题必答" : "This answer is required" }];
    },
  }
};
</script>
<style lang="less">

</style>
