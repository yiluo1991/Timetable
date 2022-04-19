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

      <ElFormItem label="科目代码" prop="IdentityCode">
        <ElInput
          v-model="form.IdentityCode"
          :disabled="form.OriginIdentityCode!=undefined"
          placeholder="请输入学号"
        ></ElInput>
      </ElFormItem>
      <ElFormItem label="入学年份" prop="AdmissionYear">
        <ElInput
          v-model="form.AdmissionYear"
          placeholder="请输入入学年份，例：“2020”"
        ></ElInput>
      </ElFormItem>
      <ElFormItem label="姓名" prop="RealName">
        <ElInput v-model="form.RealName" placeholder="请输入姓名" ></ElInput>
      </ElFormItem>
        <ElFormItem label="班级" prop="ClassText">
        <ElInput v-model="form.ClassText" placeholder="请选择班级" readonly >
          <template #append>
            <ElButton type="info" @click="pk">选择班级</ElButton>
          </template>
        </ElInput>
      </ElFormItem>
       <ElFormItem label="手机号" prop="Mobile">
        <ElInput v-model="form.Mobile" placeholder="手机号"></ElInput>
      </ElFormItem>
      <ElFormItem label="性别">
        <ElSelect style="width:100%" v-model="form.Gender" placeholder="性别">
          <ElOption
            v-for="(gender, index) in genders"
            :key="index"
            :label="gender"
            :value="index"
          ></ElOption>
        </ElSelect>
      </ElFormItem>
     
      <ElFormItem label="状态" prop="StudentState">
        <ElSelect style="width:100%" v-model="form.StudentState">
          <ElOption v-for="(state,index) in states" :key="index" :label="state" :value="index"></ElOption> 
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
  <StudentPickClassDialog @pick="pick"  ref="pkdialog"/>
  </div>
</template>
<script>
import StudentPickClassDialog from './StudentPickClassDialog.vue'
export default {
  inject: ["hasPermission"],
  components:{
    StudentPickClassDialog
  },
  data() {
    return {
      visiable: false,
      comboData: [],
      genders: ["未知", "男", "女"],
      states:['在读','毕业','休学','辍学'],
      form: {},
      rules: {
        RealName: [
          { required: true, message: "必填" },
          { max: 64, message: "最长64字符" },
        ],
        IdentityCode: [{ max: 32, message: "最长32字符" },{
          validator:(field,value,callback)=>{ 
            if(this.hasCheckPermission){
              this.$axios.post(this.$baseURL + "/student/checkexist?code="+encodeURIComponent(this.form.IdentityCode)+"&skip="+encodeURIComponent(this.form.OriginIdentityCode)).then(res=>{
                if(res.data){
                  callback()
                }else{
                  callback(new Error('学号已存在，请更换'))
                }
              })
            }else{
              return callback()
            }
          }

        }],
        Mobile:[{
          pattern:/^1\d{10}$/,message:'请输入11位手机号'
        }],
         AdmissionYear: [
          { required: true, message: "必填" },
          {
            pattern: /^20\d{2}$/,
            message: "请输入4位数字年份，范围：2000~2099",
          },
        ], 
        ClassText:[
          {required:true,message:'必须选择一个班级'}
        ]
      },
    };
  },
  mounted() {},
  computed: {
    hasAddPermission() {
      return this.hasPermission("SU_A");
    },
    hasEditPermission() {
      return this.hasPermission("SU_M");
    },
    hasCheckPermission() {
      return this.hasPermission("SU_C");
    },
    hasGetAdministrativeClassPermission() {
      return this.hasPermission("SU_GAC");
    }, 
  },
   
  methods: {
    pk(){
      this.$refs.pkdialog.show();
    },
    pick(e){
      console.log(e)
      this.form.AdministrativeClassId=e.AdministrativeClassId;
      this.form.ClassText=e.ClassText;
      this.form.CollegeId=e.CollegeId;
      this.form.DepartmentId=e.DepartmentId;
    },  
   showAddDialog() {
      if (this.hasAddPermission) {
        
          this.visiable = true;
          this.$nextTick(() => {
            this.form = { 
              RealName: "",
              Mobile: "",
              Gender: 0,
              IdentityCode: '',
              StudentState:0,
              AdministrativeClassId:'',
              AdmissionYear:'',
              CollegeId:'',
              DepartmentId:'',
              ClassText:''
            };
            this.$nextTick(()=>{
              this.$refs.form.clearValidate();
            })
          });
        
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
      if (this.form.OriginIdentityCode == undefined && this.hasAddPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var url = this.$baseURL + "/student/Add";
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
