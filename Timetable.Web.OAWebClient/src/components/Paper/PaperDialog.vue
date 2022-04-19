<template>
  <ElDialog
    :title="(this.form.Id?'修改':'添加')+ '问卷地址'"
    :lock-scroll="false"
    :visible.sync="visiable"
    width="600px"
    :close-on-click-modal="false"
  >
    <ElForm :model="form" ref="form" label-width="80px" :rules="rules">
      <ElFormItem label="标题" prop="Title">
        <ElInput v-model="form.Title" placeholder="请输入标题"></ElInput>
      </ElFormItem>
      <ElFormItem label="跳转地址" prop="Url">
        <ElInput v-model="form.Url" placeholder="请输入跳转地址"></ElInput>
      </ElFormItem>
      <ElFormItem label="排序号" prop="SN">
         <ElInput v-model="form.SN" placeholder="请输入排序号，按升序排序"></ElInput>
      </ElFormItem>
      <ElFormItem label="状态" prop="Enable">
          <ElSelect v-model="form.Enable"   style="width:100%">
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
      years: [],
      form: {
        Id: undefined,
        Title: "",
        Url: "",
        Enable: true,
      },
      rules: {
        Title:[
          {required:true,message:'必填'},
          {max:256,message:'最长256字符'}
        ],
        Url:[
          {required:true,message:'必填'},
          {pattern:/^http(s)?:\/\/.+$/,message:'请输入“http://”或“https://”开头的网页地址'},
          {max:512,message:'最长512字符'}
        ],
        SN:[
           {required:true,message:'必填'},
           {pattern:/^\d{1,4}$/,message:'请输入数值，0~9999'},
        ]
      },
    };
  },
  mounted() {},
  computed: {
    hasAddPermission() {
      return this.hasPermission("P_A");
    },
    hasEditPermission() {
      return this.hasPermission("P_M");
    },
  },
  methods: {
    showAddDialog() {
      if (this.hasAddPermission) {
        this.form = {
          Id: undefined,
          Title: "",
          Url: "",
          Enable: true,
        };
        this.visiable=true;
        this.$nextTick(() => {
          this.$refs.form.clearValidate();
        });
      }
    },
    showEditDialog(e) {
      if (this.hasEditPermission) {
        this.form = e;
           this.visiable=true;
        this.$nextTick(() => {
          this.$refs.form.clearValidate();
        });
      }
    },
    save() {
      this.$refs.form.validate((r) => {
        if (r) {
          var url;
          if (this.form.Id == undefined && this.hasAddPermission) {
            url = this.$baseURL + "/paper/Add";
          } else if (this.form.Id && this.hasEditPermission) {
            url = this.$baseURL + "/paper/edit";
          }

          if (url) {
            this.$loading({ text: "处理中" });
            this.$axios
              .post(url, this.form)
              .then((res) => {
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
        }
      });
      //this.visiable = false;
    },
  },
};
</script>
