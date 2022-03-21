<template>
<div style="flex:1 1 70%; padding:0 12px;">
            <div style="width:100%;height:100%;display:flex;flex-direction:row;">
                <div  class="echarts" style="flex:1 1 80%;">
                    <div style="position:absolute;left:10px;z-index:1111">
                      <el-button class="change_btn" type="primary" plain @click="clickAdd">{{text}}</el-button>
                    </div>
                    <div :style="{height:'500px',width:'100%'}" ref="myEchart"></div>
                </div>
                <div style="flex:1 1 20%;">
                    <ElTable :data="filterNList" height="100%">
                          <ElTableColumn prop="cn">
                            <template slot="header" slot-scope="scope">
                                 <ElInput v-model="filterkeyword"
                                          size="mini" type="text"
                                          placeholder="国家/城市" @input="change($event,scope)"/>
                            </template>
                        </ElTableColumn>
                        <ElTableColumn prop="value"
                                         label="人数">
                        </ElTableColumn>
                    </ElTable>
                </div>
            </div>
        </div>
    
</template>
<script>
import echarts from "echarts";
import 'echarts/map/js/china' // 引入中国地图数据
import 'echarts/map/js/world'
  export default {
    name: "echarts",
    data() {
      return {
        chart: null,
        chinaList:[],
        worldList:[],
        nList:[],
        filterkeyword:"",
        text:"切换到国家"
      };
    },
     computed: {
            filterNList() {
                if (this.nList) {
                    return this.nList.filter(s => {
                        try {
                            return s.cn.indexOf(this.filterkeyword) > -1
                        } catch (e) {
                            return false;
                        }
                    })
                } else {
                    return []
                }
              
            }
        },
    mounted() {
        this.getData();
         //this.$nextTick(()=>{this.chinaConfigure();})
        setTimeout(()=>{this.chinaConfigure();},1000)
    },
    beforeDestroy() {
      if (!this.chart) {
        return;
      }
      this.chart.dispose();
      this.chart = null;
    },
    methods: {
       clickAdd(){
          if(this.text=="切换到国家"){
            this.nList = this.worldList
              this.initChart();
            }else{
              this.nList = this.chinaList
              this.chinaConfigure();
            }
            this.text = this.text=="切换到国家"?"切换到城市":"切换到国家"
           
        },
        getData(){
            var url = "/LogManager/GetIpLogs";
            var that = this
            if (url) {
                this.$axios
                .post(this.$baseURL + url)
                .then(res => {
                    var list = [];
                    var list1 = [];
                    
                    for(var i in res.data.data){
                         var obj = { name: res.data.data[i].name, 
                         value: res.data.data[i].VisitorCount, 
                         cn:  res.data.data[i].Name_CN,
                         code: res.data.data[i].Code };
                        if(obj.code == "CN" && obj.cn != "中国"){
                            list.push(obj)
                        }
                        if (obj.code != 'CN' || (obj.code == 'CN' && obj.cn == "中国")) {
                            list1.push(obj)
                        }
                    }
                    that.chinaList = list;
                    that.nList = list;
                    that.worldList = list1;
                });
                
            }
        },
      chinaConfigure() {

        var that = this
        let myChart = echarts.init(this.$refs.myEchart); //这里是为了获得容器所在位置    
        window.onresize = myChart.resize;
        myChart.setOption({ // 进行相关配置
          backgroundColor: "#FFF",
          tooltip: {
             trigger: "item",
            formatter: function (a) {
              var e = (a.value + "").split(".");
              try {
                return e , that.nList.filter(s => s.name == a.name)[0].cn + "<br/>" + "访问人数 : " + e
              } catch (ex) {
                return e , a.name + "<br/>" + "访问人数 : " + e
              }
            }         
          }, // 鼠标移到图里面的浮动提示框
          dataRange: {
            show: true,
            min: 0,
            max: 100,
            text: [],
            realtime: false,
            calculable: false,
            color: ['orangered', 'yellow', 'lightskyblue']
          },
          geo: { // 这个是重点配置区
            map: 'china', // 表示中国地图
            roam: true,
            label: {
              normal: {
                show: true, // 是否显示对应地名
                textStyle: {
                  color: 'rgba(0,0,0,0.4)'
                }
              }
            },
            itemStyle: {
              normal: {
                borderColor: 'rgba(0, 0, 0, 0.2)'
              },
              emphasis: {
                areaColor: null,
                shadowOffsetX: 0,
                shadowOffsetY: 0,
                shadowBlur: 20,
                borderWidth: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
              }
            }
          },
          series: [{
              type: 'scatter',
              coordinateSystem: 'geo' // 对应上方配置
            },
            {
              name: '启动次数', // 浮动框的标题
              type: 'map',
              geoIndex: 0,
              data: that.nList
            }
          ]
        })
      },
      initChart() {

        var that = this
        let myChart = echarts.init(this.$refs.myEchart); //这里是为了获得容器所在位置    
        window.onresize = myChart.resize;
        myChart.setOption({ // 进行相关配置
          backgroundColor: "#FFF",
          tooltip: {
            trigger: "item",
            formatter: function (a) {
              var e = (a.value + "").split(".");
              try {
                return e , that.nList.filter(s => s.name == a.name)[0].cn + "<br/>" + "访问人数 : " + e
              } catch (ex) {
                return e , a.name + "<br/>" + "访问人数 : " + e
              }
            }            
          }, // 鼠标移到图里面的浮动提示框
          dataRange: {
            show: true,
            min: 0,
            max: 100,
            text: [],
            realtime: false,
            calculable: false,
            color: ['orangered', 'yellow', 'lightskyblue']
          },
          geo: { // 这个是重点配置区
            map: 'world', 
            roam: true,
            label: {
              normal: {
                show: false, // 是否显示对应地名
                textStyle: {
                  color: 'rgba(0,0,0,0.4)'
                }
              }
            },
            itemStyle: {
              normal: {
                borderColor: 'rgba(0, 0, 0, 0.2)'
              },
              emphasis: {
                areaColor: null,
                shadowOffsetX: 0,
                shadowOffsetY: 0,
                shadowBlur: 20,
                borderWidth: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
              }
            }
          },
          series: [{
              type: 'scatter',
              coordinateSystem: 'geo' // 对应上方配置
            },
            {
              name: '启动次数', // 浮动框的标题
              type: 'map',
              geoIndex: 0,
              data: that.nList
            }
          ]
        })
      },
    }
  }
</script>
<style scoped>
 .el-button--primary.is-plain:focus,.bottom-div .el-button--primary.is-plain:hover {
    color: #409EFF;
    background: #ecf5ff;
    border-color: #b3d8ff;
}
</style>