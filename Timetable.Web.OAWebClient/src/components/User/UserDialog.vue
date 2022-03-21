<template>
  <ElDialog top="60px" :title="dialogTitle" :lock-scroll="true" :visible.sync="dialogVisible" width="600px"  :close-on-click-modal="false">
    <div>
      <ElForm ref="form" label-width="140px" @submit.native.prevent :model="editData" :rules="rules">
        <ElFormItem label="手机号（登录名）" prop="Mobile">
          <ElInput v-model="editData.Mobile" placeholder="（必填）请填写手机号，手机号同时是登录名" :disabled="disabled"></ElInput>
        </ElFormItem>
         <ElFormItem label="真实姓名" prop="RealName">
          <ElInput v-model="editData.RealName" placeholder="（必填）请填写真实姓名"></ElInput>
        </ElFormItem>
         <ElFormItem label="性别" prop="Sex">
          <ElSelect v-model="editData.Sex" style="width:100%" placeholder="（必选）请选择性别">
            <ElOption label="男" :value="0"></ElOption>
            <ElOption label="女" :value="1"></ElOption>
          </ElSelect>
        </ElFormItem>
         <ElFormItem label="身份证号" prop="IDCard">
          <ElInput v-model="editData.IDCard" placeholder="（选填）身份证号"></ElInput>
        </ElFormItem>
        <ElFormItem label="账号所属机构" prop="OrganizationName">  
          <ElInput v-model="editData.OrganizationName" placeholder="（选填）请填写账号所属公司或组织机构名称"></ElInput>
          <div v-if="hasSchoolPermission" style="vertical-align: middle;font-size:12px; margin-top:4px;line-height:16px;color:#999; "> 
            若机构为院校,可<ElButton   type="text" size="mini" style="padding:0;" @click="showSCDialog">选取院校</ElButton>
          </div>
          <DesignTypeSchoolDialog  v-if="initDialog" @destory="initDialog=false"  ref="scdialog" v-model="editData.OrganizationName"></DesignTypeSchoolDialog>
        </ElFormItem>
        <ElFormItem label="机构代码" prop="OrganizationCode">
          <ElInput v-model="editData.OrganizationCode" placeholder="（选填）请填写机构代码（一般填写18位统一社会信用代码）"></ElInput>
        </ElFormItem> 
        <ElFormItem label="账号昵称" prop="NickName">
          <ElInput v-model="editData.NickName" placeholder="（选填）请填写昵称"></ElInput>
        </ElFormItem> 
        <ElFormItem label="所在地" prop="City">
          <ElChinaCity placeholder="请选择" style="width:100%" :model="$data" provinceField="Province" cityField="City" :clearable="false" questionId="editData"></ElChinaCity>
        </ElFormItem>

        <ElFormItem label="邮箱" prop="Email">
          <ElInput v-model="editData.Email" placeholder="（选填）可填写邮箱"></ElInput>
        </ElFormItem>
         <ElFormItem label="用户组" prop="UserGroupId">
           <ElSelect  style="width:100%;" v-model="editData.UserGroupId" placeholder="请选择">
          <ElOption
            v-for="item in treeData"
            :key="item.Id"
            :label="item.Name"
            :value="item.Id">
            <span style="float: left">{{ item.Name }}</span>
            <span style="float: right; color: #8492a6; font-size: 13px">{{ item.PermissionLevel }}</span>
          </ElOption>
        </ElSelect>
        </ElFormItem>
       
        <ElFormItem label="状态" prop="Status">
          <el-select v-model="editData.Status" style="width:100%;" filterable placeholder="请选择是否启用">
            <el-option label="启用" :value="0"></el-option>
            <el-option label="禁用" :value="4"></el-option>
          </el-select>
        </ElFormItem>
      </ElForm>
    </div>
    <span slot="footer" class="dialog-footer">
      <ElButton @click="dialogVisible = false" icon="el-icon-close">取 消</ElButton>
      <ElButton type="primary" @click="save()" icon="el-icon-check">确 定</ElButton>
    </span>
  </ElDialog>
