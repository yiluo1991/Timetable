<template>
  <div style=" margin:0  0px; ">
    <div class="designtitle">{{ question["text_" + lang] }}</div>
   
  </div>
</template>
<script>
export default {
  props: {
    question: Object,
    isEditing: Boolean,
    lang: String,
  },
  data() {
    return {
    
    };
  },
  methods: {
   
  },
};
</script>
<style lang="less">
.designtitle {
  color: #000;
  text-align: left;
  padding: 10px;
  font-size:18px;
  border-left: 3px solid rgb(30, 159, 219);
}
</style>
