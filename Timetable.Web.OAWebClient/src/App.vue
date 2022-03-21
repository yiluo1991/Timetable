<template>
  <div id="app">
    <RouterView />
  </div>
</template>
<script>
export default { 
 
}
</script>
<style lang="less">
.el-upload-list__item{
  transition:none!important;
}
.clearfix::after {
  content: "";
  display: block;
  clear: both;
}
.clearfix {
  zoom: 1;
}
</style>
