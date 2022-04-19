<template>
<div>
  <ElDialog
    :title="form.Id ? '修改' : '添加'"
    :lock-scroll="false"
    :visible.sync="visiable"
    width="600px"
    :close-on-click-modal="false"
  >
    <ElForm :model="form" ref="form" label-width="80px" :rules="rules"> 
       <ElFormItem prop="CourseText" label="课程">
         <ElInput v-model="form.CourseText" placeholder="请选择课程">
           <template #append>
                  <ElButton type="info" @click="pickCourse"
                    >选择课程</ElButton
                  >
                </template>
         </ElInput>
       </ElFormItem>
        <ElFormItem  prop="StudentText" label="学生">
         <ElInput v-model="form.StudentText" placeholder="请选择学生">
           <template #append>
                  <ElButton type="info" @click="pickStudent"
                    >选择学生</ElButton
                  >
                </template>
         </ElInput>
       </ElFormItem>
      
    </ElForm>
    <span slot="footer" class="dialog-footer">
      <ElButton @click="visiable = false" icon="el-icon-close">取 消</ElButton>
      <ElButton type="primary" @click="save()" icon="el-icon-check"
        >确 定</ElButton
      >
    </span>
  </ElDialog>
   <CoursePickPickStudentDialog ref="psd" @pick="onPickStudent" />
    <CoursePickPickCourseDialog ref="pcd" @pick="onPickCourse" />
   
  </div>
</template>
<script>
 import CoursePickPickStudentDialog from './CoursePickPickStudentDialog.vue';
  import CoursePickPickCourseDialog from './CoursePickPickCourseDialog.vue';
export default {
  inject: ["hasPermission"],
  components:{
     CoursePickPickStudentDialog,
     CoursePickPickCourseDialog
  },
  data() {
    return {
      visiable: false,
      comboData: [],
      
      form: {},
      rules: {
        CourseText:[{required:true,message:'请选择一个课程'}],
        StudentText:[{required:true,message:'请选择一个学生'}]
      },
    };
  },
  mounted() {},
  computed: {
    hasAddPermission() {
      return this.hasPermission("CP_A");
    },
    hasGetStudentPermission() {
      return this.hasPermission("CP_GS");
    },
    hasGetCoursePermission() {
      return this.hasPermission("CP_GC");
    },
     
  },
   
  methods: {
   pickStudent(){
     this.$refs.psd.show();
   },
   pickCourse(){
      this.$refs.pcd.show();
   },
   onPickStudent(e){
     console.log(e)
     this.form.StudentIdentityCode=e.IdentityCode;
     this.form.StudentText=e.StudentText;
   },
   onPickCourse(e){
     console.log(e)
     this.form.CourseId=e.Id;
     this.form.CourseText=e.CourseText;
   },
   showAddDialog() {
      if (this.hasAddPermission) { 
          this.visiable = true;
          this.$nextTick(() => {
            this.form = { 
               CourseText:'',
               CourseId:'',
               StudentIdentityCode:'',
               StudentText:''
            };
            this.$nextTick(()=>{
              this.$refs.form.clearValidate();
            })
          });
        
      }
    },
     
    save() {
      if ( this.hasAddPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var url = this.$baseURL + "/coursepick/Add";
            this.$loading({ text: "处理中" });
            var formData=new FormData();
            formData.append('courseId',this.form.CourseId);
            formData.append('studentId',this.form.StudentIdentityCode);
            this.$axios.post(url, formData).then((res) => {
              setTimeout(() => {
                this.$loading().close();
              }, 100);
              this.$emit("reload");
              this.visiable = false;
              if (!res.data.success) {
                this.$message.error(res.data.msg);
              }
            
            });
          }
        });
      }
    },
  },
};
</script>
