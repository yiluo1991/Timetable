<template>
  <ElDialog append-to-body title="参数值"  :lock-scroll="false" :visible.sync="dialogVisible" width="600px" close :close-on-click-modal="false">
    <div class="form-no-margin">
      <ElForm ref="form" inline size="small" :show-message="false" inline-message :model="editData">
        <ElCard style="margin:10px 0;" shadow="hover" v-for="(mapRule, index) in editData.data" :key="index">
          <div style="margin-bottom:10px;font-weight:bolder">
            <span style="font-weight:normal">当下面条件都满足时，评为</span> {{ mapRule.desc }}<el-divider direction="vertical"></el-divider><span style="color:#aaa"> {{ mapRule.value }} </span>
          </div>
          <div v-for="(rule, i) in mapRule.rules" :key="i">
            <ElFormItem>
              得分
              <ElSelect style="width:60px" v-model="rule.op">
                <ElOption v-for="o in ops" :key="o.v" :label="o.t" :value="o.v"></ElOption>
              </ElSelect>
            </ElFormItem>
            <ElFormItem :prop="'data[' + index + '].rules[' + i + '].num'" :rules="[{ required: true, message: '必填' }]">
              <ElInputNumber style="margin-left:3px" v-model="rule.num" :controls="false" placeholder="请输入数值" :max="999999999999999" :min="-999999999999999"></ElInputNumber>
            </ElFormItem>
            <ElButton type="text" size="small" icon="el-icon-delete" v-if="mapRule.rules.length > 1" @click="del(mapRule.rules, i)"></ElButton>
            <ElButton type="text" v-else size="small" icon="el-icon-circle-plus-outline" @click="add(mapRule.rules)"></ElButton>
          </div>
        </ElCard>
      </ElForm>
    </div>
    <span slot="footer" class="dialog-footer">
      <ElButton @click="dialogVisible = false" icon="el-icon-close">取 消</ElButton>
      <ElButton type="primary" @click="save()" icon="el-icon-check">确 定</ElButton>
    </span>
  </ElDialog>
</template>
<script>
export default {
  data() {
    return {
      ops: [
        { t: "=", v: "eq" },
        { t: "≠", v: "neq" },
        { t: "<", v: "lt" },
        { t: ">", v: "gt" },
        { t: "≤", v: "lte" },
        { t: "≥", v: "gte" },
      ],
      editData: {},
      index:undefined,
      dialogVisible: false,
      rules: {},

    };
  },
  methods: {
    showDialog(mapRules,index) {
       this.index=index;
      this.editData = { data: JSON.parse(JSON.stringify(mapRules)) };
      this.dialogVisible = true;
    },
    add(rules) {
      rules.push({
        op: "eq",
        num: 0,
      });
    },
    del(rules, index) {
      rules.splice(index, 1);
    },
    save() {
      this.$refs["form"].validate((result) => {
        if (result) {
            this.$emit('update',this.editData.data,this.index);
            this.dialogVisible=false;
        }
      });
    },
  },
};
</script>
<style>
.form-no-margin .el-form--inline .el-form-item {
  margin: 0 !important;
}
</style>
