<template>
  <div>
    <!-- 题目 -->
    <ElFormItem ref="formitem" label="" :prop="`${question.id}`" :rules="rules">
      <div class="noevent clearfix ti">
        <div class="qn fl"><span class="red" v-if="question.required">*</span>{{ numObj[question.id] }}.</div>
        <div class="qt fl">
          <pre style="font-family:none"   >{{question['text_' + lang]}}</pre>
        </div>
      </div>
      <!-- 选项展示 -->
      <div   class="sorttablebox" @click.stop style="margin-bottom:10px;">
        <ElSortboxGroup :max="question.max ? question.max : undefined" v-model="innerValue">
          <table class="optable ">
            <colgroup>
              <col />
            </colgroup>
            <tr v-for="row in question.items" :key="row.fid">
              <td style="border:1px solid #ccc;padding-left:10px;">
                <ElSortbox :label="row.fid">
                  <span class="text">{{ row.text_CN }} </span>
                </ElSortbox>
              </td>
            </tr>
          </table>
        </ElSortboxGroup>
      </div>
    </ElFormItem>
  </div>
</template>
<script>
import ElSortbox from "../Common/ElementUI/ElSortbox";
import ElSortboxGroup from "../Common/ElementUI/ElSortboxGroup";
export default {
  components: { ElSortboxGroup, ElSortbox },
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
    numObj: Object,
    answers: Object,
  },
  mounted() {
    if (this.outterValus) this.innerValue = this.outterValus;
  },
  watch: {
    outterValus() {
      if (this.outterValus) this.innerValue = this.outterValus;
      else{
        this.innerValue =[]
      }
    },
    innerValue(v) {
      if (v) {
        if (v.length > 0) {
          if (!this.answers[this.question.id]) {
            var d = {};
            v.forEach((s, i) => {
              d[s] = this.question.items.length - i;
            });
            this.$set(this.answers, this.question.id, d);
          } else {
            for (var i in this.answers[this.question.id]) {
              if (v.indexOf(i) == -1) {
                this.$delete(this.answers[this.question.id], i);
              }
            }
            v.forEach((s, i) => {
              this.$set(this.answers[this.question.id], s, this.question.items.length - i);
            });
          }
        } else {
          this.$delete(this.answers, this.question.id);
        }

        this.$refs.formitem.validate();
      }
    },
  },
  computed: {
    rules() {
      var rules = [{ required: true, message: this.lang == "CN" ? "该题必答" : "This answer is required" }];
      if (this.question.min) {
        rules.push({
          validator: (rule, v, callback) => {
            var count=0;
            for(var i in v){
              i,
              count++;
            }
            if (count > 0 && count< this.question.min) {
              callback(this.lang == "CN" ? `最少选择${this.question.min}项` : `Select at least  ${this.question.min} items.`);
            } else {
              callback();
            }
          },
        });
      }
      if(this.question.max){
         rules.push({
          validator: (rule, v, callback) => {
            var count=0;
            for(var i in v){
              i,
              count++;
            }
            if (count > 0 && count > this.question.max) {
              callback(this.lang == "CN" ? `最多选择${this.question.max}项` : `Select up to  ${this.question.max} items.`);
            } else {
              callback();
            }
          },
        });
      }
      console.log(rules)
      return rules;
    },
    outterValus() {
      if (this.answers[this.question.id]) {
        var list = [];
        for (var i in this.answers[this.question.id]) {
          if (i.startsWith(`${this.question.id}_`)) {
            list.push({ name: i, value: this.answers[i] });
          }
        }
        return list.sort((b, a) => a.value - b.value).map((s) => s.name);
      } else {
        return null;
      }
    },
  },
  data() {
    return {
      innerValue: [],
    };
  },
  methods: {},
};
</script>
<style lang="less">
.sorttablebox {
  margin: 10px 5px 5px 5px;

  border: 1px solid #ddd;
  padding: 0;
  border-radius: 5px;

  overflow: hidden;
  .optable {
    margin-top: -1px!important;
    width: 101%!important;
  }
  table {
    border-collapse: collapse;

    margin: -1px;
  }
}
</style>
