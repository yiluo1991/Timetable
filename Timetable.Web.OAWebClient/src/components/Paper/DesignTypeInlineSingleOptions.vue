<template>
  <div :class="{ noevent: noevent }" @click.stop>
    <div class="ratepadding">
      <b>{{ texts[0] }}</b>
      <ElRate
        style="display:inline-block"
        v-model="innerValue"
        :low-threshold="level1"
        :high-threshold="level2"
        :colors="colors"
        show-text
        :texts="texts"
        :void-icon-class="voidIconClass"
        :icon-classes="icons"
        :max="question.options.length"
      ></ElRate>
      <b>{{ texts[texts.length - 1] }}</b>
    </div>
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
    voidIconClass: {},
    question: Object,
    value: {
      default: undefined,
    },
    icon: {
      required: true,
    },
    colors: {},
  },

  watch: {
    value(value) {
      var index = this.question.options.findIndex((s) => {
        return s.value == value;
      });

      if (index == -1) {
        this.innerValue = undefined;
      } else {
        this.innerValue = index + 1;
      }
    },
    selectValue: function(value) {
      if (value === undefined) {
        this.innerValue = undefined;
      }
      this.$emit("input", value);
      if (this.$parent && this.$parent.form) {
        this.$parent.validate();
      }
    },
  },
  computed: {
    selectValue() {
      if (this.innerValue === undefined) {
        return undefined;
      } else {
        if (this.innerValue > 0) {
          return this.question.options[this.innerValue - 1].value;
        } else {
          return undefined;
        }
      }
    },

    texts() {
      return this.question.options.map((s) => s["text_" + this.lang]);
    },
    icons() {
      return [this.icon, this.icon, this.icon];
    },
    level1() {
      return Math.floor(this.question.options.length / 3);
    },
    level2() {
      return Math.ceil(((this.question.options.length + 1) / 3) * 2);
    },
  },
  data() {
    return {
      innerValue: undefined,
    };
  },
  mounted() {
    this.innerValue = this._mountInnerValue();
  },
  methods: {
    _mountInnerValue() {
      var index = this.question.options.findIndex((s) => {
        return s.value == this.value;
      });
      if (index == -1) {
        return undefined;
      } else {
        return index + 1;
      }
    },
  },
};
</script>
<style lang="less">
.ratepadding {
  padding-top: 10px;
  padding-bottom: 20px;
  .el-rate {
    position: relative;
    .el-rate__text {
      position: absolute;
      top: 30px;
      left: 50%;
      transform: translateX(-50%);
    }
  }
  b{
   vertical-align: middle;
   font-size: 14px;
   &:first-child{
       margin-right:10px;
   }   
   &:last-child{
       margin-left:10px;
   }
  }
}
</style>
