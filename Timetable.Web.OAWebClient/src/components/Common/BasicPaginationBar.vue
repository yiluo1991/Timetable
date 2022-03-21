<template>
  <ElPagination
    style="margin-top:10px;"
    :current-page.sync="p"
    :page-sizes="[10, 15, 20,25, 30]"
    :page-size.sync="s"
    background
    layout="total,sizes, prev, pager, next,jumper,slot"
    @current-change="$emit('current-change')"
    @size-change="$emit('size-change')"
    :total="total"
  >
  </ElPagination>
</template>
<style lang="less">

</style>
<script>
export default {
    props: {
        page:{
            type:Number,
            default:1
        },
        size:{
            type:Number,
            default:15
        },
        total:{
            type:Number,
            default:0
        }
    },data () {
        return {
            p:this.page,
            s:this.size
        }
    },
    watch:{
        s(v){
            this.$emit('update:size',v)
        },
        p(v){
            this.$emit('update:page',v)
        },
        page(v){
            this.p=v;
        }
    }
 
}
</script>