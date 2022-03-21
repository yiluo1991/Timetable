<template>
  <div>
    <!-- 题目 -->
   
    <div style="margin-top:00px;" class="">
      <ElFormItem ref="ss" label="" :prop="`${question.id}`" :rules="testRule">
         <div class=" clearfix ti">
      <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
      <div class="qt fl">
        <pre style="font-family:none"  >{{question['text_' + lang]}}</pre>
      </div>
    </div>
             <div style="padding:14px 3px;">
        <ElInput
          v-model="answers[question.id]"
          resize="none"
          :maxlength="question.max ? question.max : 4096"
          clearable
          :show-word-limit="question.max || question.max === null ? true : false"
          :autosize="{ minRows: 3, maxRows: 10 }"
          :type="question.textarea ? 'textarea' : 'text'"
        ></ElInput>
             </div>
      </ElFormItem>
    </div>
  </div>
</template>
<script>
export default {
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
    numObj: Object,
    answers: Object,
  },
  data() {
    return {
      exRules: {
        email: { regex: /^[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?$/, text_CN: "邮箱", text_EN: "Email" },
        cn: { regex: /^[\u4e00-\u9fa5]+$/,  message_CN: "只能包含中文字符" ,message_EN:"It can only contain Chinese characters"},
        en: { regex: /^[A-Za-z]+$/,  message_CN: "只能包含英文字符" ,message_EN:"It can only contain English characters"},
        cnen: { regex: /^[\u4e00-\u9fa5A-Za-z]+$/,message_CN: "只能包含中英文字符",message_EN:"It can only contain Chinese and English characters" },
        cnennum: { regex: /^[\u4e00-\u9fa5A-Za-z0-9\-.]+$/,message_CN: "只能包含中英文字符和数字",message_EN:"It can only contain Chinese and English characters and numbers" },
        ennum: { regex: /^[A-Za-z0-9\-.]+$/, message_CN: "只能包含英文字符和数字",message_EN:"It can only contain English characters and numbers" },
        cnennumspace: { regex: /^[\u4e00-\u9fa5A-Za-z0-9\-.\s]+$/,  message_CN: "只能包含中英文字符、数字和空格",message_EN:"It can only contain Chinese and English characters, numbers and spaces" },
        date: {
          regex: /^(?:(?!0000)[0-9]{4}([-/.]?)(?:(?:0?[1-9]|1[0-2])\1(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])\1(?:29|30)|(?:0?[13578]|1[02])\1(?:31))|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-]?)0?2\2(?:29))$/,
          message_CN: "请输入正确的日期格式，例如 2020-1-1",
          message_EN: "Please enter the correct date format, for example, 2020-1-1",
        },
        mobile: { regex: /^1\d{10}$/, text_CN: "手机号",text_EN:"Chinese MobilePhone Number" },
        phone: { regex: /(^(0[0-9]{2,3}-)?([0-9]{7,8})+(-[0-9]{1,4})?$)|(^1\d{10}$)/,  message_CN: "请输入正确的电话格式,支持手机和座机，例如13959999999，0591-8888888",message_EN:"Please input the correct Chinese mobile phone and landline phone format, for example, 13959999999, or 0591-8888888" },
        tel: { regex: /^(0[0-9]{2,3}-)?([0-9]{7,8})+(-[0-9]{1,4})?$/,  message_CN: "请输入正确的座机号码，例如0591-8888888，或分机格式0591-8888888-011",message_EN:"Please enter the correct landline number, for example, 0591-8888888, or 0591-8888888-011" },
        idcard: { regex: /^[1-9]\d{5}(18|19|20|(3\d))\d{2}((0[1-9])|(1[0-2]))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/, text_CN: "身份证号" ,text_EN:"Chinese ID card Number"},
        qq: { regex: /^[1-9][0-9]{4,12}$/, text_CN: "QQ",text_EN:"QQ" },
        url: {
          regex: /^(http|https):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)?((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.[a-zA-Z]{2,4})(:[0-9]+)?(\/[^/][a-zA-Z0-9.,?'\\/+&%$#=~_\-@]*)*$/,
          message_CN: "请输入正确的网址，网址以http:// 或 https://开头",
          message_EN:"Please enter the correct web address, which starts with http:// or HTTPS://"
        },
      },
    };
  },
  computed: {
    testRule() {
      var rule = [];
      if (this.question.required) {
        rule.push({ required: true, message:(this.lang=="CN"?"该题必答" :"This answer is required") });
      }
      if (this.question.max || this.question.max === null) {
        rule.push({
          type: "string",
          max: this.question.max || 4096,
          message: (this.lang=="CN"?"最大长度" :"Maximum length：")+ (this.question.max || 4096),
        });
      }
      if (this.question.min) {
        rule.push({
          type: "string",
          min: this.question.min,
          message: (this.lang=="CN"?"最小长度" :"Minimum length：")+ + this.question.min,
        });
      }
      if (this.question.rule) {
        rule.push({
          pattern: this.exRules[this.question.rule].regex,
          message: this.exRules[this.question.rule]["message_"+this.lang] ? this.exRules[this.question.rule]["message_"+this.lang] : (this.lang=="CN"?"请输入正确的":"Please enter the correct ") + this.exRules[this.question.rule]["text_"+this.lang],
        }); 
      }
      return rule;
    },
  },
  methods: {},
};
</script>
<style lang="less"></style>
