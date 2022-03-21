<template>
  <div>
 
    <ElFormItem ref="ss" label="" :prop="`${question.id}`" :rules="rules">
       <div class=" clearfix ti">
      <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
      <div class="qt fl">
        <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
      </div>
    </div>
      <DesignTypeMultiOptions v-model="answers[question.id]" :lang="lang" :question="question"></DesignTypeMultiOptions>
    </ElFormItem>
  </div>
</template>
<script>
import DesignTypeMultiOptions from "./DesignTypeMultiOptions";
export default {
  components: {
    DesignTypeMultiOptions,
  },
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
    numObj: Object,
    answers: Object,
  },
  computed: {
    rules() {
      return [
        { required: this.question.required, message: this.lang == "CN" ? "该题必答" : "This answer is required" },
        {
          trigger: "change",
          validator: (rule, v, callback) => { 
            if(v==null){
              callback()
            }else{
 if (this.question.max&&this.question.max<v.length) { 
               callback((this.lang == "CN") ? `最多选中${this.question.max}项` : `You can select up to ${this.question.max}`);
            } else{
               callback();
            }
            }
           
             
          },
        },
      ];
    },
  },
  methods: {},
};
</script>
<style lang="less"></style>
