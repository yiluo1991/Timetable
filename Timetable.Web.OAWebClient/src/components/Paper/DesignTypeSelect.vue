<template>
  <div>
    <!-- 题目 -->
  
    <!-- 选项展示 -->
   
      <ElFormItem ref="ss" label="" :prop="`${question.id}`" :rules="rules">
          <div class="noevent clearfix ti">
      <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
      <div class="qt fl">
        <pre style="font-family:none"  >{{question['text_' + lang]}}</pre>
      </div>
    </div>
         <div style="padding:14px 3px;">
        <ElSelect clearable style="width:100%;" v-model="answers[question.id]" :placeholder="lang == 'CN' ? '-  请选择  -' : '-  Please select  -'">
          <ElOption v-for="option in question.options" :key="option.oid" :label="option['text_' + lang]" :value="option.value"></ElOption>
        </ElSelect>
        
    </div>
      </ElFormItem>
  </div>
</template>
<script>
export default {
  components: {},
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
    numObj: Object,
    answers: Object,
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
