<template>
  <div class="previewpaper">
    <ElScrollbar style="height:100%;overflow-x:hidden">
      
      <div class="shadow">
        <div class="papercontainer" style="margin-bottom:10px; ">
          <!-- 问卷主体 -->
          <div v-if="paper">
            <!-- 答题界面 -->
            <div v-if="!finish">
              <div v-if="paperMode == false || pageNum == 0">
                <h2 style="text-align:center">{{ paper.basicInfo["Name_" + lang] }}</h2>
                <div v-html="$xss(paper.basicInfo['Introduction_' + lang])"></div>
              </div>
              <div class="ti">
                <ElForm @submit.native.prevent size="mini" :model="answers" ref="mainform">
                  <template v-for="question in visiableList">
                    <DesignItem
                      :id="'id_' + question.id"
                      v-show="!paperMode || QuestionsMapByPage[question.id] == pageNum"
                      :isWX="isWX"
                      :answers="answers"
                      :lang="lang"
                      :ref="'c_' + question.id"
                      :key="question.id"
                      :question="question"
                      :numObj="numObj"
                    ></DesignItem>
                  </template>
                </ElForm>
              </div>

              <div class="bottomtoolbar">
                <table>
                  <tr>
                    <td v-if="paperMode && pageNum > 0">
                      <ElButton :disabled="!pageButtonsAvailable.prev" type="default" @click="prevPage" round size="mini">
                        <i v-if="!pageText.startsWith('1 /')" class="el-icon-arrow-left"></i>
                        {{ pageText.startsWith("1 /") ? "欢迎页" : "" }}
                      </ElButton>
                    </td>
                    <td v-if="paperMode && pageNum > 0" style="font-size:14px">
                      {{ pageText }}
                    </td>
                    <td v-if="paperMode">
                      <ElButton type="default" :disabled="!pageButtonsAvailable.next" @click="nextPage" round :size="pageNum > 0 ? 'mini' : 'large'">
                        <i v-if="pageNum > 0" class="el-icon-arrow-right"></i>
                        {{ pageNum > 0 ? "" : "开始答题" }}
                      </ElButton>
                    </td>
                    <td v-if="paperMode && pageNum > 0">
                      <el-divider direction="vertical" content-position="left"></el-divider>
                    </td>
                    <td v-if="!(paperMode && pageNum == 0) &&!viewMode">
                      <ElButtonGroup>
                        <ElButton type="danger" size="small" @click="clearAnswer">重置</ElButton>
                        <ElButton type="primary" @click="valid" size="small"> 提交</ElButton>
                      </ElButtonGroup>
                    </td>
                  </tr>
                </table>
              </div>
            </div>
            <!-- 答题完毕感谢语 -->
            <div v-else style="font-size:16px;text-align:center;color:#55aa55;padding-top:30px;padding-bottom:30px;">
              <i class="el-icon-circle-check" style="display:block;font-size:50px;margin-bottom:10px;"></i>
              {{ paper.setting.welcome ? paper.setting.welcome : "您的答卷已提交，感谢您的参与！" }}
            </div>
          </div>
          <div v-else-if="nofound == false && !paper">
            <div style="text-align:center;padding:100px 50px; color:#999;line-height:40px;">
              <i class="el-icon-loading" style="display:block;font-size:40px;" aria-hidden="true"></i>
              {{ this.lang == "CN" ? "正在获取问卷" : "Loading..." }}
            </div>
          </div>

          <!-- 未找到问卷 -->
          <div v-else-if="nofound">
            <div style="text-align:center;padding:100px 50px; color:#999;line-height:40px;">
              <i class="fa fa-file-text-o" style="display:block;font-size:40px;" aria-hidden="true"></i>
              {{ this.lang == "CN" ? "问卷不存在" : "The questionnaire does not exist or the survey has ended." }}
            </div>
          </div>
        </div>
      </div>

    </ElScrollbar>
  </div>
</template>
<script>
import DesignItem from "./DesignItem";

