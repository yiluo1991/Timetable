<template>
  <div class="admin-page">
    <BasicToolbar
      :showAddButton="hasEditPermission"
      searchText="请输入查询关键字"
      @openAdd="openAdd"
      :showRefreshButton="hasGetPermission"
      :showSearchbox="hasGetPermission"
      @search="search"
      @reload="reload"
    >
    </BasicToolbar>
    <CourseTable @openEdit="openEdit" ref="table" @view="view" />
    <CourseDialog ref="dialog" @reload="reload()" />
    <CourseViewDialog ref="viewdialog" />
  </div>
</template>
<script>
import CourseDialog from "../components/Course/CourseDialog.vue";
import CourseTable from "../components/Course/CourseTable.vue";
import CourseViewDialog from "../components/Course/CourseViewDialog.vue";
export default {
  components: {
    CourseTable,
    CourseDialog,
    CourseViewDialog
  },
  name: "adminstudent",
  inject: ["hasPermission"],
  computed: {
    hasGetPermission() {
      return this.hasPermission("SU_G");
    },
    hasEditPermission() {
      return this.hasPermission("SU_A");
    },
  },
  data() {
    return {};
  },
  methods: {
    view(row){
       
      this.$refs.viewdialog.show(row)
    },
    search(e) {
      this.$refs.table.reload(e);
    },
    reload() {
      this.$refs.table.reload();
    },
    /**打开添加窗口 */
    openAdd() {
      this.$refs.dialog.showAddDialog();
    },
    /**打开修改窗口 */
    openEdit(row) {
      this.$refs.dialog.showEditDialog(row);
    },
  },
};
</script>
