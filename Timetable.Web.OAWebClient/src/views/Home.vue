<template>
  <div class="admin-page home">
    <el-container v-if="info" style="height:100%">
      <el-header height="160px" style="padding:0;">
        <ElRow :gutter="14">
          <ElCol :span="5" class="block-outer">
            <div class="block-inner" @click="hasPermission('SV_G') & $router.push('/admin/survey')" :class="{ nav: hasPermission('SV_G') }">
              <h3>销售情况</h3>
              <p class="title">未支付 / 总订单：</p>
              <p>
                <span>{{ info.SubjectSummary.Runing }}</span> 份 / <span>{{ info.SubjectSummary.Total }}</span> 份
              </p>
              <p class="title">近一个月新增订单：</p>
              <p>
                <span>{{ total }}</span> 份
              </p>
            </div>
          </ElCol>
          <ElCol :span="5" class="block-outer">
            <div class="block-inner" @click="hasPermission('AS_G') & $router.push('/admin/auditsurvey')" :class="{ nav: hasPermission('AS_G') }">
              <h3>审核情况</h3>
              <p class="title">待审核：</p>
              <p>
                <span>{{ info.AuditingSummary.AuditingCount }}</span> 份
              </p>
              <p class="title">已审核：</p>
              <p>
                <span>{{ info.AuditingSummary.AuditedCount }}</span> 份
              </p>
            </div>
          </ElCol>
         

          <ElCol :span="5" class="block-outer">
            <div class="block-inner" :class="{ nav: hasPermission('UL_G') }" @click="hasPermission('UL_G') & $router.push('/admin/userloginlog')">
              <h3>访问情况</h3>
              <p class="title">近一个月登录：</p>
              <p>
                <span>{{ total2 }}</span> 人次
              </p>
              <p class="title">历史总计登录：</p>
              <p>
                <span>{{ info.LoginLogSummary.Total }}</span> 人次
              </p>
              
            </div>
          </ElCol>
           <ElCol :span="5" class="block-outer">
            <div class="block-inner" :class="{ nav: hasPermission('TP_G') }" @click="hasPermission('TP_G') & $router.push('/admin/template')">
              <h3>商品情况</h3>
              <p class="title">当前待审核商品：</p>
              <p>
                <span>{{ info.TemplateSummary.AuditingCount }}</span> 份
              </p> 
              <p class="title">当前已审核商品：</p>
              <p>
                <span>{{ info.TemplateSummary.AuditedCount }}</span> 份
              </p>
              
            </div>
          </ElCol>
           <ElCol :span="4" class="block-outer">
            <div class="block-inner" :class="{ nav: hasPermission('TO_G') }" @click="hasPermission('TO_G') & $router.push('/admin/tipoff')">
              <h3>举报情况</h3>
              <p class="title">待处理：</p>
              <p>
                <span>{{ info.FeedbackSummary.Unfinished }}</span> 份
              </p>
              <p class="title">已处理：</p>
              <p>
                <span>{{ info.FeedbackSummary.Total - info.FeedbackSummary.Unfinished }}</span> 份
              </p>
            </div>
          </ElCol>
        </ElRow>
      </el-header>
      <el-main style="padding:0;">
        <el-container style="height:100%">
          <el-header height="42px" style="padding:10px 0 0 0;">
            <el-radio-group v-model="radio" size="small">
              <el-radio-button label="1">近一个月新增用户</el-radio-button>
              <el-radio-button label="2">近一个月用户登录</el-radio-button>
            </el-radio-group>
          </el-header>
          <el-main style="padding:0;">
            <div id="homechart">
              <highcharts key="1" class="noprint" ref="charts" :options="radio == '1' ? columnOptions1 : columnOptions2"></highcharts>
            </div>
          </el-main>
        </el-container>
      </el-main>
    </el-container>
  </div>
