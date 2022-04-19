<template>
  <ElDialog
    :title="form.Id ? '修改' : '添加'"
    :lock-scroll="false"
    :visible.sync="visiable"
    width="600px"
    :close-on-click-modal="false"
  >
    <ElForm :model="form" ref="form" label-width="80px" :rules="rules">
       
      <ElFormItem label="所属学院" prop="Year"     v-if="mode!==0">
        <ElSelect
     
          v-model="form.CollegeId"
          placeholder="请选择学院"
          style="width:100%"
        >
          <ElOption
            v-for="c in comboData"
            :label="c.Name"
            :value="c.Id"
            :key="c.Id"
          ></ElOption>
        </ElSelect>
      </ElFormItem>

      <ElFormItem
        :label="form.CollegeId || mode == 1 ? '系/专业' : '院/系'"
        prop="Name"
      >
        <ElInput
          v-model="form.Name"
          :placeholder="
            '请输入' + (form.CollegeId || mode == 1 ? '系/专业名' : '院/系名')
          "
        ></ElInput>
      </ElFormItem>
      <ElFormItem label="联系人" prop="ContactName">
        <ElInput
          v-model="form.ContactName"
          placeholder="请输入联系人"
        ></ElInput>
      </ElFormItem>
      <ElFormItem label="联系电话" prop="ContactPhone">
        <ElInput
          v-model="form.ContactPhone"
          placeholder="请输入联系电话"
        ></ElInput>
      </ElFormItem>
      <ElFormItem label="状态" prop="Enable">
        <ElSelect v-model="form.Enable">
          <ElOption label="启用" :value="true"></ElOption>
          <ElOption label="禁用" :value="false"></ElOption>
        </ElSelect>
      </ElFormItem>
      <ElFormItem label="备注" prop="Remark">
        <ElInput v-model="form.Remark" placeholder="请输入"></ElInput>
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
      mode: undefined,
      form: {},
      rules: {
        Name: [
          { required: true, message: "必填" },
          { max: 128, message: "最长128字符" },
        ],
        Remark: [{ max: 256, message: "最长256字符" }],
        ContactName: [{ max: 64, message: "最长64字符" }],
        ContactPhone: [
          { max: 32, message: "最长32字符" },
          {
            pattern: /^\d{0,32}$/,
            message: "请输入号码（纯数字）",
          },
        ],
      },
    };
  },
  mounted() {},
  computed: {
    hasAddPermission() {
      return this.hasPermission("CD_A");
    },
    hasComboPermission() {
      return this.hasPermission("CD_GC");
    },
    hasEditPermission() {
      return this.hasPermission("CD_M");
    },
  },
  methods: {
    loadCombo(add) {
      if (this.hasComboPermission) {
        var url = this.$baseURL + "/CollegeDepartment/CollegeCombo";
        this.$axios.post(url).then((res) => {
          if (res.data.success) {
            if (add) {
              this.comboData = [
                { Id: null, Name: "无（添加新学院）" },
                ...res.data.data,
              ];
            } else {
              this.comboData = res.data.data;
            }
          }
        });
      }
    },
    showAddDialog() {
      if (this.hasAddPermission) {
        this.mode = undefined;
        this.form = {
          Name: "",
          Remark: "",
          ContactName: "",
          ContactPhone: "",
          Enable: true,
          CollegeId: null,
        };
        this.loadCombo(true);
        this.visiable = true;
        this.$nextTick(() => {
          this.$refs.form.clearValidate();
        });
      }
    },
    showEditDialog(row) {
      this.form = row;
      if (row.CollegeId) {
        this.mode = 1;
        this.loadCombo(false);
      } else {
        this.mode = 0;
      }
      this.visiable = true;
      this.$nextTick(() => {
        this.$refs.form.clearValidate();
      });
    },
    save() {
      
      if ((this.form.Id == undefined) && this.hasAddPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var url = this.$baseURL + "/CollegeDepartment/Add";
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
      } else if (this.form.Id && this.hasEditPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var url = this.$baseURL + "/CollegeDepartment/Edit";
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
