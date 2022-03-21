<template>
  <div>
    <!-- 题目 -->
 
    <!-- 选项展示 -->
    <ElFormItem label="" :prop="`${question.id}`" :rules="rules">
         <div class=" clearfix ti">
      <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
      <div class="qt fl">
        <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
      </div>
    </div>
      <DesignTypeInlineSingleOptions  icon="el-icon-star-on" void-icon-class="el-icon-star-off" :lang="lang" v-model="answers[question.id]" :question="question"></DesignTypeInlineSingleOptions>
    </ElFormItem>
  </div>
</template>
<script>
import DesignTypeInlineSingleOptions from "../Paper/DesignTypeInlineSingleOptions";
export default {
  components: {
    DesignTypeInlineSingleOptions,
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
  },
  methods: {},
};
</script>
<style lang="less"></style>