export default {
  props: ["isWX", "testInput", "data"],
  components: {
    DesignItem,
  },
  data() {
    return {
      finish: false,
      pageNum: 0,
      paperMode: true,
      showDialog: false,
      nofound: false,
      timer: null,
      continue: false,
      lang: "CN",
      studentCode: "",
      viewMode: false,

      setting: {},

      countLimited: false,
      countLimitedText: "",
      inTime: false,
      userinfo: {},
      paper: undefined,
      answers: {},
      visiableList: [],
    };
  },
  computed: {
    numObj() {
      var ex = ["Title", "Text", "PaperBreak"];
      var obj = {};
      var num = 0;

      for (var q of this.paper.originList) {
        if (ex.indexOf(q.type) == -1) {
           if(q.questionNo&&q.dependents&&q.dependents.items&&q.dependents.items.length>0){
             obj[q.id]=q.questionNo;
          }else{
             obj[q.id] = ++num;
          }
        } else {
          switch (q.type) {
            case "Title":
              obj[q.id] = "段落标题";
              break;
            case "Text":
              obj[q.id] = "段落文字";
              break;

            case "PaperBreak":
              obj[q.id] = "分页符";
              break;
          }
        }
      }
      console.warn("计算题号");
      return obj;
    },
    totalpages() {
      console.log("计算分页总数");
      return this.paper.originList.filter((s) => s.type == "PaperBreak").length;
    },
    pages() {
      return this.paper.originList.filter((s) => s.type == "PaperBreak");
    },
    canDependentOptions() {
      if (this.paper) {
        var obj = {};

        const inc = ["InlineSingle", "Multi", "Nps", "ScoreMulti", "ScoreSingle", "Select", "Single"];
        var list = this.paper.originList.filter((s) => inc.indexOf(s.type) > -1);
        list.forEach((s) => {
          obj[s.id] = {};
          s.options.forEach((o) => {
            obj[s.id][o.value] = o.oid;
          });
        });
        return obj;
      } else {
        return {};
      }
    },
    QuestionsByPage() {
      if (this.paper && this.paper.originList) {
        let pages = [];
        let page = undefined;
        this.paper.originList.forEach((q) => {
          if (q.type == "PaperBreak") {
            if (page && page.count > 0) {
              pages.push(page);
            }
            page = { count: 0, list: [] };
          } else {
            page.count++;
            page.list.push(q);
          }
        });
        if (page && page.count > 0) {
          pages.push(page);
        }
        var index = 0;
        pages.forEach((p) => {
          p.pageNum = ++index;
        });

        return pages;
      } else {
        return [];
      }
    },
    QuestionsMapByPage() {
      var map = {};
      if (this.QuestionsByPage.length > 0) {
        this.QuestionsByPage.forEach((page) => {
          page.list.forEach((q) => {
            map[q.id] = page.pageNum;
          });
        });
      }
      return map;
    },
    visiableQuestionsByPage() {
      let pages = [];
      if (this.QuestionsByPage.length > 0) {
        this.QuestionsByPage.forEach((rawPaper) => {
          let page = { count: 0, list: [], pageNum: rawPaper.pageNum };
          rawPaper.list.forEach((q) => {
            if (this.visiableList.indexOf(q) > -1) {
              page.list.push(q);
              page.count++;
            }
          });
          if (page.count > 0) {
            pages.push(page);
          }
        });
      }
      return pages;
    },
    pageButtonsAvailable() {
      var obj = {
        prev: true,
        next: true,
      };
      if (this.pageNum == 0) {
        obj.prev = false;
      } else {
        var index = this.visiableQuestionsByPage.findIndex((s) => s.pageNum == this.pageNum);
        if (index == -1 || index == this.visiableQuestionsByPage.length - 1) {
          obj.next = false;
        }
      }
      return obj;
    },
    pageText() {
      return `${this.pageNum == 0 ? "简介" : this.visiableQuestionsByPage.findIndex((s) => s.pageNum == this.pageNum) + 1} / ${this.visiableQuestionsByPage.length}`;
    },
  },
  watch: {
    QuestionsByPage(list) {
      console.log(list);
      if (list.length == 1) {
        this.paperMode = false;
      }
    },
    data(v){
      this.getPaper()
      this.$nextTick(()=>{
        document.querySelector('.previewpaper .el-scrollbar__wrap').scrollTop	=0
      })
    }
  },
  mounted: async function() {
    this.getPaper();
    if (!this.viewMode) {
      this.$watch(
        "answers",
        () => {
          console.log("答案修改" + JSON.stringify(this.answers));
          this.__updateVisiableList();
        },
        {
          deep: true,
        }
      );
    }
  },

  methods: {
    __updateVisiableList() {
      console.log("更新可见题");
      var step1arr = [],
        step2arr = [];
      if (this.paper) {
        var gotoTarget = null;
        for (let i = 0; i < this.paper.originList.length; i++) {
          const question = this.paper.originList[i];
          if (gotoTarget) {
            //跳题查找模式
            if (gotoTarget != question.id) {
              //不是要找的题，跳过，不显示
              if (this.answers[question.id]) {
                //删除答案
                this.$delete(this.answers, question.id);
              }
              continue;
            } else {
              //找到了，退出查找模式
              gotoTarget = null;
            }
          }
          step1arr.push(question);
          if (question.gotos && (question.gotos.always || (question.gotos.options && question.gotos.options.length > 0)) && this.hasAnswered(question)) {
            //有跳题逻辑并且该题已做答
            if (question.gotos.always) {
              //有无条件跳题逻辑，执行
              gotoTarget = question.gotos.always;
            } else if (question.gotos.options.length > 0) {
              const oid = question.options.find((s) => s.value == this.answers[question.id]).oid;
              if (oid) {
                const gotoOption = question.gotos.options.find((s) => s.oid == oid);
                if (gotoOption) {
                  gotoTarget = gotoOption.to;
                }
              }
            }
          }
        }
        //到这里，处理完了跳题,在step1arr中
        for (const question of step1arr) {
          var deleteAnswer = true;
          if (question.dependents && question.dependents.items && question.dependents.items.length > 0) {
            //有依赖，判断
            if (question.dependents.rule == "or") {
              //多个依赖之间为或关系，只要有一条成立就显示
              for (var item of question.dependents.items) {
                if (this.answers[item.id] != undefined && this.answers[item.id] !== "") {
                  if (this.answers[item.id] instanceof Array) {
                    //多选题
                    const answerOids = this.answers[item.id].map((s) => this.canDependentOptions[item.id][s]);
                    if (item.rule == "or") {
                      let find = false;
                      for (let itemoid of item.oids) {
                        if (answerOids.indexOf(itemoid) > -1) {
                          find = true;
                          break;
                        }
                      }
                      if (find) {
                        step2arr.push(question);
                        deleteAnswer = false;
                        break;
                      }
                    } else {
                      let miss = false;
                      for (let itemoid of item.oids) {
                        if (answerOids.indexOf(itemoid) == -1) {
                          //不全
                          miss = true;
                          break;
                        }
                      }
                      if (!miss) {
                        step2arr.push(question);
                        deleteAnswer = false;
                        break;
                      }
                    }
                  } else {
                    //单选题
                    const oid = this.canDependentOptions[item.id][this.answers[item.id]];
                    if (item.oids.indexOf(oid) > -1) {
                      //找到了
                      step2arr.push(question);
                      deleteAnswer = false;
                      break;
                    }
                  }
                }
              }
            } else {
              //多个依赖之间为且关系，必须全部满足
              var allright = true;
              for (let item of question.dependents.items) {
                if (this.answers[item.id] != undefined && this.answers[item.id] !== "") {
                  //有答题
                  if (this.answers[item.id] instanceof Array) {
                    //多选题
                    const answerOids = this.answers[item.id].map((s) => this.canDependentOptions[item.id][s]);
                    if (item.rule == "or") {
                      let find = false;
                      for (let itemoid of item.oids) {
                        if (answerOids.indexOf(itemoid) > -1) {
                          find = true;
                          break;
                        }
                      }
                      if (!find) {
                        allright = false;
                        break;
                      }
                    } else {
                      let miss = false;
                      for (let itemoid of item.oids) {
                        if (answerOids.indexOf(itemoid) == -1) {
                          //不全
                          miss = true;
                          break;
                        }
                      }
                      if (miss) {
                        allright = false;
                        break;
                      }
                    }
                  } else {
                    //单选题
                    const oid = this.canDependentOptions[item.id][this.answers[item.id]];
                    if (item.oids.indexOf(oid) == -1) {
                      //没找到
                      allright = false;
                      break;
                    }
                  }
                } else {
                  //没答题
                  allright = false;
                  break;
                }
              }
              if (allright) {
                step2arr.push(question);
                deleteAnswer = false;
              }
            }
          } else {
            step2arr.push(question);
            deleteAnswer = false;
          }
          if (deleteAnswer) {
            this.$delete(this.answers, question.id);
          }
        }
        //["Single","ScoreSingle","InlineSingle","Select","Nps"]
      }

      this.visiableList = step2arr;
    },

    hasAnswered(question) {
      if (question.type.indexOf("Martrix") > -1) {
        //矩阵题判断所有小题都选了
        for (let item of question.items) {
          if (this.answers[question.id] && this.answers[question.id][item.fid] != undefined && this.answers[question.id][item.fid] != "") {
            continue;
          } else {
            return false;
          }
        }
        return true;
      } else if (question.type == "Rate") {
        //比例题判断有不为0的
        for (let item of question.items) {
          if (this.answers[question.id]) {
            if (this.answers[question.id][item.fid] == 0) {
              continue;
            } else {
              return true;
            }
          } else {
            return false;
          }
        }
        return false;
      } else if (question.type.indexOf("Multi") > -1) {
        //多选题判断有选
        return this.answers[question.id] && this.answers[question.id].length > 0;
      } else if (question.type == "ChinaCity") {
        return this.answers[question.id] && this.answers[question.id][question.items[0].fid] != undefined;
      } else {
        //其他
        return this.answers[question.id] != undefined && this.answers[question.id] !== null && this.answers[question.id] !== "";
      }
    },

    valid() {
      this.$refs.mainform.validate((s) => {
        if (s) {
          this.__save();
        } else {
          this.$nextTick(() => {
            this.gotoFirstError();
          });
        }
      });
    },
    clearAnswer() {
      if (this.paperMode) {
        this.pageNum = 1;
      }
      this.answers = {};
      this.$nextTick(() => {
        this.$refs.mainform.clearValidate();
      });
    },
    __save() {
      this.$message({
        message: "答卷并未真实提交，仅演示效果",
        offset: 80,
      });
      this.finish = true;
    },
    getPaper() {
      if (this.data) {
        this.paper = this.data;
        this.visiableList = this.data.originList;
        this.paperMode = false;
        this.viewMode=true
      } else {
        this.$axios.post("/api/surveydesign/getpaper", "id=" + this.$route.params.id).then((res) => {
          if (res.data.success) {
            res.data.data.originList = JSON.parse(res.data.data.originList);
            this.visiableList = res.data.data.originList;
            this.paper = res.data.data;
            this.setting = res.data.data.setting;
          } else {
            this.nofound = true;
          }
        });
      }
    },
    prevPage() {
      if (this.pageButtonsAvailable.prev) {
        console.log("上一页");
        var loading = this.$loading();
        var prevs = this.visiableQuestionsByPage.filter((s) => s.pageNum < this.pageNum);
        if (prevs.length > 0) {
          this.pageNum = prevs[prevs.length - 1].pageNum;
        } else {
          this.pageNum = 0;
        }
        this.$nextTick(() => {
          loading.close();
        });
      }
    },
    nextPage() {
      if (this.pageButtonsAvailable.next) {
        console.log("下一页");
        var loading = this.$loading();
        var prevs = this.visiableQuestionsByPage.filter((s) => s.pageNum > this.pageNum);
        if (prevs.length > 0) {
          this.pageNum = prevs[0].pageNum;
        } else {
          this.pageNum = 0;
        }
        this.$nextTick(() => {
          loading.close();
        });
      }
    },
    gotoFirstError() {
      var target = document.querySelector(".designitem .is-error.el-form-item");
      let idNode = target.parentElement;
      while (!idNode.classList.contains("designitem")) {
        idNode = idNode.parentElement;
      }
      console.log(idNode);
      var id = idNode.id.replace("id_", "");
      if (this.paperMode) {
        this.pageNum = this.QuestionsMapByPage[id];
      }
      this.$nextTick(() => {
        window.scrollTo(0, idNode.offsetTop);
      });
    },
  },
};
</script>
<style lang="less">
.previewpaper {
  .el-scrollbar__wrap {
    overflow-x: hidden;
    padding:5px 5px; 
    box-sizing: border-box;
  }
  .el-scrollbar{
   
  }
  .shadow {
    pointer-events: none;
    box-shadow: 0px 0px 1px rgba(0, 0, 0, 0.5);
  }
  overflow-y: auto;
 
  height: 100%;
  .el-backtop {
    border-radius: 0;
    color: #fff;
    background: rgb(94, 164, 255);
    &:hover {
      background: darken(rgb(94, 164, 255), 10);
    }
  }
  .papercontainer {
    position: relative;
    z-index: 0;
    min-height: 100px;
    background: #fff;
    padding: 10px 5px;
    margin: 0 auto;
    width: calc(100% - 10px);
    box-sizing: border-box;
    .designitem {
      font-size: 16px;

      position: relative;
      .el-form-item {
        margin-bottom: 10px;

        border: 1px solid transparent;
        border-top: 1px dashed #ddd;
        &.is-error {
          border: 1px dashed rgb(255, 145, 145);
        }
        .el-form-item__content {
          padding: 20px 20px 10px 20px;
          .el-form-item__error {
            position: relative;
            top: 0;
            left: 0;
            margin-top: 10px;
            background: rgb(255, 229, 229);
            display: block;
            padding: 10px 5px;
            border-radius: 6px;
          }
        }
      }

      .pg {
        page-break-before: always;
      }
    }

    .red {
      color: red;
    }
    img {
      max-width: 100%;
    }
    .ti {
      font-weight: bold;
      font-size: 16px;
      display: flex;
      .qn {
       padding-right:5px;
      }
      .qt {
     
        pre {
          white-space: pre-wrap;
          margin: 0;
        }
      }
    }
    .matable {
      margin-top: 10px;
      table-layout: fixed;
      width: 100%;
      border-spacing: 0px;
      .scoretr {
        th {
          color: orange;
          font-weight: bold;
          &:first-child {
            text-align: left;
          }
        }
      }
      td,
      th {
        text-align: center;
        font-size: 14px;
        line-height: 20px;
        padding: 10px 5px;
      }

      tbody {
        th,
        td {
          border-bottom: 1px solid #eee;
        }
        tr:last-child {
          th,
          td {
            border-bottom: none;
          }
        }

        th {
          text-align: left;
          &.mt {
            border-bottom: none;
            padding: 0;
            span {
              display: block;
              background: #eee;
              height: 20px;
              padding: 5px;

              border-radius: 0px 0 7px 7px;
            }
          }
        }
      }
    }
    .optable {
      margin-top: 5px;
      width: 100%;
      table-layout: fixed;
      td {
        position: relative;
        padding: 3px 0;
        .score {
          color: orange;
        }
      }
      .el-checkbox__label {
        white-space: normal;
        line-height: 18px;
      }
      .el-checkbox.is-bordered {
        background: #fff;
        overflow: hidden;
        height: auto;
        min-height: 40px !important;
        padding-bottom: 12px;
        vertical-align: middle;

        .el-checkbox__input {
          width: 16px;
          position: absolute;
          top: calc(50% - 8px);

          bottom: 0;
          left: 5px;
        }
        .el-checkbox__label {
          margin-left: 16px;
          width: calc(100% - 26px);
          float: left;
          .img {
            text-align: center;
            display: block;
            img {
              max-width: 100%;
              max-height: 650px;
            }
          }
          .text {
            display: block;

            text-align: center;
          }
        }
      }

      .el-radio__label {
        white-space: normal;
        line-height: 18px;
      }
      .el-radio.is-bordered {
        background: #fff;
        overflow: hidden;
        height: auto;
        min-height: 40px !important;
        padding-bottom: 12px;
        vertical-align: middle;

        .el-radio__input {
          width: 16px;
          position: absolute;
          top: calc(50% - 8px);

          bottom: 0;
          left: 5px;
        }
        .el-radio__label {
          margin-left: 16px;
          width: calc(100% - 26px);
          float: left;
          .img {
            text-align: center;
            display: block;
            img {
              max-width: 100%;
              max-height: 650px;
            }
          }
          .text {
            display: block;

            text-align: center;
          }
        }
      }
    }
  }
  &.mobile {
    border: 1px solid #888;

    width: 380px;
    margin-left: auto;
    margin-right: auto;

    height: 600px;
    margin-top: 40px;
    overflow-y: scroll;
    padding: 15px 5px 15px 5px;
    .el-message {
      min-width: 300px !important;
      max-width: 350px !important;
    }
    .el-form-item__content {
      padding: 20px 10px 10px 10px !important;
    }
    .paper .el-backtop {
      display: none;
    }
    .hidden-sm {
      display: none;
    }
    .visible-sm {
      display: table-cell;
    }
    .hidden-sm-c {
      display: none;
    }
    .el-rate__item .el-rate__icon {
      margin-right: 0;
      font-size: 17px;
    }
  }
  &.desktop {
    .hidden-sm {
      display: table-cell;
    }
    .visiable-sm {
      display: none;
    }
    .papercontainer {
      width: 850px !important;
      margin: 0 auto;
      padding: 8px;
    }
  }
}
.fl{
  float:left;
}
.previewpaper {
  color: #555;
  margin-top: 0;
}
</style>
