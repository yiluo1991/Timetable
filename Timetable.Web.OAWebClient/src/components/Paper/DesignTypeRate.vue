<template>
  <div>
    <!-- 选项展示 -->

    <ElFormItem :prop="`${question.id}`" :rules="rule">
      <div class="noevent clearfix ti">
        <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
        <div class="qt fl">
          <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
        </div>
      </div>
      <div style="padding:5px">
        <DesignTypeRateOptions v-model="answers[question.id]"  :lang="lang" :question="question"></DesignTypeRateOptions>
      </div>
    </ElFormItem>
  </div>
</template>
<script>
import DesignTypeRateOptions from "./DesignTypeRateOptions";
export default {
  components: {
    DesignTypeRateOptions,
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
          validator: (field, value, callback) => {
            if (value === undefined) {
              callback();
            } else {
              var total = 0;
              var limit = this.question.total;

              for (var i = 0; i < this.question.items.length; i++) {
                if(value[this.question.items[i].fid]!==undefined){

                     total += value[this.question.items[i].fid];
                }else{
                  this.$set(value,this.question.items[i].fid,0);
                }
              }
              if (total == 0) {
                if (this.question.required) {
                  callback("当前已分配" + total + "，应分配总比重为" + limit + "。");
                } else {
                  callback();
                }
              } else {
                if (limit != total) {
                  callback("当前已分配" + total + "，应分配总比重为" + limit + "。");
                } else {
                  callback();
                }
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
