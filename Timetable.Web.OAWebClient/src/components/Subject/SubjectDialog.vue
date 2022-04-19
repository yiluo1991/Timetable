<template>
  <ElDialog
    :title="form.Id ? '修改' : '添加'"
    :lock-scroll="false"
    :visible.sync="visiable"
    width="600px"
    :close-on-click-modal="false"
  >
    <ElForm :model="form" ref="form" label-width="80px" :rules="rules">
     
      <ElFormItem label="开设单位" prop="SID">
        <BasicSelectTree
          :width="455"
          ref="select"
          :data="comboData"
          node-key="SID"
          :default-props="{ label: 'Name' }"
          :height="400"
          v-model="form.CD"
          placeholder="请选科开设单位"
        ></BasicSelectTree>
      </ElFormItem>
      <ElFormItem label="科目代码" prop="SubjectCode">
        <ElInput
          v-model="form.SubjectCode"
          :disabled="form.OriginSubjectCode"
          placeholder="请输入课程代码"
        ></ElInput>
      </ElFormItem>
      <ElFormItem label="科目名" prop="Name">
        <ElInput v-model="form.Name" placeholder="请输入科目名"></ElInput>
      </ElFormItem>
      <ElFormItem label="课程类型">
        <ElSelect style="width:100%" v-model="form.SubjectType" placeholder="请选择课程类型">
          <ElOption
            v-for="(type, index) in types"
            :key="index"
            :label="type"
            :value="index"
          ></ElOption>
        </ElSelect>
      </ElFormItem>
      <ElFormItem label="课程性质">
        <ElSelect style="width:100%" v-model="form.SubjectProperty" placeholder="请选择课程性质">
          <ElOption
            v-for="(type, index) in props"
            :key="index"
            :label="type"
            :value="index"
          ></ElOption>
        </ElSelect>
      </ElFormItem>
      <ElFormItem label="状态" prop="Enable">
        <ElSelect style="width:100%" v-model="form.Enable">
          <ElOption label="启用" :value="true"></ElOption>
          <ElOption label="禁用" :value="false"></ElOption>
        </ElSelect>
      </ElFormItem>
    </ElForm>
    <span slot="footer" class="dialog-footer">
      <ElButton @click="visiable = false" icon="el-icon-close">取 消</ElButton>
      <ElButton type="primary" @click="save()" icon="el-icon-check"
        >确 定</ElButton
      >
    </span>
  </ElDialog>
</template>
<script>
export default {
  inject: ["hasPermission"],
  data() {
    return {
      visiable: false,
      comboData: [],
      types: ["未知", "通识课", "专业课", "实践课"],
      props: ["未知", "必修", "选修"],
      form: {},
      rules: {
        Name: [
          { required: true, message: "必填" },
          { max: 128, message: "最长128字符" },
        ],
        SubjectCode: [{ max: 32, message: "最长32字符" },{
          validator:(field,value,callback)=>{
            
            if(this.hasCheckPermission){
              this.$axios.post(this.$baseURL + "/subject/checkexist?code="+encodeURIComponent(this.form.SubjectCode)+"&skip="+encodeURIComponent(this.form.OriginSubjectCode)).then(res=>{
                if(res.data){
                  callback()
                }else{
                  callback(new Error('科目代码已存在，请更换'))
                }
              })
            }else{
              return callback()
            }
          }

        }],
        
         
      },
    };
  },
  mounted() {},
  computed: {
    hasAddPermission() {
      return this.hasPermission("S_A");
    },
    hasComboPermission() {
      return this.hasPermission("S_GCD");
    },
    hasEditPermission() {
      return this.hasPermission("S_M");
    },
    hasCheckPermission() {
      return this.hasPermission("S_C");
    },
  },
  watch: {
    "form.CD": function(a) {
      var vs = a.replace("C_", "").split("D_");
      if (this.form) {
        this.form.CollegeId = vs[0];
        this.form.DepartmentId = vs[1];
      }
    },
  },
  methods: {
    async loadCombo() {
      if (this.hasComboPermission) {
        var url = this.$baseURL + "/CollegeDepartment/combodata";
        try {
          var t = await this.$axios.post(url);
          if (t.data.success) {
            this.comboData = t.data.data;
            return true;
          } else {
            return false;
          }
        } catch (error) {
          return false;
        }
      } else {
        return false;
      }
    },
    async showAddDialog() {
      if (this.hasAddPermission) {
        if (await this.loadCombo()) {
          this.visiable = true;
          this.$nextTick(() => {
            this.form = {
              SubjectCode: "",
              Name: "",
              CollegeId: "",
              DepartmentId: "",
              Enable: true,
              SubjectType: 0,
              SubjectProperty: 0,
              CD:""
            };
            this.$nextTick(()=>{
              this.$refs.form.clearValidate();
            })
          });
        } else {
          this.$message.error("开设单位获取失败，可能您没有相关权限");
        }
      }
    },
    async showEditDialog(row) {
      if (this.hasEditPermission) {
        if (await this.loadCombo()) {
          this.visiable = true;
          this.$nextTick(() => {
            this.form = row;
            this.$refs.form.clearValidate();
          });
        } else {
          this.$message.error("开设单位获取失败，可能您没有相关权限");
        }
      }
    },
    save() {
      if (this.form.OriginSubjectCode == undefined && this.hasAddPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var url = this.$baseURL + "/Subject/Add";
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
      } else if (this.form.OriginSubjectCode && this.hasEditPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var url = this.$baseURL + "/Subject/Edit";
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
