<template>
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
          :disabled="form.OriginIdentityCode"
          placeholder="请输入教工号"
        ></ElInput>
      </ElFormItem>
      <ElFormItem label="姓名" prop="RealName">
        <ElInput v-model="form.RealName" placeholder="请输入姓名" ></ElInput>
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
      genders: ["未知", "男", "女"],
  
      form: {},
      rules: {
        RealName: [
          { required: true, message: "必填" },
          { max: 64, message: "最长64字符" },
        ],
        IdentityCode: [{ max: 32, message: "最长32字符" },{
          validator:(field,value,callback)=>{
            
            if(this.hasCheckPermission){
              this.$axios.post(this.$baseURL + "/teacher/checkexist?code="+encodeURIComponent(this.form.IdentityCode)+"&skip="+encodeURIComponent(this.form.OriginIdentityCode)).then(res=>{
                if(res.data){
                  callback()
                }else{
                  callback(new Error('教工号已存在，请更换'))
                }
              })
            }else{
              return callback()
            }
          }

        }],
        Mobile:[{
          pattern:/^1\d{10}$/,message:'请输入11位手机号'
        }]
        
         
      },
    };
  },
  mounted() {},
  computed: {
    hasAddPermission() {
      return this.hasPermission("T_A");
    },
   
    hasEditPermission() {
      return this.hasPermission("T_M");
    },
    hasCheckPermission() {
      return this.hasPermission("T_C");
    },
  },
   
  methods: {
    
   showAddDialog() {
      if (this.hasAddPermission) {
        
          this.visiable = true;
          this.$nextTick(() => {
            this.form = { 
              RealName: "",
              Mobile: "",
              Enable: true,
              Gender: 0,
              IdentityCode: ''
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
            var url = this.$baseURL + "/Teacher/Add";
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
            var url = this.$baseURL + "/Teacher/Edit";
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
