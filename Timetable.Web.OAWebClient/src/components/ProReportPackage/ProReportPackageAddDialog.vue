<template>
  <ElDialog
    title="添加归档任务"
    :visible.sync="visiable"
    width="800px"
    :close-on-click-modal="false"
  >
    <div style="margin-top:-15px;">
      <el-steps
        finish-status="success"
        process-status="finish"
        :active="selectedTemplate == undefined ? 0 : 1"
        align-center
      >
        <el-step title="选择归档的问卷模板"></el-step>
        <el-step title="选择专业报告模板"></el-step>
      </el-steps>
    </div>
    <!-- STEP1 -->
    <div v-if="!selectedTemplate" style="margin-top:10px">
      <div
        style="padding:5px;border:1px solid #eee;border-bottom:none;background:#fafafa"
      >
        <ElInput
          size="small"
          style="width:400px"
          v-model="keyword"
          @keydown.13.native="search"
          placeholder="请输入要检索的模板信息"
        >
          <template slot="prepend">搜索</template>
          <ElButton
            slot="append"
            icon="el-icon-search"
            @click="search()"
          ></ElButton>
        </ElInput>
      </div>
      <div style="border:1px solid #eee;border-bottom:none;">
        <ElTable
          :data="data"
          size="small"
          height="360px"
          highlight-current-row
          v-loading="loading"
        >
          <ElTableColumn
            label="问卷模板ID"
            prop="Id"
            width="120px"
          ></ElTableColumn>
          <ElTableColumn
            label="模板名"
            prop="TemplateName"
            show-overflow-tooltip
          ></ElTableColumn>
          <ElTableColumn
            label="模板关键词"
            prop="CategoryKeywords"
            show-overflow-tooltip
          ></ElTableColumn>
          <ElTableColumn label="操作" prop="A" width="80px">
            <template v-slot="{ row }">
              <ElButton
                type="primary"
                round
                size="mini"
                plain
                @click="pick(row)"
                >选择</ElButton
              >
            </template>
          </ElTableColumn>
        </ElTable>
      </div>
      <ElPagination
        style="margin-top:10px;"
        :current-page.sync="searchOption.page"
        :page-sizes="[1, 10, 15, 20, 25, 30]"
        :page-size.sync="searchOption.rows"
        background
        layout="total,sizes, prev, pager, next,jumper,slot"
        @current-change="reload()"
        @size-change="reload()"
        :total="total"
      >
      </ElPagination>
    </div>
    <!-- STEP2 -->
    <div v-else style="margin-top:40px">
      <ElForm ref="form" label-width="110px" :model="form">
        <ElFormItem
          label="报告模板"
          prop="groupItem"
          :rules="[{ required: true, message: '请选择报告模板' }]"
        >
          <ElSelect
            popper-class="oxx"
            v-model="form.groupItem"
            style="width:100%;"
            placeholder="请选择一套专业报告模板"
          >
            <ElOption
              v-for="item in groupItems"
              :key="item.Id"
              :label="item.Name"
              :value="item.Id"
            ></ElOption>
          </ElSelect>
        </ElFormItem>
        <ElFormItem
          label="归档名称"
          prop="name"
          :rules="[
            { required: true, message: '请填写归档名称' },
            { max: 128, min: 1, message: '归档名称1~128个字符' },
          ]"
        >
          <ElInput
            v-model="form.name"
            placeholder="请为归档设定一个名称"
          ></ElInput>
        </ElFormItem>
      </ElForm>
    </div>
    <span slot="footer" class="dialog-footer">
      <ElButton
        @click="visiable = false"
        v-if="selectedTemplate == undefined"
        icon="el-icon-close"
        >取 消</ElButton
      >
      <ElButton @click="redo" v-else icon="el-icon-arrow-left"
        >返回重选问卷模板</ElButton
      >
      <ElButton
        v-if="hasAddPermission"
        type="primary"
        @click="add()"
        :disabled="!selectedTemplate || !form.groupItem || !form.name"
        icon="el-icon-check"
        >创建归档任务</ElButton
      >
    </span>
  </ElDialog>
</template>
<script>
export default {
  inject: ["hasPermission"],
  computed: {
    hasGetProTemplatePermission() {
      return this.hasPermission("PRP_TG");
    },
    hasGetGroupItemPermission() {
      return this.hasPermission("PRP_GG");
    },
    hasAddPermission() {
      return this.hasPermission("PRP_A");
    },
  },
  data() {
    return {
      visiable: false,
      data: [],
      keyword: "",

      selectedTemplate: undefined,
      total: 0,
      loading: true,
      groupItems: [],
      form: {
        name: "",
        groupItem: undefined,
      },

      searchOption: {
        page: 1,
        rows: 10,
        keyword: "",
      },
    };
  },
  methods: {
    openDialog() {
      this.visiable = true;
      this.reload();
      this.keyword = "";
      this.searchOption.page = 1;
      this.searchOption.rows = 10;
      this.searchOption.keyword = "";
      this.selectedTemplate = undefined;
      this.groupItems = []; 
      this.form.groupItem = undefined;
      this.form.name='';
    },
    search() {
        this.reload(this.keyword)
    },
    reload(e) {
      if (this.hasGetProTemplatePermission) {
        if(arguments.length>0){
            this.searchOption.keyword=e;
            this.searchOption.page=1;
        }
        this.loading = true;
        
        this.$axios
          .post(
            this.$baseURL + "/proreportpackage/protemplatelist",
            this.searchOption
          )
          .then((res) => {
            this.loading = false;
            this.data = res.data.data;
            this.total = res.data.total;
          });
      }
    },
    redo() {
      this.selectedTemplate = undefined;
      this.groupItems = []; 
      this.form.groupItem = undefined;
      this.form.name='';
    },
    pick(row) {
      this.selectedTemplate = row;
      if (this.hasGetGroupItemPermission) {
        this.$axios
          .post(this.$baseURL+"/proreportpackage/GetGroupItems", "id=" + row.Id)
          .then((res) => {
            this.groupItems = res.data.data.filter((s) => s.Mate);
          });
      }
    },
    add() {
      if (this.hasAddPermission) {
        this.$refs.form.validate((r) => {
          if (r) {
            if (
              this.selectedTemplate &&
              this.form.groupItem &&
              this.form.name
            ) {
                var loading = this.$loading({ text: "正在处理请求" });
                var formData=new FormData();
                formData.append('id',this.selectedTemplate.Id);
                formData.append('groupItemId',this.form.groupItem);
                formData.append('packageName',this.form.name);
                this.$axios.post(this.$baseURL+'/proreportpackage/add',formData).then(res=>{
                    loading.close();
                    if(res.data.success){
                        this.visiable=false;
                        this.$emit('reload');
                    }else{
                        this.$message.error(res.data.msg)
                    }

                })
            } else {
              this.$message.warning("请将数据填写完整后创建归档任务");
            }
          }
        });
      }
    },
  },
};
</script>
