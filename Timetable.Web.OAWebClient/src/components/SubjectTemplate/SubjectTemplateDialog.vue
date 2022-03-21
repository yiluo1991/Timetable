<template>
  <ElDialog :visible.sync="dialogVisible" width="1260px" height="1000px" top="5vh" title="问卷审核信息" :close-on-click-modal="false">
    <div style="overflow: hidden;" v-if="paper">
      <div style="float:left;width:880px;height:75vh">
        <ViewPaper :data="paper" class="desktop"></ViewPaper>
      </div>
      <div style="float:left;width:330px;height:75vh;margin-left:10px;overflow: auto;">
        <ElForm ref="v" :model="paper" size="mini">
          <ElFormItem
            label="模板名（可重命名）："
            prop="TemplateName"
            :rules="[
              { required: true, message: '模板名必填' },
              { max: 128, message: '模板名最长128字符' },
            ]"
          >
            <ElInput :readonly="!editPermission" v-model="paper.TemplateName" placeholder="请输入模板名"></ElInput>
          </ElFormItem>
          <p style="line-height:14px;margin-bottom:8px;">关键词标签（可调整）</p>
          <div class="templatetags">
            <el-autocomplete
              :readonly="!editPermission"
              @select="handleSelect"
              size="mini"
              style="width:100%"
              :fetch-suggestions="querySearch"
              :trigger-on-focus="false"
              v-model="Tag"
              placeholder="请输入要添加的关键词标签"
            >
              <ElButton v-if="editPermission" slot="append" icon="el-icon-plus" @click="addTag">添加</ElButton>
            </el-autocomplete>
            <ElFormItem prop="CategoryKeywords" :rules="[{ max: 256, message: '标签过多，请删除不需要的标签' }]">
              <div class="templatetabsbox clearfix" v-if="Tags.length > 0">
                <span class="item" v-for="tag in Tags" :key="tag"
                  >{{ tag }}

                  <ElTooltip v-if="quickAddPermission && paper.CanAdd.indexOf(tag) > -1" content="快速添加到全局关键词" placement="bottom-start">
                    <i @click="quickAdd(tag)" style="display:inline-block" class="el-icon-refresh"></i>
                  </ElTooltip>
                  <ElTooltip content="移除标签" v-if="editPermission" placement="bottom-start">
                    <i @click="removeTag(tag)" class="el-icon-close"></i>
                  </ElTooltip>
                </span>
              </div>
            </ElFormItem>
          </div>

          <ElFormItem label="申请附言：" style="margin-bottom:0;">
            <ElInput :readonly="true" type="textarea" resize="none" rows="5" v-model="paper.Apply" placeholder="无附言内容"></ElInput>
          </ElFormItem>
          <ElFormItem label="审核反馈信息：" style="margin-bottom:5px;" prop="DenyFeedback" :rules="[{ max: 1024, message: '反馈信息最长1024个字符' }]">
            <ElInput :readonly="!editPermission" type="textarea" resize="none" rows="5" v-model="paper.DenyFeedback" placeholder="请输入审核信息" maxlength="1024" show-word-limit></ElInput>
          </ElFormItem>
          <ElFormItem label="审核状态：" style="margin-bottom:5px;">
            <el-radio-group v-model="paper.SubjectTemplateStatus" v-if="editPermission">
              <el-radio-button :label="0">待审核</el-radio-button>
              <el-radio-button :label="1">审核通过</el-radio-button>
              <el-radio-button :label="2">不通过</el-radio-button>
            </el-radio-group>

            <ElTag v-else :type="colors[paper.SubjectTemplateStatus]" size="small">
              {{ paper.SubjectTemplateStatus | state }}
            </ElTag>
            <div v-if="!editPermission" style="margin-left:15px;display:inline-block;">
              状态：
              <ElTag :type="paper.Enable ? 'success' : 'danger'" size="small">
                {{ paper.Enable ? "可用" : "禁用" }}
              </ElTag>
            </div>
          </ElFormItem>
          <template v-if="editPermission">
            <ElFormItem label="排序号：" style="width:50%; float:left;" prop="SortNum" :rules="[{ required: true, message: '必填' }]">
              <ElInputNumber :min="0" :disabled="!editPermission" style="max-width:100%" :max="2000000000" :controls="false" v-model="paper.SortNum" placeholder="排序号"></ElInputNumber>
            </ElFormItem>
            <ElFormItem label="热度：" style="width:50%; float:left;" prop="Useage" :rules="[{ required: true, message: '必填' }]">
              <ElInputNumber :min="0" :disabled="!editPermission" style="max-width:100%" :max="2000000000" :controls="false" v-model="paper.Useage" placeholder="热度"></ElInputNumber>
            </ElFormItem>
            <ElSwitch v-model="paper.Enable" active-text="可用" inactive-text="禁用" inactive-color="gray"></ElSwitch>
          </template>
          <template v-else>
            <div style="margin:10px 0">
              <div style="display:inline-block;width:50%">排序号：{{ paper.SortNum }}</div>
              <div style="display:inline-block;width:50%">热度：{{ paper.Useage }}</div>
            </div>
          </template>
        </ElForm>
        <div v-if="editPermission" style="margin-top:15px;text-align:center;">
          <ElButton @click="save" type="primary" icon="el-icon-check">保存</ElButton>
        </div>
      </div>
    </div>
  </ElDialog>
