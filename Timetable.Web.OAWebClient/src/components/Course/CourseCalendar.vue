<template>
  <div>
    <ElRow :gutter="10">
      <ElCol v-if="innerValue" :span="editable?18:24">
        <el-calendar class="mycalendar" v-model="selectDate">
          <template v-slot:dateCell="{ date, data }">
            <div class="calendar-cell" :class="{ inarea: inArea(data) }">
              <div class="ke" v-if="hasKe(data)">
                <div class="hasKe"></div>
                <div class="list">
                  {{
                    innerValue.Book[data.day.replace(/-0/g, "-")].join("、")
                  }}节
                </div>
              </div>
              <div class="weektip" v-if="getWeekTip(data)">
                第{{ getWeekTip(data) }}周
              </div>
              <div class="date">
                {{
                  data.day
                    .split("-")
                    .slice(1)
                    .join("-")
                }}
              </div>
            </div>
          </template>
        </el-calendar>
      </ElCol>
      <ElCol v-if="innerValue&&editable" :span="6">
        <ElButton
          @click="showMigic"
          type="success"
          icon="el-icon-magic-stick"
          plain
          round
          size="small"
          style="width:100%"
          >点击进行批量设置</ElButton
        >

        <h2 v-if="selectDate">
          {{ getDate(selectDate) }}
        </h2>
        <div
          v-if="
            selectDate &&
              inArea({ day: getDate(selectDate) }) &&
              innerValue.Book[getDate(selectDate)]
          "
        >
          <el-checkbox-group
            size="medium"
            @change="changeKe"
            v-model="innerValue.Book[getDate(selectDate)]"
          >
            <el-checkbox v-for="i in 12" :label="i" :key="i">{{
              "第" + i + "节"
            }}</el-checkbox>
          </el-checkbox-group>
        </div>
        <div v-else>
          不在本学期校历日期内
        </div>
      </ElCol>
    </ElRow>

    <ElDialog
      title="批量设置"
      append-to-body
      :visible.sync="visiable"
      width="1200px"
    >
      <ElForm ref="form" inline :model="quick">
        <ElFormItem
          prop="startWeek"
          :rules="[{ required: true, message: '请填写' }]"
        >
          第
          <el-input-number v-model="quick.startWeek" :min="1" :max="20" />
          周开始上课 ，
        </ElFormItem>
        <ElFormItem
          prop="count"
          :rules="[{ required: true, message: '请填写' }]"
        >
          共上 <el-input-number v-model="quick.count" :min="1" :max="30" /> 周课
        </ElFormItem>
        <ElFormItem label="周一 ：">
          <el-checkbox-group
            size="medium"
            @change="changeKe"
            v-model="quick.data[1]"
          >
            <el-checkbox v-for="i in 12" :label="i" :key="i">{{
              "第" + i + "节"
            }}</el-checkbox>
          </el-checkbox-group>
        </ElFormItem>

        <ElFormItem label="周二 ：">
          <el-checkbox-group
            size="medium"
            @change="changeKe"
            v-model="quick.data[2]"
          >
            <el-checkbox v-for="i in 12" :label="i" :key="i">{{
              "第" + i + "节"
            }}</el-checkbox>
          </el-checkbox-group>
        </ElFormItem>
        <ElFormItem label="周三 ：">
          <el-checkbox-group
            size="medium"
            @change="changeKe"
            v-model="quick.data[3]"
          >
            <el-checkbox v-for="i in 12" :label="i" :key="i">{{
              "第" + i + "节"
            }}</el-checkbox>
          </el-checkbox-group>
        </ElFormItem>
        <ElFormItem label="周四 ：">
          <el-checkbox-group
            size="medium"
            @change="changeKe"
            v-model="quick.data[4]"
          >
            <el-checkbox v-for="i in 12" :label="i" :key="i">{{
              "第" + i + "节"
            }}</el-checkbox>
          </el-checkbox-group>
        </ElFormItem>
        <ElFormItem label="周五 ：">
          <el-checkbox-group
            size="medium"
            @change="changeKe"
            v-model="quick.data[5]"
          >
            <el-checkbox v-for="i in 12" :label="i" :key="i">{{
              "第" + i + "节"
            }}</el-checkbox>
          </el-checkbox-group>
        </ElFormItem>
        <ElFormItem label="周六 ：">
          <el-checkbox-group
            size="medium"
            @change="changeKe"
            v-model="quick.data[6]"
          >
            <el-checkbox v-for="i in 12" :label="i" :key="i">{{
              "第" + i + "节"
            }}</el-checkbox>
          </el-checkbox-group>
        </ElFormItem>
        <ElFormItem label="周日 ：">
          <el-checkbox-group
            size="medium"
            @change="changeKe"
            v-model="quick.data[0]"
          >
            <el-checkbox v-for="i in 12" :label="i" :key="i">{{
              "第" + i + "节"
            }}</el-checkbox>
          </el-checkbox-group>
        </ElFormItem>
      </ElForm>
      <span slot="footer">
        <el-button @click="visiable = false">取消</el-button>
        <el-button type="primary" @click="set">设置</el-button>
      </span>
    </ElDialog>
  </div>
