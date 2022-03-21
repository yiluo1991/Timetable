<template>
    <div class="el-sortbox-group">
        <slot></slot>
    </div>
</template>
<script>
export default {
    name:"ElSortboxGroup",
    props:[
        "value",
        "max"
    ]
}

</script>
<style lang="less">
.el-sortbox-group{
        line-height: 28px;
}
</style>