</template>
<script>
import { Chart } from "highcharts-vue";
import highcharts from "highcharts";
highcharts.setOptions({
  global: {
    useUTC: false,
  },
  lang: {
    shortMonths: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
    weekdays: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
  },
});
export default {
  inject: ["hasPermission"],
  name: "adminrefresh",
  components: {
    highcharts: Chart,
  },
  updated() {},
  data() {
    return {
      height: 400,
      radio: "1",
      info: undefined,
      event: () => {
        var ele = document.getElementById("homechart");
        if (ele) {
          this.height = ele.offsetHeight;
        }
      },
      timer: undefined,
    };
  },
  computed: {
    total() {
      if (this.info.SubjectSummary.Summary30.length > 0) {
        return this.info.SubjectSummary.Summary30.map((s) => s.Count).reduce((a, b) => a + b);
      } else {
        return 0;
      }
    },
    total2() {
      if (this.info.LoginLogSummary.Summary30.length > 0) {
        return this.info.LoginLogSummary.Summary30.map((s) => s.Count).reduce((a, b) => a + b);
      } else {
        return 0;
      }
    },
    columnOptions1() {
      var d = {
        credits: {
          enabled: false,
        },

        legend: {
          enabled: false,
        },
        title: {
          text: "近一个月用户新增情况",
        },
        chart: {
          type: "column",
          height: this.height,
        },
        yAxis: {
          min: 0,
          allowDecimals: false,
          title: {
            text: "新增问卷（份）",
          },
        },
        xAxis: {
          type: "datetime",
          labels: {},
          dateTimeLabelFormats: {
            millisecond: "%H:%M:%S.%L",
            second: "%H:%M:%S",
            minute: "%H:%M",
            hour: "%H:%M",
            day: "%m-%d",
            week: "%m-%d",
            month: "%Y-%m",
            year: "%Y",
          },
        },

        tooltip: {
          pointFormat: "{series.name}: <b>{point.y}</b>",
          dateTimeLabelFormats: {
            millisecond: "%H:%M:%S.%L",
            second: "%H:%M:%S",
            minute: "%H:%M",
            hour: "%H:%M",
            day: "%Y-%m-%d,%A",
            week: "%m-%d",
            month: "%Y-%m",
            year: "%Y",
          },
        },
        series: [
          {
            name: "新增问卷",
            dataLabels: {
              enabled: true,
            },

            data: [],
          },
        ],
      };
      this.info.SubjectSummary.Summary30.forEach((o) => {
        d.series[0].data.push([new Date(o.Date) - 0, o.Count]);
      });
      return d;
    },
    columnOptions2() {
      var d = {
        credits: {
          enabled: false,
        },

        legend: {
          enabled: false,
        },
        title: {
          text: "近一个月用户登录情况",
        },
        chart: {
          type: "column",
          height: this.height,
        },
        yAxis: {
          min: 0,
          allowDecimals: false,
          title: {
            text: "登录人次",
          },
        },
        xAxis: {
          type: "datetime",
          labels: {},
          dateTimeLabelFormats: {
            millisecond: "%H:%M:%S.%L",
            second: "%H:%M:%S",
            minute: "%H:%M",
            hour: "%H:%M",
            day: "%m-%d",
            week: "%m-%d",
            month: "%Y-%m",
            year: "%Y",
          },
        },

        tooltip: {
          pointFormat: "{series.name}: <b>{point.y}</b>",
          dateTimeLabelFormats: {
            millisecond: "%H:%M:%S.%L",
            second: "%H:%M:%S",
            minute: "%H:%M",
            hour: "%H:%M",
            day: "%Y-%m-%d,%A",
            week: "%m-%d",
            month: "%Y-%m",
            year: "%Y",
          },
        },
        series: [
          {
            name: "登录人次",
            dataLabels: {
              enabled: true,
            },

            data: [],
          },
        ],
      };
      this.info.LoginLogSummary.Summary30.forEach((o) => {
        d.series[0].data.push([new Date(o.Date) - 0, o.Count]);
      });
      return d;
    },
  },
  mounted() {
    this.timer = setInterval(() => {
      this.event();
    }, 300);
    this.$axios.post(this.$baseURL + "/home/info").then((res) => {
      this.info = res.data.data;

      this.$nextTick(() => {
        this.event();
      });
    });
  },
  beforeDestroy() {
    clearInterval(this.timer);
  },
};
</script>
<style lang="less">
.admin-page.home {
  height: calc(100vh - 101px);
}
.tabmode {
  .admin-page.home {
    height: calc(100vh - 145px);
  }
}
.home {
  #homechart {
    overflow: hidden;
    height: 100%;
  }
  .block-outer {
    transform-style: preserve-3d;
    perspective: 500px;
    h3 {
      margin: 5px;
      color: #fff;
    }
    .title {
      color: #fff;
      margin: 5px 25px;
      text-align: left;
      font-weight: bold;
      font-size: 15px;
    }
    p {
      margin: 5px;
      text-align: center;
      color: #fff;
      span {
        font-size: 20px;
        font-weight: bold;
      }
    }
    height: 160px;
    .block-inner {
      border-width: 1px;
      border-style: solid;
      border-radius: 6px;
      height: 160px;
      padding: 5px;
      box-sizing: border-box;
      transition: all 0.3s;

      &.nav {
        cursor: pointer;
        transform: scale(1);
      }
    }

    .block-inner.nav:hover {
      filter: brightness(1.1);
    }
    &:nth-child(1) .block-inner {
      background: rgb(0, 162, 255);
      border-color: darken(rgb(0, 162, 255), 5);
      box-shadow: 0px 0px 0px rgb(0, 162, 255);

      &.nav:hover {
        box-shadow: 0px 0px 10px rgb(0, 162, 255);
      }
    }
    &:nth-child(2) .block-inner {
      background: rgb(255, 180, 17);
      border-color: darken(rgb(255, 180, 17), 5);
      box-shadow: 0px 0px 0px rgb(255, 180, 17);
      &.nav:hover {
        box-shadow: 0px 0px 10px rgb(255, 180, 17);
      }
    }
    &:nth-child(3) .block-inner {
      background: rgb(255, 0, 128);
      border-color: darken(rgb(255, 0, 128), 5);
      box-shadow: 0px 0px 0px rgb(255, 0, 128);
      &.nav:hover {
        box-shadow: 0px 0px 10px rgb(255, 0, 128);
      }
    }
    &:nth-child(4) .block-inner {
      background: rgb(16, 192, 110);
      border-color: darken(rgb(16, 192, 110), 5);
      box-shadow: 0px 0px 0px rgb(16, 192, 110);
      &.nav:hover {
        box-shadow: 0px 0px 10px rgb(16, 192, 110);
      }
    }
     &:nth-child(5) .block-inner {
      background: rgb(219, 62, 233);
      border-color: darken( rgb(219, 62, 233), 5);
      box-shadow: 0px 0px 0px  rgb(219, 62, 233);
      &.nav:hover {
        box-shadow: 0px 0px 10px  rgb(219, 62, 233);
      }
    }
  }
}
</style>
