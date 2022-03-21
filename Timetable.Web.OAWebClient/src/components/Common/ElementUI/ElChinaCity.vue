<template>
  <ElCascader :props="props" :options="mapData" :placeholder="placeholder" v-model="innerData" :clearable="clearable"></ElCascader>
</template>
<script>
import mapData from "../../../data/city";
mapData.forEach((item) => {
  item.value = item.name;
  item.children.forEach((child) => {
    child.value = item.name + "/" + child.name;
  });
});
export default {
  props: {
    model: { type: Object },
    provinceField: { type: String },
    cityField: { type: String },
    placeholder: { Type: String, default: "- 请选择 -" },
    questionId: { Type: String },
    clearable:{
      Type:Boolean,
      default:true
    }
  },
  data() {
    return {
      mapData,
      innerData: [],
      props: {
        lazy: false,
        label: "name",
        value: "value",
        expandTrigger: "hover",
      },
    };
  },
  mounted() {
    if (this.model && this.provinceField && this.cityField) {
      this.$watch("model." + this.questionId, () => {
        if (this.model[this.questionId] && this.model[this.questionId][this.provinceField] && this.model[this.questionId][this.cityField]) {
          this.innerData = [this.model[this.questionId][this.provinceField], this.model[this.questionId][this.cityField]];
          console.log(this.innerData);
        } else {
          this.innerData = [];
        }
      });
    }
    if (this.model && this.model[this.questionId] && this.model[this.questionId][this.provinceField] && this.model[this.questionId][this.cityField]) {
      this.innerData = [this.model[this.questionId][this.provinceField], this.model[this.questionId][this.cityField]];
    }
  },
  watch: {
    innerData(value) {
      if (value.length == 2) {
        if (!this.model[this.questionId]) {
          this.$set(this.model, this.questionId, {
            [this.provinceField]: value[0],
            [this.cityField]: value[1],
          });
        } else {
          this.$set(this.model[this.questionId], this.provinceField, value[0]);
          this.$set(this.model[this.questionId], this.cityField, value[1]);
        }
      } else {
        if (this.model) {
         this.$delete(this.model, this.questionId);
        }
      }
      if (this.$parent && this.$parent.form) {
        this.$parent.validate();
      }
    },
  },
  methods: {},
};
</script>
