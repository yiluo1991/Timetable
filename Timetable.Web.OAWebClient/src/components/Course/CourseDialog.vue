<template>
  <div>
    <ElDialog
      :title="form.Id ? '修改' : '添加'"
      :lock-scroll="false"
      :visible.sync="visiable"
      width="1000px"
      :close-on-click-modal="false"
      top="50px"
    >
      <ElForm
        v-if="form"
        :model="form"
        ref="form"
        label-width="80px"
        :rules="rules"
      >
        <ElRow :gutter="10">
          <ElCol :span="12">
            <ElFormItem label="选择科目" prop="SubjectText">
              <ElInput
                v-model="form.SubjectText"
                placeholder="请选择科目"
                readonly
              >
                <template #append>
                  <ElButton type="info" @click="pickSubject">选择科目</ElButton>
                </template>
              </ElInput>
            </ElFormItem>
            <ElFormItem label="选择教师" prop="TeacherText">
              <ElInput
                v-model="form.TeacherText"
                placeholder="请选择授课教师"
                readonly
              >
                <template #append>
                  <ElButton type="info" @click="pickTeacher">选择教师</ElButton>
                </template>
              </ElInput>
            </ElFormItem>
          </ElCol>
          <ElCol :span="12">
            <ElFormItem label="识别名" prop="CourseName">
              <ElInput
                v-model="form.CourseName"
                placeholder="教师一门课上多个班时请加以区分，如“线性代数-1班”"
              />
            </ElFormItem>
            <ElFormItem label="班级" prop="ClassText">
              <ElInput
                v-model="form.ClassText"
                placeholder="如果是全班上课，请选择班级"
                readonly
              >
                <template #append>
                  <ElButton type="info" @click="pickAdministrativeClass"
                    >选择班级</ElButton
                  >
                </template>
              </ElInput>
            </ElFormItem>
          </ElCol>
        </ElRow>
        <ElFormItem
          v-if="form.TimebookJson"
          label="上课时间"
          prop="TimebookJson.Desc"
        >
          <ElInput
            v-model="form.TimebookJson.Desc"
            max="256"
            placeholder="简要输入上课时间信息，如“1~10周，周一1~4节、周三5~8节”"
          ></ElInput>
        </ElFormItem>
         <ElFormItem 
          label="上课地点"
          prop="Address"
        >
          <ElInput
            v-model="form.Address"
            max="128"
            placeholder="请输入上课地点"
          ></ElInput>
        </ElFormItem>
      </ElForm>
      <div style="border:1px solid #eee;border-radius:5px;padding:15px;">
        <CourseCalendar v-if="form" v-model="form.TimebookJson" :term="term" />
      </div> 
      <span slot="footer" class="dialog-footer">
        <ElButton @click="visiable = false" icon="el-icon-close"
          >取 消</ElButton
        >
        <ElButton @click="showAddDialog" icon="el-icon-refresh-left"
          >重置</ElButton
        >
        <ElButton type="primary" @click="save()" icon="el-icon-check"
          >确 定</ElButton
        >
      </span>
    </ElDialog>
    <CoursePickClassDialog
      @pick="onPickAdministrativeClass"
      ref="pickAdministrativeClassDialog"
    />
    <CoursePickTeacherDialog @pick="onPickTeacher" ref="pickTeacherDialog" />
    <CoursePickSubjectDialog ref="pickSubjectDialog" @pick="onPickSubject" />
  </div>
