<template>
  <div :class="{ noevent: noevent }" @click.stop>
    <ElCheckboxGroup  :max="question.max?question.max:undefined" v-model="innerValue">
      <table class="optable">
        <colgroup>
          <col v-for="i in repeat" :key="i" />
        </colgroup>
        <tr v-for="row in rowOptions" :key="row.id">
          <td v-for="o in row" :key="o.oid">
            <ElCheckbox :style="{ width: border ? '100%' : 'auto' }" :border="border" :label="o.value">
              <span class="img" v-if="o.pic && o.pic.src">
                <img :src="o.pic.src" alt="" :style="o.pic.autosize ? {} : { width: o.pic.width ? o.pic.width + 'px' : 'auto', height: o.pic.height ? o.pic.height + 'px' : 'auto' }" />
              </span>
              <span class="text">{{ o["text_" + lang] }}<span class="score" v-if="question.type=='ScoreMulti'">
                <!-- （分值：{{o.score }}） -->
                </span></span>
            </ElCheckbox>
          </td>
        </tr>
      </table>
    </ElCheckboxGroup>
  </div>
</template>
<script>
export default {
  props: {
    lang: String,
    noevent: {
      type: Boolean,
      default: false,
    },
    question: Object,
    value: {
      default: function() {
        return [];
      },
    },
  },
  watch: {
    value(value) {
      this.innerValue = value;
    },
    innerValue() {
      this.$emit("input", this.innerValue);
      //经测试发现会自动触发外层表单项验证，需注意复检
      //   if (this.$parent && this.$parent.form) {
      //     this.$parent.validate();
      //   }
    },
  },
  data() {
    return {
      innerValue: this.value,
    };
  },
  computed: {
    border() {
      return this.question.options.filter((s) => s.pic && s.pic.src).length > 0;
    },
    repeat() {
      var arr = [];
      for (var i = 0; i < this.question.col; i++) {
        arr.push(i);
      }
      return arr;
    },
    rowOptions: function() {
      var col = this.question.col;
      var options = this.question.options;
      var rowOptions = [];
      for (var i in options) {
        if (i % col == 0) {
          var newRow = [];
          newRow.id = i;
          rowOptions.push(newRow);
        }
        rowOptions[rowOptions.length - 1].push(options[i]);
      }
      return rowOptions;
    },
  },
};
</script>
<style lang="less">

</style>