</template>
<script>
import Vue from "vue";
export default {
  props: {
    value: {
      type: Object,
      required: true,
    },
    term: {
      type: Object,
    },
    editable:{
        type:Boolean,
        default(){
            return true;
        }
    }
  },
  data() {
    return {
      innerValue: undefined,
      selectDate: new Date(),
      visiable: false,
      quick: {
        startWeek: 1,
        count: undefined,
        data: {
          0: [],
          1: [],
          2: [],
          3: [],
          4: [],
          5: [],
          6: [],
        },
      },
    };
  },
  mounted() {
    this.innerValue = this.value;
    this.$nextTick(() => {
      var ss = this.getDate(this.selectDate);
      if (!this.innerValue.Book[ss]) {
        this.$set(this.innerValue.Book, ss, []);
      }
    });
  },
  watch: {
    selectDate: function(s) {
      var ss = this.getDate(s); 
      if (this.inArea({ day: ss }) ) {
          if(!this.innerValue.Book[ss]){

        this.$set(this.innerValue.Book, ss, []);
          }
      } else {
        this.$delete(this.innerValue.Book, ss);
      }
    },
    value: function(v) {
      this.innerValue = v;
        var ss = this.getDate(this.selectDate); 
      if (this.inArea({ day: ss }) ) {
          if(!this.innerValue.Book[ss]){

        this.$set(this.innerValue.Book, ss, []);
          }
      } else {
        this.$delete(this.innerValue.Book, ss);
      }
    },
  },
  computed: {
    fixedStart() {
      if (this.term) {
        return new Date(this.term.FixedStart);
      }
      return undefined;
    },
    end() {
      if (this.term) {
        return new Date(this.term.CustomEnd);
      }
      return undefined;
    },
    start() {
      if (this.term) {
        return new Date(this.term.CustomStart);
      }
      return undefined;
    },
  },
  methods: {
    set() {
      this.$refs.form.validate((r) => {
        if (r) {
          var x = new Date(this.fixedStart);
         
          var weekStart=new Date(this.fixedStart);
          weekStart.setDate(weekStart.getDate()+(this.quick.startWeek -1)*7)
           var lengthEnd = new Date(new Date(Vue.filter('date')(weekStart)+' 0:0:0'));
          lengthEnd.setDate( lengthEnd.getDate() + this.quick.count * 7 - 1);
          var day = 1;
          console.log(lengthEnd);
          console.log(weekStart)
          while (x - this.end <= 0 && x - lengthEnd <= 0) {
            if (x - weekStart >= 0 && x-this.start>=0 && this.quick.data[day].length > 0) {
              var key = Vue.filter("date")(x).replace(/-0/g, "-");
              console.log(key);
              if (!this.innerValue.Book[key]) {
                this.$set(this.innerValue.Book, key, []);
              }
              var set = new Set(this.innerValue.Book[key]);
              for (const i of this.quick.data[day]) {
                if (!set.has(i)) {
                  set.add(i);
                }
              }
              this.innerValue.Book[key] = [...set];
            }
            x.setDate(x.getDate() + 1);
            day = (day + 1) % 7;
          }
          this.visiable=false;
        }
      });
    },
    resetQuick() {
      this.quick = {
        startWeek: 1,
        count: undefined,
        data: {
          0: [],
          1: [],
          2: [],
          3: [],
          4: [],
          5: [],
          6: [],
        },
      };
    },
    showMigic() {
      this.resetQuick();
      this.visiable = true;
      this.$nextTick(()=>{
          this.$refs.form.clearValidate()
      })
    },
    getDate(date) {
      return Vue.filter("date")(date).replace(/-0/g, "-");
    },
    hasKe({ day }) {
      day = day.replace(/-0/g, "-");
      if (this.innerValue.Book[day] && this.innerValue.Book[day].length > 0) {
        return true;
      } else {
        return false;
      }
    },
    getWeekTip({ day }) {
      var d = new Date(day + " 00:00:00");

      var diffStart = d - this.fixedStart;
      var diffEnd = d - this.end;

      if (this.fixedStart && diffStart >= 0 && diffEnd <= 0) {
        var diffDay = diffStart / (3600 * 1000 * 24);
        if (diffDay % 7 == 0) {
          return parseInt(diffDay / 7) + 1;
        }
      }
    },
    inArea({ day }) {
      var d = new Date(day + " 00:00:00");

      var diffStart = d - this.start;
      var diffEnd = d - this.end;

      if (this.start && diffStart >= 0 && diffEnd <= 0) {
        return true;
      } else {
        return false;
      }
    },
    changeKe(e) {
      console.log(e);
      e = e.sort((a,b)=>a-b);
    },
  },
};
</script>

<style lang="less">
.mycalendar .el-calendar-table__row{
   .prev,.next{
       .el-calendar-day{
opacity: 1;
       }
       
   }
   .is-today{
    //    color:#555;
   }
   .current{
       font-weight: bold;
       color:#555;
   }
}
.calendar-cell {
  width: 100%;
  left: -8px;
  top: -8px;
  height: 100%;
  padding: 8px;
  position: relative;
  &.inarea {
    background: rgba(245, 255, 255, 0.6);
  }
  .weektip {
    position: absolute;
    right: 5px;
    top: 5px;
    background: rgb(24, 183, 204);
    font-size: 18px;
    padding: 5px;
    color: #fff;
    border-radius: 3px;
    transform: scale(0.5);
    transform-origin: right top;
  }
  .date {
    position: absolute;
    top: 5px;
    left: 5px;
  }
  .ke {
    .hasKe {
      position: absolute;
      width: 0px;
      height: 0px;
      left: 0px;
      top: 0px;
      border-top: 15px solid orange;
      border-right: 15px solid transparent;
    }

    margin: -4px;
    margin-top: 20px;
    font-size: 10px;
    padding: 4px;
    background: rgba(255, 166, 0, 0.774);
    border-radius: 10px;
    text-align: center;
    color: #fff;
  }
}
.is-selected .calendar-cell {
  background: lighten(rgb(24, 183, 204), 30);
  .date {
    font-weight: bold;
    color: #fff;
  }
}
</style>