</template>
<script>
import CoursePickClassDialog from "./CoursePickClassDialog.vue";
import CoursePickTeacherDialog from "./CoursePickTeacherDialog.vue";
import CoursePickSubjectDialog from "./CoursePickSubjectDialog.vue";
import CourseCalendar from "./CourseCalendar.vue";
export default {
  inject: ["hasPermission"],
  components: {
    CoursePickClassDialog,
    CoursePickTeacherDialog,
    CoursePickSubjectDialog,
    CourseCalendar,
  },
  data() {
    return {
      visiable: false,
      comboData: [],
      term: undefined,
      form: {},
      rules: {
        ClassText: [],
        TeacherText: [{ required: true, message: "必须选择一个授课老师" }],
        SubjectText: [{ required: true, message: "必须选择一个科目" }],
        CourseName: [
          
          { max: 128, message: "最多128个字符" },
        ],
        "TimebookJson.Desc": [
          { required: true, message: "请输入上课时间描述" },
        ],
        Address:[
           { required: true, message: "必填" },
            { max: 128, message: "最多128个字符" },
        ]
      },
    };
  },
  mounted() {},
  computed: {
    hasAddPermission() {
      return this.hasPermission("C_A");
    },
    hasGetCurrentSchoolTermPermission() {
      return this.hasPermission("C_GCT");
    },
    hasGetAdministrativeClassPermission() {
      return this.hasPermission("C_GAC");
    },
    hasGetTeacherPermission() {
      return this.hasPermission("C_GT");
    },
    hasGetSubjectPermission() {
      return this.hasPermission("C_GS");
    },
  },

  methods: {
    clearClass() {
      this.form.AdministrativeClassId = undefined;
      this.form.ClassText = "";
    },
    pickAdministrativeClass() {
      this.$refs.pickAdministrativeClassDialog.show();
    },
    onPickAdministrativeClass(e) {
      this.form.AdministrativeClassId = e.AdministrativeClassId;
      this.form.ClassText = e.ClassText;
    },
    pickTeacher() {
      this.$refs.pickTeacherDialog.show();
    },
    onPickTeacher(e) {
      this.form.TeacherIdentityCode = e.TeacherIdentityCode;
      this.form.TeacherText = e.RealName;
    },
    pickSubject() {
      this.$refs.pickSubjectDialog.show();
    },
    onPickSubject(e) {
      this.form.SubjectCode = e.SubjectCode;
      this.form.SubjectText = e.Name;
    },
    showAddDialog() {
      if (this.hasAddPermission) {
        if (this.hasGetCurrentSchoolTermPermission) {
          this.$axios
            .post(this.$baseURL + "/schoolterm/getcurrentterm")
            .then((res) => {
              if (res.data.success) {
                this.term = res.data.data;
                this.visiable = true;
                this.form = {
                  SubjectCode: "",
                  CourseName: "",
                  TeacherIdentityCode: 0,
                  AdministrativeClassId: "",
                  TimebookJson: {
                    Book: {},
                    Desc: "",
                  },
                  Address:"",
                  ClassText: "",
                  TeacherText: "",
                  SubjectText: "",
                };
                this.$nextTick(() => {
                  this.$nextTick(() => {
                    this.$refs.form.clearValidate();
                  });
                });
              } else {
                this.$message.error("无法获取当前学期");
              }
            });
        }
      }
    },
    async showEditDialog(row) {
      if (this.hasEditPermission) {
        this.visiable = true;
        this.$nextTick(() => {
          this.form = row;
          this.$refs.form.clearValidate();
        });
      }
    },
    save() {
      if (this.hasAddPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var count = 0;
            for (var i in this.form.TimebookJson.Book) {
              if (this.form.TimebookJson.Book[i].length == 0) {
                this.$delete(this.form.TimebookJson.Book, i);
              } else {
                count++;
              }
            }
            if (count > 0) {
              var url = this.$baseURL + "/course/add";
              this.$loading({ text: "处理中" });

              this.$axios.post(url, this.form).then((res) => {
                setTimeout(() => {
                  this.$loading().close();
                }, 100);
                this.$emit("reload");
                this.visiable = false;
                if (!res.data.success) {
                  this.$message.error(res.data.msg);
                }
              });
            } else {
              this.$message.warning('未设置课程安排')
            }
          }
        });
      } else if (this.form.OriginIdentityCode && this.hasEditPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var url = this.$baseURL + "/student/Edit";
            this.$loading({ text: "处理中" });
            this.$axios.post(url, this.form).then((res) => {
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
      //this.visiable = false;
    },
  },
};
</script>
