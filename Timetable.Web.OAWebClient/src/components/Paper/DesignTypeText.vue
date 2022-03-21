<template>
  <div style="padding:20px 20px 10px 20px;border-bottom:1px dashed #ddd">
      
    <div class="nor" v-html="$xss(question['text_' + lang])" style="padding-bottom:15px;overflow:hidden"></div>
      
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
      rules: {
          text_CN:[
              {required:true,message:"必填"},
               {max:10240,min:1,message:"内容过长"}
          ],
          text_EN:[
               {required:true,message:"必填"},
               {max:10240,min:1,message:"内容过长"}
          ]
      },
    };
  },
  methods: {
    validate(callback) {
      this.$refs.form.validate(res=>{
          callback(res)
      })
    },
  },
};
</script>
<style lang="less">
.nor{
  font-weight: normal;
    img{
    max-width: 100%!important;
  }
}
</style>
