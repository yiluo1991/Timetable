<template>
  <ElDialog title="选择院校" append-to-body top="5vh" custom-class="minipadding" :visible.sync="visiable" :close-on-click-modal="false" @close="destory">
    <ElInput v-model="keyword" size="small" prefix-icon="el-icon-search" placeholder="省份 / 学校名" clearable ></ElInput>
    <div class="schoolscrollview">
      <ElCollapse v-if="keyword === ''" accordion v-model="current">
        <ElCollapseItem v-for="item in schoolList" :key="item.province" :name="item.province" :title="item.province">
          <div v-if="current == item.province" class="clearfix">
            <div @click="select(s)" class="scfl schoolitem" v-for="(s, index) in item.list" :key="index">
              {{ s }}
            </div>
          </div>
        </ElCollapseItem>
      </ElCollapse>
      <div class="schoolsearchresult clearfix"  v-if="keyword !== ''" accordion :value="'a'">
          <div v-if="result.length==0" style="text-align:center;color:#777">
              未找到相关记录，请尝试更换关键词
          </div>
        <div @click="select(s)" class="fl schoolitem" v-for="(s, index) in result" :key="index">
          {{ s }}
        </div>
      </div>
    </div>
    <div style="text-align:center;padding-top:10px;">
      <ElButton size="small" @click="select('')"><i class="el-icon-close"></i> 清空题目中选择的院校</ElButton>
    </div>
  </ElDialog>
</template>
<script>
import { mapState, mapMutations } from "vuex";
export default {
  props: ["value"],
  data() {
    return {
      keyword: "",
      visiable: false,
      current: undefined,
    };
  },
  mounted() {},
  computed: {    result() {
        console.log("搜索");
        var result = [];
        if (this.keyword !== "") {
          this.schoolList.forEach((s) => {
            if (s.province.indexOf(this.keyword) > -1) {
              result.push.apply(result, s.list);
            } else {
              s.list.forEach((sc) => {
                if (sc.indexOf(this.keyword) > -1) {
                  result.push(sc);
                }
              });
            }
          });
        }
        return result;
      },
 
    ...mapState(["schoolList"]),
  },
  methods: {
    destory() {
      this.visiable = false;
      this.$emit("destory");
    },
    select(s) {
      this.$emit("input", s);
      this.destory();
    },
  
    showDialog() {
       console.log(this.schoolList)
      if (this.schoolList == undefined) {
     
        this.$axios.get("/api/school/school").then((res) => {
          var list = [];
          var dic = res.data.data;
          for (var i in dic) {
            list.push({
              province: i,
              list: dic[i],
            });
          }
          list.sort((a, b) => a.province.localeCompare(b.province));
          this.updateSchoolList(list);
          this.visiable = true;
        });
      } else {
        this.visiable = true;
      }
    },
    ...mapMutations(["updateSchoolList"]),
  },
};
</script>
<style lang="less">
.schoolitem {
  margin: 3px;
  font-size: 12px;
  border: 1px solid #ccc;
  background: #f1f1f1;
  padding: 4px 5px;
  &:active {
    background: #ddd;
  }
}
.schoolscrollview {
  margin-top: 10px;
  max-height: 65vh;
  overflow-y: scroll;
  .el-collapse-item__wrap {
    transition: none;
  }
}

.schoolsearchresult {
  padding: 10px 0;
}
.scfl{
  float:left;
}
</style>
