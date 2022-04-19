<template>
  <div class="admin-page">
    <BasicToolbar
      ref="toolbar"
      searchText="请输入查询关键字"
      :showAddButton="false"
      :showRefreshButton="hasGetPermission"
      :showSearchbox="hasGetPermission"
      @search="search"
      @reload="reload"
    >
      <template #default>
        学年：<ElSelect
          @change="searchByChange()"
          style="margin-right:10px"
          v-model="cid"
          size="medium"
        >
          <ElOption
            v-for="year in years"
            :key="year.t"
            :label="year.t"
            :value="year.v"
          ></ElOption>
        </ElSelect>
        学期：<ElSelect
          @change="searchByChange()"
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
    <AllCourseTable ref="table"  @view="view"/>
    <CourseViewDialog ref="viewdialog" />
  </div>
</template>
<script>
import CourseViewDialog from "../components/Course/CourseViewDialog.vue";
import AllCourseTable from "../components/AllCourse/AllCourseTable.vue";
export default {
  components: {
    AllCourseTable,
    CourseViewDialog,
  },
  name: "adminstudent",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("C_GAC");
    },
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
    view(row) {
      this.$refs.viewdialog.show(row);
    },
    searchByChange() {
      this.$refs.table.reload(
        this.$refs.toolbar.keyword,
        this.cid,
        this.intFlag
      );
    },
    search(e) {
      this.$refs.table.reload(e, this.cid, this.intFlag);
    },

    reload() {
      this.$refs.table.reload();
    },
  },
};
</script>
