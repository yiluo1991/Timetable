<template>
  <ElDialog
    title="添加学期"
    :lock-scroll="false"
    :visible.sync="visiable"
    width="600px"
    :close-on-click-modal="false"
  >
    <ElForm :model="form" ref="form" label-width="80px" :rules="rules">
      <ElFormItem label="学年" prop="Year">
        <ElSelect
          v-model="form.Year"
          placeholder="请选择学年"
          style="width:100%"
        >
          <ElOption
            v-for="year in years"
            :label="year + '学年'"
            :value="year"
            :key="year"
          ></ElOption>
        </ElSelect>
      </ElFormItem>
      <ElFormItem label="学期" prop="Term">
        <ElSelect
          v-model="form.Term"
          placeholder="请选择学期"
          style="width:100%"
        >
          <ElOption label="第一学期" value="1"></ElOption>
          <ElOption label="第二学期" value="2"></ElOption>
        </ElSelect>
      </ElFormItem>
      <ElFormItem label="日期范围" prop="Range">
        <el-date-picker
          style="width:100%"
          v-model="form.Range"
          type="daterange"
          value-format="yyyy-MM-dd"
          :unlink-panels="true"
          range-separator="至"
          start-placeholder="开始日期"
          end-placeholder="结束日期"
        >
        </el-date-picker>
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
        Range: [],
        Year: "",
        Term: "1",
      },
      rules: {
        Year: [
          {
            required: true,
            message: "必填",
          },
        ],
        Term: [
          {
            required: true,
            message: "必填",
          },
        ],
        Range: [
             {
            required: true,
            message: "必填，请选择学期开始和结束日期",
          },
          {
            validator(fileds, value, callback) {
              if (value && value.length == 2) {
                callback();
              } else {
                callback(new Error("必填，请选择学期开始和结束日期"));
              }
            },
          },
        ],
      },
    };
  },
  mounted() {
    for (var i = 2021; i < 2100; i++) {
      this.years.push(i.toString());
    }
  },
  computed: {
    hasAddPermission() {
      return this.hasPermission("ST_A");
    },
  },
  methods: {
    showDialog() {
      if (this.hasAddPermission) {
        this.form.Range = [];
        this.form.Year = new Date().getFullYear().toString();
        this.form.Term = "1";
        this.visiable = true;
        this.$nextTick(() => {
          this.$refs.form.clearValidate();
        });
      }
    },
    save() {
      if (this.hasAddPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            var url = this.$baseURL + "/SchoolTerm/Add";
            this.$loading({ text: "处理中" });
            this.$axios
              .post(url, {
                CustomStart: this.form.Range[0],
                CustomEnd: this.form.Range[1],
                Year: this.form.Year,
                Term: this.form.Term,
              })
              .then((res) => {
                setTimeout(() => {
                  this.$loading().close();
                }, 100);
                this.$emit("reload");
                this.visiable=false;
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
