<template>
  <!-- 矩阵单选 -->
  <div>
 

    <ElFormItem :prop="`${question.id}`" :rules="rule">
        <div class=" clearfix ti">
      <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
      <div class="qt fl">
        <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
      </div>
    </div>
      <DesignTypeMatrixSingleOptions v-model="answers[question.id]"  :lang="lang" :question="question"></DesignTypeMatrixSingleOptions>
    </ElFormItem>
  </div>
</template>
<script>
import DesignTypeMatrixSingleOptions from "./DesignTypeMatrixSingleOptions";
export default {
  components: {
    DesignTypeMatrixSingleOptions,
  },
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
    numObj: Object,
    answers: Object,
  },
  computed: {
    rule() {
      return [
        {
          validator: (rule, v, callback) => {
            if (this.question.required) {
              var hasErr;
              for (var item of this.question.items) {
                if (this.answers[this.question.id]===undefined||(this.answers[this.question.id][item.fid] === undefined)) {
                  callback((this.lang == "CN") ? `该题每个行都要选择答案` : `Each line needs an answer.`);
                  hasErr = true;
                  break;
                }
              }
              if (!hasErr) {
                callback();
              }
            } else {
              callback();
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
