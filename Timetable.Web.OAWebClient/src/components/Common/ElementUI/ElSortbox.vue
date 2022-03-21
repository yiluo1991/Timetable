<template>
  <label class="el-sortbox" :class="{ 'is-bordered': border, seleced: checked, 'is-disabled': !checked && max <= innerValue.length }">
    <input type="checkbox" v-model="checked" hidden :disabled="!checked && max <= innerValue.length" />
    <span class="el-sortbox-checkbox">{{ index }} </span>
    <span class="el-sortbox-label"><slot></slot></span>
  </label>
</template>
<script>
export default {
  props: {
    label: {
      type: String,
    },
    value: {
      type: Array,
    },
    border: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      checked: false,
      innerValue: [],
      index: "",
      max: undefined,
    };
  },
  methods: {
    updateCheckedState(v) {
      this.innerValue = v;
      var index = this.innerValue.indexOf(this.label);
      if (index > -1) {
        this.index = index + 1;
        this.checked = true;
      } else {
        this.checked = false;
        this.index = "";
      }
    },
    updateinnerValueState() {
      var index = this.innerValue.indexOf(this.label);
      if (this.checked && index == -1) {
        this.innerValue.push(this.label);
        this.index = this.innerValue.length;
      } else if (!this.checked && index > -1) {
        this.innerValue.splice(index, 1);
        this.index = "";
      }
    },
  },
  mounted() {
    if (this.$parent.$options.name == "ElSortboxGroup") {
      this.innerValue = this.$parent.$props.value;
      this.max = this.$parent.$props.max;
      this.$watch("$parent.$props.value", this.updateCheckedState);
      this.$watch("$parent.$props.max", (v) => {
        this.max = v;
      });
    } else {
      this.innerValue = this.value;
      this.$watch("value", this.updateCheckedState);
    } 
  },
  watch: {
    checked: function() {
      this.updateinnerValueState();
    },
  },
};
</script>
<style lang="less">
.el-sortbox {
  line-height: normal;
  margin-right: 30px;
  display: inline-block;
  &.is-bordered {
    &:first-child {
      margin-left: 0px;
    }
    &.seleced {
      border-color: #409eff;
    }
    margin-left: 10px;
    border: 1px solid #dcdfe6;
    box-sizing: border-box;
    padding: 3px 15px 3px 10px;
    border-radius: 3px;
    height: 28px;
  }
  .el-sortbox-checkbox {
    outline: 0;
    cursor: pointer;
    line-height: 1;
    vertical-align: middle;
    display: inline-block;
    position: relative;
    border: 1px solid #dcdfe6;
    border-radius: 2px;
    box-sizing: border-box;
    font-size: 14px;
    text-align: center;
    min-width: 14px;
    height: 14px;
    background-color: #fff;
    z-index: 1;
    transition: border-color 0.25s cubic-bezier(0.71, -0.46, 0.29, 1.46), background-color 0.25s cubic-bezier(0.71, -0.46, 0.29, 1.46);
  }

  input[type="checkbox"]:checked {
    + .el-sortbox-checkbox {
      background-color: #409eff;
      border-color: #409eff;
      color: #fff;
    }
    + .el-sortbox-checkbox + .el-sortbox-label {
      color: #409eff;
    }
  }
  .el-sortbox-label {
    white-space: normal;
    line-height: 12px;
    font-size: 12px;
    display: inline;
    padding-left: 10px;
  }

  &.is-disabled {
    border-color: #ebeef5;
    cursor: not-allowed;
    input[type="checkbox"]:not(:checked) {
      + .el-sortbox-checkbox {
        background-color: #edf2fc;
        border-color: #dcdfe6;
        cursor: not-allowed;
      }
      + .el-sortbox-checkbox + .el-sortbox-label {
        color: #c0c4cc;
        cursor: not-allowed;
      }
    }
  }
}
</style>
