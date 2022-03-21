<template>
  <div>
    <ElDialog ref="dialog" title="自动运算" append-to-body top="50px" :lock-scroll="false" :visible.sync="dialogVisible" width="1000px" close :close-on-click-modal="false">
      <div>
        <el-container>
          <el-header >
            <ElForm   ref="form" inline hide-required-asterisk size="small" :model="formData" inline-message>
              <ElFormItem label="" prop="acceptMinAnswersCount" :rules="[{ required: true, message: '必填' }]">
                答卷阈值<el-tooltip class="item" effect="dark" content="设置达到多少份答卷的调查才能参与本运算。" placement="top-start"> <i class="el-icon-question"></i> </el-tooltip>：
                <ElInputNumber style="width:80px" v-model="formData.acceptMinAnswersCount" placeholder="" :controls="false"></ElInputNumber>
              </ElFormItem>
              <ElFormItem>
                日期范围<el-tooltip class="item" effect="dark" content="如果设置了日期区间，调查的创建时间在设定的区间内，才参与本运算。" placement="top-start">
                  <i class="el-icon-question"></i> </el-tooltip
                >：
                <ElDatePicker
                  v-model="formData.dataRange"
                  type="daterange"
                  align="right"
                  unlink-panels
                  range-separator="至"
                  start-placeholder="开始日期"
                  end-placeholder="结束日期"
                  :picker-options="pickerOptions"
                  value-format="yyyy-MM-dd"
                  style="margin-right:10px;width:240px;vertical-align: top;margin-left:10px;"
                >
                </ElDatePicker>
              </ElFormItem>
              <ElFormItem>
                <ElButton type="primary" @click="autoSummary">运算</ElButton>
              </ElFormItem>
            </ElForm>
          </el-header>
          <el-main height="">
            <!-- Main content -->
            <div v-if="editData && editData.updateMetas && editData.updateMetas.length == 0" style="padding:100px;text-align:center;color:#999">
              没有可更新数据，请进行运算。
            </div>
            <div v-else class="form-no-margin">
              <ElForm size="mini" inline inline-message :show-message="false" hide-required-asterisk ref="editform" @submit.native.prevent :model="editData">
                <ElTable  size="mini" :data="editData.updateMetas">
                  <ElTableColumn show-overflow-tooltip label="参数标识名" width="150" prop="metaName"></ElTableColumn>
                  <!-- <ElTableColumn show-overflow-tooltip label="参数类型" width="150">
                    <template v-slot="scope">
                      {{ scope.row.type == "areaMap" ? "评分标准" : scope.row.type == "number" ? "数字" : "字符串" }}
                    </template>
                  </ElTableColumn> -->
                  <ElTableColumn show-overflow-tooltip label="参数说明" prop="desc"></ElTableColumn>
                  <ElTableColumn show-overflow-tooltip label="当前值" width="200">
                    <template v-slot="scope">
                      {{ scope.row.type == "number" ? scope.row.num : scope.row.str }}
                    </template>
                  </ElTableColumn>

                  <ElTableColumn show-overflow-tooltip label="运算得到的新值" width="200">
                    <template v-slot="scope">
                      <ElFormItem v-if="scope.row.type == 'number'" :rules="[{ required: true }]" :prop="'updateMetas[' + scope.$index + '].newValue'">
                        <ElInputNumber
                          style="width:100%"
                          :controls="false"
                          :max="999999999999999"
                          :min="-999999999999999"
                          v-model="editData.updateMetas[scope.$index].newValue"
                          placeholder="请输入数值"
                        ></ElInputNumber>
                      </ElFormItem>
                      <ElFormItem v-else-if="scope.row.type == 'string'" :rules="[{ required: true }, { max: 128 }]" :prop="'updateMetas[' + scope.$index + '].newValue'">
                        <ElInput maxlength="128" :rules="[{ required: true }]" style="width:100%" v-model="editData.updateMetas[scope.$index].newValue" placeholder="请输入文字"></ElInput>
                      </ElFormItem>
                    </template>
                  </ElTableColumn>
                  <ElTableColumn show-overflow-tooltip label="" width="30">
                    <template v-slot="scope">
                      <i style="color:orange" v-if="(scope.row.type == 'number' ? scope.row.num : scope.row.str) != scope.row.newValue" class="el-icon-warning-outline"></i>
                    </template>
                  </ElTableColumn>
                </ElTable>
              </ElForm>
              
            </div>
          </el-main>
        </el-container>
      </div>
       <span slot="footer" class="dialog-footer">
       <ElButton size="medium" type="primary" @click="ok" v-if="editData.updateMetas.length>0">确认并将修改载入到参数调整界面</ElButton>
                <ElButton size="medium" type="default" @click="dialogVisible = false">关闭</ElButton>
       </span>
    </ElDialog>
  </div>
</template>
<script>
export default {
  props: ["item"],
  data() {
    return {
      metas: [],
      serverMetas: [],
      editData: { updateMetas: [] },
      dialogVisible: false,
      pickerOptions: {
        shortcuts: [
          {
            text: "最近一周",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 7);
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "最近一个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 30);
              picker.$emit("pick", [start, end]);
            },
          },
          {
            text: "最近三个月",
            onClick(picker) {
              const end = new Date();
              const start = new Date();
              start.setTime(start.getTime() - 3600 * 1000 * 24 * 90);
              picker.$emit("pick", [start, end]);
            },
          },
        ],
      },
      formData: {
        dataRange: null,
        acceptMinAnswersCount: 1,
      },
    };
  },
  methods: {
    show(metas) {
      this.metas = JSON.parse(JSON.stringify(metas));
      this.serverMetas = [];
      this.editData = { updateMetas: [] };
      this.dialogVisible = true;
      this.formData = {
        dataRange: null,
        acceptMinAnswersCount: 1,
      };
    },
    autoSummary() {
      this.$refs.form.validate((ok) => {
        if (ok) {
          var lod = this.$loading({ text: "运算中..." });
          this.serverMetas = [];
          (this.editData = { updateMetas: [] }),
            this.$axios
              .post(
                `/api/proreport/autosummary?groupItemId=${this.item.Id}&rangeStart=${this.formData.dataRange ? this.formData.dataRange[0] : ""}&rangeEnd=${
                  this.formData.dataRange ? this.formData.dataRange[1] : ""
                }&acceptMinAnswersCount=${this.formData.acceptMinAnswersCount}`
              )
              .then((res) => {
                lod.close();
                console.log(res.data);
                if (res.data.success) {
                  this.serverMetas = res.data.data.Mates;
                  this.createUpdateMetas();
                } else {
                  this.$message({
                    type: "warning",
                    message: res.data.msg,
                    offset: 80,
                  });
                }
              });
        }
      });
    },
    createUpdateMetas() {
      var editData = {};
      editData.updateMetas = [];
      this.metas.forEach((meta) => {
        var serverMeta = this.serverMetas.find((s) => s.metaName == meta.metaName);
        if (serverMeta) {
          editData.updateMetas.push({ ...meta, newValue: serverMeta.type == "string" ? serverMeta.str : serverMeta.num });
        }
      });
      this.editData = editData;
    },
    ok() {
      this.$refs.editform.validate((ok) => {
        if (ok) {
            var copy=JSON.parse(JSON.stringify(this.editData.updateMetas));
            copy.forEach(s=>{
                var v=s.newValue;
                if(s.type=="string"){
                    s.str=v;
                }else if(s.type=="number"){
                    s.num=v;
                }
                delete(s.newValue)
            })
           this.$emit('change',copy);
           this.dialogVisible=false;
        }
      });
    },
  },
};
</script>