</template>
<script>
import DesignTypeSchoolDialog from '../Paper/DesignTypeSchoolDialog'
import ElChinaCity from "../Common/ElementUI/ElChinaCity";
export default {
  components: {
    ElChinaCity,
    DesignTypeSchoolDialog
  },
  inject: ["hasPermission"],
  computed: {
    hasCheckPermission() {
      return this.hasPermission("U_V");
    },

    hasAddPermission() {
      return this.hasPermission("U_A");
    },
    hasEditPermission() {
      return this.hasPermission("U_M");
    },
    hasSchoolPermission(){
      return this.hasPermission("U_GSC")
    }
  },
  data() {
    return {
       initDialog:false,
      editData: {},
      disabled: false,
      dialogTitle: "修改",
      dialogVisible: false,
      rules: {
        Mobile: [
          { required: true, message: "手机号（登录名）必填" },
          { pattern: /^1\d{10}$/, message: "手机号格式有误" },
          {
            validator: (rule, value, callback) => {
              if (this.disabled) {
                callback();
              } else {
                if (!this.hasCheckPermission) {
                  callback("您的权限不足以验证用户名是否重复");
                } else {
                  var url = "/user/CheckMobile";
                  this.$axios.post(this.$baseURL + url, "mobile=" + encodeURIComponent(value)).then((res) => {
                    if (res.data) {
                      callback();
                    } else {
                      callback(new Error("用户名已存在"));
                    }
                  });
                }
              }
            },
          },
        ],
        IDCard:[
            {
              pattern:/^(\d{18,18}|\d{17,17}(X|x))$/,message:'身份证号码格式有误'
            }
        ],
        OrganizationCode:[
            {
              max:32,message:'机构代码最长32字符'
            }
        ],
         OrganizationName:[
            {
              max:64,message:'组织机构名最长64字符'
            }
        ],
        Sex: [{ required: true, message: "姓名必选" }],
        RealName: [
          { required: true, message: "必须填写用户姓名" },
          { min: 1, max: 32, message: "姓名长度为1~32字符" },
        ],
        NickName: [{ max: 32, message: "昵称最大长度32字符" }],
        Email: [
          { type: "email", message: "邮箱格式有误" },
          { max: 128, message: "邮箱最大长度在128位" },
        ],
        City:[
           { required: true, message: "所在地必填" }
        ]
        
      },
      treeData: []
    };
  },

  methods: {
    getSelect: function(){
      var url = "/usergroup/list";
      if (url) {
        this.$axios
          .post(this.$baseURL + url)
          .then((res) => {
            this.treeData = res.data.data;
          });
      }
    },
       showSCDialog() {
   
      this.initDialog=true;
      this.$nextTick(()=>{ 
        this.$refs.scdialog.showDialog();
      })
     
    },
    showAddDialog: function() {
      if(!this.hasAddPermission)return;
      this.dialogTitle = "添加调查用户";
      this.dialogVisible = true;
      this.disabled = false;
      this.editData = {
        City: "福建省/厦门市",
        Province: "福建省",
        Sex: 0,
        RealName: "",
        Mobile: "",
        NickName: "",
        Email: "",
        Status: 0,
        Id: "00000000-0000-0000-0000-000000000000",
      };
      this.$nextTick(() => {
        this.$refs.form.clearValidate();
      });
    },
    showEditDialog: function(row) {
      if(!this.hasEditPermission)return;
      this.dialogTitle = "修改调查用户";
      this.dialogVisible = true;
      this.disabled = true;
      this.editData = JSON.parse(JSON.stringify(row));
      this.$nextTick(() => {
        this.$refs.form.clearValidate();
      });
    },
    save: function() {
      this.$refs.form.validate((r) => {
        if (r) {
          var mode = "add";
          if (this.editData.Id != "00000000-0000-0000-0000-000000000000") {
            mode = "edit";
          }
          if ((mode == "add" && !this.hasAddPermission) || (mode == "edit" && !this.hasEditPermission)) {
            return;
          }
          var loading = this.$loading({
            text: "数据处理中",
          });

          var url = mode == "add" ? "/user/add" : "/user/edit";
          this.$axios.post(this.$baseURL + url, this.editData).then((res) => {
            this.dialogVisible = false;
            if (mode == "add") {
              this.$alert(res.data.msg, "添加成功", {
                confirmButtonText: "确定",
              });
            }
            setTimeout(() => {
              loading.close();
              this.$emit("reload");
            }, 0);
          });
        }
      });
    },
  },
  mounted(){
    this.getSelect()
  }
};
</script>