</template>
<script>
import ViewPaper from "../Paper/ViewPaper";
export default {
  components: {
    ViewPaper,
  },
  filters: {
    state(v) {
      switch (v) {
        case 0:
          return "待审核";
        case 1:
          return "审核通过";
        case 2:
          return "不通过";
      }
    },
  },
  inject: ["hasPermission"],
  computed: {
    editPermission() {
      return this.hasPermission("TP_M");
    },
    quickAddPermission() {
      return this.hasPermission("TP_QA");
    },
    VerifyPermission() {
      return this.hasPermission("TP_V");
    },
    Tags: function() {
      return this.paper.CategoryKeywords == undefined || this.paper.CategoryKeywords == "" ? [] : this.paper.CategoryKeywords.split("‖");
    },
  },
  data() {
    return {
      paper: {},
      colors: ["warning", "success", "danger"],
      dialogVisible: false,
      Tag: "",
    };
  },
  watch: {
    "paper.TemplateName": function(value) {
      this.paper.basicInfo.Name_CN = value;
    },
  },
  methods: {
    showDialog(data) {
      this.paper = data;
      this.Tag="";
      
      this.dialogVisible = true;
    },
    save() {
      if (!this.editPermission) return;
      this.$refs.v.validate((r) => {
        if (r) {
            var data={
                Id:this.paper.Id,
                CategoryKeywords:this.paper.CategoryKeywords,
                TemplateName:this.paper.TemplateName,
                DenyFeedback:this.paper.DenyFeedback,
                SubjectTemplateStatus:this.paper.SubjectTemplateStatus,
                SortNum:this.paper.SortNum,
                Useage:this.paper.Useage,
                Enable:this.paper.Enable
            };
          this.$axios
            .post(
              this.$baseURL + "/template/edit",
              data
            )
            .then((res) => {
              this.dialogVisible = false;
              this.$emit("reload");
              if (!res.data.success) {
                this.$message({
                  message: res.data.msg,
                  type: "error",
                  offset: 80,
                });
              }
            });
        }
      });
    },
    // audit(result) {
    //   if (!this.auditPermission) return;
    //   this.$refs.v.validate((r) => {
    //     if (r) {
    //       this.$axios
    //         .post(
    //           this.$baseURL + "/template/audit",
    //           `id=${encodeURIComponent(this.paper.id)}&result=${encodeURIComponent(result)}&denyfeedback=${this.paper.DenyFeedback ? encodeURIComponent(this.paper.DenyFeedback) : ""}`
    //         )
    //         .then((res) => {
    //           this.dialogVisible = false;
    //           this.$emit("reload");
    //           if (!res.data.success) {
    //             this.$message({
    //               message: res.data.msg,
    //               type: "error",
    //               offset: 80,
    //             });
    //           }
    //         });
    //     }
    //   });
    // },
    handleSelect() {
      this.addTag();
    },
    validateTags() {
      this.$refs.v.fields[1].validate();
    },
    removeTag(tag) {
      this.paper.CategoryKeywords = this.Tags.filter((s) => s != tag).join("‖");
      var i = this.paper.CanAdd.indexOf(tag);
      if (i > -1) {
        this.paper.CanAdd.splice(i, 1);
      }
      this.validateTags();
    },
    addTag() {
      if (this.Tag.trim() !== "") {
        var tag = this.Tag.trim();
        if (tag.length <= 32) {
          if (this.Tags.indexOf(tag) == -1) {
            if (this.paper.CategoryKeywords !== "") {
              this.paper.CategoryKeywords += "‖";
            }
            this.paper.CategoryKeywords += tag;
            this.Tag = "";
            this.validateTags();
            if (this.VerifyPermission) {
              this.$axios.post(this.$baseURL + "/tag/hastag", "tag=" + encodeURIComponent(tag)).then((res) => {
                if (res.data == false) {
                  var i = this.paper.CanAdd.indexOf(tag);
                  if (i == -1) {
                    this.paper.CanAdd.push(tag);
                  }
                }
              });
            }
          } else {
            this.$message({
              message: "标签已存在",
            });
          }
        } else {
          this.$message({
            message: "每个标签长度不可超过32字符",
          });
        }
      }
    },
    quickAdd(tag) {
      if (this.quickAddPermission) {
        this.$axios.post(this.$baseURL + "/tag/quickadd", "tag=" + encodeURIComponent(tag)).then((res) => {
          var i = this.paper.CanAdd.indexOf(tag);
          if (i > -1) {
            this.paper.CanAdd.splice(i, 1);
          }
        });
      }
    },
    querySearch(queryString, cb) {
      if (queryString === "") {
        cb([]);
      } else {
        this.$axios.post(this.$baseURL + "/tag/suggest", "keyword=" + encodeURIComponent(queryString)).then((res) => {
          cb(res.data.data);
        });
      }
    },
  },
};
</script>

<style lang="less">
.templatetags {
  .templatetabsbox {
    margin-top: 5px;
    border: 1px solid #ddd;
    border-radius: 5px;
    padding: 0px 5px 5px 5px;
    .item {
      float: left;
      font-size: 12px;
      margin-top: 5px;
      padding: 5px 10px;
      background: rgb(221, 239, 247);
      border-radius: 5px;
      border: 1px solid darken(rgb(221, 239, 247), 10);
      line-height: 15px;
      margin-right: 5px;
      cursor: default;
      color: rgb(56, 148, 187);
      i {
        cursor: pointer;
      }
      &:hover {
        background: darken(rgb(221, 239, 247), 5);
        border: 1px solid darken(rgb(221, 239, 247), 15);
        i {
          display: inline-block;
          color: lighten(rgb(56, 148, 187), 15);
          &:hover {
            color: darken(rgb(56, 148, 187), 10);
          }
        }
      }
    }
  }
}
.el-form-item.is-error {
  .templatetabsbox {
    border: 1px solid lighten(red, 20);
  }
}
</style>
