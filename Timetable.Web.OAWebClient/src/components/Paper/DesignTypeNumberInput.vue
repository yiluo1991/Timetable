<template>
  <div>
    <!-- 题目 -->

    <div style="margin-top:10px;" class="noevnet">
      <ElFormItem ref="ss" label="" :prop="`${question.id}`" :rules="testRule">
        <div class="noevent clearfix ti">
          <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
          <div class="qt fl">
            <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
          </div>
        </div>
        <div style="padding:14px 3px;">
          <ElInput v-if="!question.useSlider" v-model="answers[question.id]" resize="none" @blur="parseValue" maxlength="9" clearable>
            <template #append v-if="question.suffix">
              <span style="">
                {{ question.suffix }}
              </span>
            </template>
          </ElInput>
          <ElSlider
            v-if="question.useSlider"
            :format-tooltip="(v) => v + question.suffix"
            :min="parseFloat(question.min || 0)"
            :step="[1, 0.1, 0.01, 0.001][question.precision]"
            :max="parseFloat(question.max == '' || question.max == undefined ? 100 : question.max)"
            v-model="answers[question.id]"
            show-input
            :show-input-controls="false"
          ></ElSlider></div
      ></ElFormItem>
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
    return {};
  },
  watch: {
    "question.precision": function() {
      this.parseValue();
      this.parse("min");
      this.parse("max");
    },
  },
  computed: {
    testRule() {
      var rule = [];
      if (this.question.required) {
        rule.push({ required: true, message: "必填" });
      }
      var min = this.question.min || -99999999;
      var max = this.question.max || 999999999;
      rule.push({
        validator: (rule, v, callback) => {
          if (v === undefined || v === "" || v === null) {
            callback();
          } else if (typeof v == "number") {
            if (v < min) {
              callback("不可低于最小值" + min);
            } else if (v > max) {
              callback("不可大于最大值" + max);
            } else {
              callback();
            }
          } else {
            if (/^-?([1-9]\d*\.\d*)$|^-?(0\.\d*)$|^(-?[1-9]\d*|0)$/.test(v)) {
              var pv = parseFloat(v);
              if (pv < min) {
                callback("不可低于最小值" + min);
              } else if (pv > max) {
                callback("不可大于最大值" + max);
              } else {
                callback();
              }
            } else {
              callback("请输入正确的数字格式");
            }
          }
        },
      });
      return rule;
    },
  },
  methods: {
    parseValue() {
      var tv = this.answers[this.question.id];
      if (tv === "") {
        this.answers[this.question.id] = null;
      } else {
        var n = parseFloat(tv);
        if (isNaN(n)) {
          this.answers[this.question.id] = null;
        } else {
          var min = this.question.min || -99999999;
          var max = this.question.max || 999999999;
          if (n > max) n = max;
          if (n < min) n = min;
          n = parseFloat(n.toFixed(this.question.precision));
          this.answers[this.question.id] = n;
        }
      }
    },
  },
};
</script>
<style lang="less"></style>
