<template>
  <div class="designitem" :class="{ error: question.hasError }">

    <component  :answers="answers" :numObj="numObj" :lang="lang" :isEditing="false" :is="componentName" ref="content" :question="question">
  
     
        
    </component>
  
  
  </div>
</template>
<script>
var available = ["PaperBreak", "Title", "Text", "Single", "Select", "Multi", "ScoreSingle", "ScoreMulti", "MatrixSingle", "Nps", "InlineSingle", "TextInput", "NumberInput", "MatrixInlineSingle","Rate","ChinaCity","School","Sort"];
export default {
  components: {
    //按下面的写法让题目组件让webpack全部打包到DesignTypeCollection包中 
    DesignTypePaperBreak: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypePaperBreak"),
    DesignTypeTitle: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeTitle"),
    DesignTypeText: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeText"),
    DesignTypeSingle: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeSingle"),
    DesignTypeMulti: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeMulti"),
    DesignTypeScoreSingle: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeScoreSingle"),
    DesignTypeNps: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeNps"),
    DesignTypeInlineSingle: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeInlineSingle"),
    DesignTypeTextInput: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeTextInput"),
    DesignTypeNumberInput: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeNumberInput"),
    DesignTypeSelect: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeSelect"),
    DesignTypeScoreMulti: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeScoreMulti"),
    DesignTypeMatrixSingle: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeMatrixSingle"),
    DesignTypeMatrixInlineSingle: () => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeMatrixInlineSingle"),
    DesignTypeRate:() => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeRate"),
    DesignTypeChinaCity:() => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeChinaCity"),
        DesignTypeSchool:() => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeSchool"),
    DesignTypeSort:() => import(/*webpackChunkName:"DesignTypeCollection"*/ "../Paper/DesignTypeSort"),
  },
  props: {
    question: Object, 
    lang: String,
    numObj: Object,
    answers:Object
  },

  computed: {
   
    componentName() {
     
      if (available.indexOf(this.question.type) > -1) {
        return "DesignType" + this.question.type;
      } else {
        return "";
      }
    },
  },
  methods: {
    
    
  },
};
</script>
<style lang="less">

</style>
