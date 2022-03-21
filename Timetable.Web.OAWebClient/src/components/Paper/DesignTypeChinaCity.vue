<template>
  <div>
    <!-- 题目 -->
    <ElFormItem label="" :prop='`${question.id}`' :rules="rules">
      <div class=" clearfix ti">
        <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
        <div class="qt fl">
          <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
        </div>
      </div>
      <div  style="padding:14px 3px;">
          <ElChinaCity 
        :model="answers"
        :provinceField="question.items[0].fid"
        :cityField="question.items[1].fid"
        :questionId="question.id"
        :placeholder="lang == 'CN' ? '-  请选择  -' : '-  Please select  -'"
        style="width:100%"
      ></ElChinaCity>
      </div>
    
    </ElFormItem>
  </div>
</template>
<script>
import ElChinaCity from '../Common/ElementUI/ElChinaCity'
export default {
  components: {ElChinaCity},
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
    numObj: Object,
    limitMode: Boolean,
    answers: Object,
  },
  computed :{
  
      rules() {
        
        return [{ required: this.question.required, message: this.lang == "CN" ? "该题必答" : "This answer is required" }];
      },
    
  },
  methods: {
    //表单验证
    validate(callback) {
      this.$refs.form.validate((res) => {
        callback(res);
      });
    },
  },
};
</script>
<style lang="less"></style>
