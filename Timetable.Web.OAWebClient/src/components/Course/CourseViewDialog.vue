<template>
  <div>
    <ElDialog
      title="课程安排"
      :lock-scroll="false"
      :visible.sync="visiable"
      width="1000px"
      top="50px"
      :close-on-click-modal="false"
    >
      
        <CourseCalendar :editable="false" v-if="form" v-model="form.TimebookJson" :term="term" />
  
      <span slot="footer" class="dialog-footer">
        <ElButton @click="visiable = false" icon="el-icon-close"
          >关闭</ElButton
        >
      </span>
    </ElDialog>
   
  </div>
</template>
<script>

import CourseCalendar from "./CourseCalendar.vue";
export default {
  inject: ["hasPermission"],
  components: {
  
    CourseCalendar,
  },
  data() {
    return {
      visiable: false,
      comboData: [],
      term: undefined,
      form: {},
     
    };
  },
  mounted() {},
  computed: {
   
    hasGetCurrentSchoolTermPermission() {
      return this.hasPermission("C_GCT");
    },
    
    hasGetSubjectPermission() {
      return this.hasPermission("C_GS");
    },
  },

  methods: {
   
    show(row) {
    
       
        if (this.hasGetCurrentSchoolTermPermission) {
          this.$axios
            .post(this.$baseURL + "/schoolterm/getcurrentterm")
            .then((res) => {
              if (res.data.success) {
                this.term = res.data.data;
                this.visiable = true;
                this.form = { 
                  TimebookJson:  row.TimebookJson, 
                };
                
              } else {
                this.$message.error("无法获取当前学期");
              }
            });
        }
      
    }
  },
};
</script>
