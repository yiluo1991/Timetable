<template>
  <div class="admin-page">
    <BasicToolbar
    ref="toolbar"
      :showAddButton="hasAddPermission"
      searchText="请输入查询关键字"
      @openAdd="openAdd"
      :showRefreshButton="hasGetPermission"
      :showSearchbox="hasGetPermission"
      @search="search"
      @reload="reload"
    >
     <template #default>
        学年：<ElSelect @change="searchByChange()" style="margin-right:10px" v-model="cid" size="medium">
          <ElOption
            v-for="year in years"
            :key="year.t"
            :label="year.t"
            :value="year.v"
          ></ElOption>
        </ElSelect>
        学期：<ElSelect  @change="searchByChange()"
          style="margin-right:10px"
          v-model="intFlag"
          size="medium"
        >
          <ElOption label="不限制" :value="undefined"></ElOption>
          <ElOption label="第一学期" :value="'1'"></ElOption>
          <ElOption label="第二学期" :value="'2'"></ElOption>
        </ElSelect>
      </template>
    </BasicToolbar>
    <CoursePickTable   ref="table" />
    <CoursePickDialog ref="dialog" @reload="reload()" />
  </div>
</template>
<script>
import CoursePickDialog from "../components/CoursePick/CoursePickDialog.vue";
import CoursePickTable from "../components/CoursePick/CoursePickTable.vue";
export default {
  components: {
    CoursePickDialog,
    CoursePickTable,
  },
  name: "admincoursepick",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("CP_G");
    },
    hasAddPermission(){
      return this.hasPermission("CP_A");
    }
  
  },
   mounted() {
    for (var i = 2021; i < 2100; i++) {
      this.years.push({ t: i, v: i.toString() });
    }
  },
  data() {
    return {
      years: [{ t: "不限制", v: undefined }],
      cid: undefined,
      intFlag: undefined,
    };
  },
  methods: {
   searchByChange(){
       this.$refs.table.reload(this.$refs.toolbar.keyword,this.cid,this.intFlag);
    },
    search(e) {
      this.$refs.table.reload(e,this.cid,this.intFlag);
    },
    reload() {
      this.$refs.table.reload();
    },
    /**打开添加窗口 */
    openAdd() {
      this.$refs.dialog.showAddDialog();
    },
     
  },
};
</script>
