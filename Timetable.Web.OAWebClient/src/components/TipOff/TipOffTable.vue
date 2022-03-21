<template>
  <div>
    <ElTable :data="list" highlight-current-row >
      <ElTableColumn show-overflow-tooltiplabel label="问卷ID" width="70" prop="RefferId"></ElTableColumn>
       <ElTableColumn show-overflow-tooltiplabel label="问卷名" prop="Name_CN">
        <template v-slot="{row}">
            <span v-if="row.Subject">{{row.Subject.Name_CN}}</span> 
            <span style="color:#bbb" v-else>问卷已删除</span>
        </template>
       </ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="问卷所属用户 (账号 / 真名)" prop="NickName">
        <template v-slot="{ row }" > 
           <span v-if="row.Subject"> {{ row.Subject.Mobile }}  / {{ row.Subject.RealName }}</span>
              <span style="color:#bbb" v-else>问卷已删除</span>
         </template>
      </ElTableColumn>
      <ElTableColumn show-overflow-tooltip label="举报人(称呼 / 手机号 / QQ)" prop="NickName">
       
        <template v-slot="{ row }">
          <span v-if="row.Contact||row.Mobile||row.QQ">  {{row.Contact}} / {{ row.Mobile }} / {{ row.QQ }} </span>
             <span style="color:#bbb" v-else>未填写</span>
           </template>
      </ElTableColumn>
      <ElTableColumn width="180" show-overflow-tooltip label="举报内容" prop="CreateTime">
        <template v-slot="{ row }">
          {{ row.Content }}
        </template>
      </ElTableColumn>
        <ElTableColumn width="180" show-overflow-tooltip label="处理备注" prop="CreateTime">
        <template v-slot="{ row }">
          {{ row.Remark }}
        </template>
      </ElTableColumn>
      <ElTableColumn width="80" show-overflow-tooltip label="处理进度" prop="Finished">
        <template v-slot="{ row }">
          <ElTag v-if="row.Finished" type="success" size="small">已处理 </ElTag>
          <ElTag v-else type="warning"  size="small">  待处理 </ElTag>
        </template>
      </ElTableColumn>
      <ElTableColumn width="160" show-overflow-tooltip label="创建时间" prop="CreateTime">
        <template v-slot="{ row }">
          {{ row.CreateTime | datetime }}
        </template>
      </ElTableColumn>

      <ElTableColumn width="200" label="操作" v-if="infoPermission||finishPermission">
        <template v-slot="{ row }">
          <ElButton   v-if="infoPermission&&row.Subject" @click="showDialog(row.RefferId)" plain icon="el-icon-view" round size="small" type="primary">查看问卷</ElButton>
          <ElButton v-if="finishPermission&&!row.Finished" @click.native="finishTipOff(row.Id)" plain icon="el-icon-check" round size="small" type="primary">完成</ElButton>
        </template> 
      </ElTableColumn>
    </ElTable>
    <BasicPaginationBar :page.sync="page" :size.sync="size" :total="total" @current-change="reload" @size-change="reload"></BasicPaginationBar>
    <ElDialog v-if="dialogVisiable && paper" title="问卷详情" visible width="920px" top="5vh" :close-on-click-modal="false" @close="dialogVisiable = false">
      <div style=" width:880px;height:75vh">
        <ViewPaper :data="paper" class="desktop"></ViewPaper>
      </div>
    </ElDialog>
  </div>
</template>
<script>
import ViewPaper from "../Paper/ViewPaper";
export default {
  components: {
    ViewPaper,
  },
  data() {
    return {
      page: 1,
      dialogVisiable: false,
      paper: undefined,
      searchOptions: {
        publishState: "",
        acceptAnswer: "",
        keyword: "",
      },
      colors: ["default", "warning", "success", "danger", "info"],
      size: 15,
      total: 0,
      list: [],
      subjects:[]
    };
  },
  inject: ["hasPermission"],
  filters: {
    state(v) {
      switch (v) {
        case 0:
          return "从未申请";
        case 1:
          return "待审核";

        case 2:
          return "通过";
        case 3:
          return "不通过";
        case 4:
          return "用户取消";
      }
    },
  },
  computed: {
    getPermission() {
      return this.hasPermission("TO_G");
    },
    infoPermission() {
      return this.hasPermission("TO_I");
    },
    finishPermission() {
      return this.hasPermission("TO_F");
    },
    
  },
  methods: {
    reload(e) {
      if (!this.getPermission) return;
      this.$nextTick(() => {
        if (arguments.length == 1) {
          this.searchOptions = e;
          this.page = 1;
        }
        this.$loading({ text: "正在加载表格数据" });
        var url = this.$baseURL + "/tipoff/list";
        if (url) {
          var data = {
            keyword: this.searchOptions.keyword,
            intFlag: this.searchOptions.publishState,
            boolFlag: this.searchOptions.acceptAnswer,
            page: this.page,
            rows: this.size,
          };

          if (this.searchOptions.dataRange && this.searchOptions.dataRange.length > 0) {
            data.datestart = this.searchOptions.dataRange[0];
            data.dateend = this.searchOptions.dataRange[1];
          }

          this.$axios.post(url, data).then((res) => {
            setTimeout(() => {
              this.$loading().close();
            }, 100);
            for(var i in res.data.data.list){
              for(var j in res.data.data.subjects){
                if(res.data.data.list[i].RefferId == res.data.data.subjects[j].Id){
                   res.data.data.list[i].Subject={};
                  res.data.data.list[i].Subject.Name_CN = res.data.data.subjects[j].Name_CN
                  res.data.data.list[i].Subject.Mobile = res.data.data.subjects[j].Mobile
                  res.data.data.list[i].Subject.RealName = res.data.data.subjects[j].RealName
                }
              }
            }
            this.list = res.data.data.list;
            this.subjects = res.data.data.subjects;
            this.total = res.data.total;
          });
        }
      });
    },
    showDialog(id) {
      if (!this.infoPermission) return;
      this.$axios.post(this.$baseURL + "/survey/info?id=" + id).then((res) => {
        if (res.data.success) {
          this.paper = res.data.data;
          this.paper.originList = JSON.parse(res.data.data.originList);
          this.dialogVisiable = true;
        } else {
          this.$message({
            message: res.data.msg,
            type: "error",
            offset: 80,
          });
        }
      });
    },
    finishTipOff(id) {
      if (!this.finishPermission) return;
      this.$prompt("可输入处理备注，如处理结果，处理原因等：", "举报信息处理", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        inputPattern: /^[.]{0,1024}/,
        inputErrorMessage: "原因最长1024个字符",
      })
        .then(({ value }) => {
          this.$axios.post(this.$baseURL + "/tipoff/finish", "id=" + id + "&remark="+encodeURIComponent(value!=null?value:'')).then((res) => {
            this.reload();
          });
        })
        .catch(() => {});
    }
  },
  mounted() {
    this.reload();
  },
};
</script>
