<template>
  <div :class="{ noevent: noevent }" @click.stop>
    <div v-for="item in question.items" :key="item.fid">
       {{item["text_"+lang]}}
      <ElSlider :show-tooltip="false"   v-model="value[item.fid]" :max="question.total" show-input @change="input($event, item.fid)" :show-input-controls="false" input-size="mini"></ElSlider>
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
    question: Object,
     value: {
      type: Object,
      default:function(){
        return {}
      }
    },
  },
  watch: {},
  methods: {
    input(value, prop) {
       if(this.value==undefined){
         this.value={};
       }
      var lastprop = this.question.items[this.question.items.length - 1].fid;
      if (prop != lastprop) {
          
        var total = 0;
        var limit = this.question.total;
        for (var i = 0; i < this.question.items.length; i++) {
          if (i < this.question.items.length - 1) { 
            total += this.value[this.question.items[i].fid];
          }
        }
        if (limit > total) {
         this.value[lastprop] = limit - total;
        } else {
          this.value[lastprop] = 0;
        }
      }
    },
  },
  mounted () {
    this.$watch(
      "value",
      () => {
         this.$emit("input", this.value) 
        if (this.$parent && this.$parent.form) {
  
         // this.$parent.validate();
        }
      },
      { deep: true }
    ); 
  },
};
</script>
<style lang="less"></style>
