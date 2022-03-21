<template>
  <div>
    <!-- 选项展示 -->
    <ElFormItem ref="ss" label="" :prop="`${question.id}`" :rules="rules">
      <div class=" clearfix ti">
        <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
        <div class="qt fl">
          <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
        </div>
      </div>
      <DesignTypeSingleOptions  :lang="lang" v-model="answers[question.id]" :question="question"></DesignTypeSingleOptions>
    </ElFormItem>
  </div>
</template>
<script>
import DesignTypeSingleOptions from "./DesignTypeSingleOptions";
export default {
  components: {
    DesignTypeSingleOptions,
  },
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
    numObj: Object,
    answers: Object,
  },
  mounted() {},
  computed: {
    rules() {
      return [{ required: this.question.required, message: this.lang == "CN" ? "该题必答" : "This answer is required" }];
    },
  },
  methods: {},
};
</script>
<style lang="less"></style>
