<template>
  <ElDialog
    :title="form.Id ? '修改' : '添加'"
    :lock-scroll="false"
    :visible.sync="visiable"
    width="600px"
    :close-on-click-modal="false"
  >
    <ElForm :model="form" ref="form" label-width="80px" :rules="rules">
      {{ form }}
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
      <ElFormItem label="入学年份" prop="Grade">
        <ElInput
          v-model="form.Grade"
          placeholder="请输入入学年份，例：“2020”"
        ></ElInput>
      </ElFormItem>
      <ElFormItem label="班级全名" prop="FullName">
        <ElInput
          v-model="form.FullName"
          placeholder="请输入班级全名，例：“20软件工程1班”"
        ></ElInput>
      </ElFormItem>

      <ElFormItem label="课程性质">
        <ElSelect style="width:100%" v-model="form.AdministrativeClassState">
          <ElOption
            v-for="(type, index) in props"
            :key="index"
            :label="type"
            :value="index"
          ></ElOption>
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

      props: ["在读", "毕业", "解散"],
      form: {},
      rules: {
        Name: [
          { required: true, message: "必填" },
          { max: 128, message: "最长64字符" },
        ],
        Grade: [
          { required: true, message: "必填" },
          {
            pattern: /^20\d{2}$/,
            message: "请输入4位数字年份，范围：2000~2099",
          },
        ],
      },
    };
  },
  mounted() {},
  computed: {
    hasAddPermission() {
      return this.hasPermission("AC_A");
    },
    hasComboPermission() {
      return this.hasPermission("AC_GCD");
    },
    hasEditPermission() {
      return this.hasPermission("AC_M");
    },
    hasCheckPermission() {
      return this.hasPermission("AC_C");
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
              Id:0,
              Grade:'',
              AdministrativeClassState:0,
              FullName: "",
              CollegeId: "",
              DepartmentId: "", 
              CD: "",
            };
            this.$nextTick(() => {
              this.$refs.form.clearValidate();
            });
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
      if (this.form.Id == 0 && this.hasAddPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var url = this.$baseURL + "/administrativeclass/Add";
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
            var url = this.$baseURL + "/administrativeclass/Edit";
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
