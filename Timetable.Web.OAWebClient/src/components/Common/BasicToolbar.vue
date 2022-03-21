<template>
  <div class=" toolbar" style="overflow: hidden;">
    <ElButton v-if="showAddButton" type="success"  icon="el-icon-plus" size="medium" style="margin-right:10px" plain round @click="clickAdd">{{addText}}</ElButton>

    <slot name="default">
      

    </slot>

     <ElInput autocomplete="off" v-if="showSearchbox"  @keyup.13.native="search"  v-model="keyword" style="width:360px;margin-right:10px;"  size="medium" :placeholder="searchText" >
        <template slot="prepend">搜索</template>
        <ElButton slot="append" icon="el-icon-search" @click="search"></ElButton>
    </ElInput>
    
    <ElButton v-if="showRefreshButton" type="primary" style="margin-right:10px;" icon="el-icon-refresh" size="medium"  plain round @click="reload">{{refreshText}}</ElButton>
     <slot name="append">
      

    </slot>

  </div>
</template>

<style lang="less" scoped>

</style>

<script>
export default {

  props: {
    showAddButton: {
      type: Boolean,
      default: true
    },
    showRefreshButton: {
      type: Boolean,
      default: true
    },
    showSearchbox: {
      type: Boolean,
      default: true
    },
    addText: {
      type: String,
      default: "添加"
    },
    refreshText: {
      type: String,
      default: "刷新"
    },
    searchText: {
      type: String,
      default: "请输入查询关键字"
    }
  },
  methods: {
    clickAdd() {
      this.$emit("openAdd")
    },
    reload(){
       this.$emit("reload")
    },
     search(){
       this.$emit("search",this.keyword)
    }
  },
  data () {
      return {
          keyword:""
      }
  }
};
</script>