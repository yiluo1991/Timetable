import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import axios from "axios";
import ElementUI from "element-ui";
import "nprogress/nprogress.css";
import xss from "xss";
import "element-ui/lib/theme-chalk/index.css";
import "font-awesome/css/font-awesome.css";
import common from "./components/Common";
import errorHandler from './resources/scripts/errorHandler';

axios.interceptors.response.use(
  (data) => {
    return data;
  },
  (err) => {
    switch (err.response.status) {
      case 404:
        ElementUI.Message({
          message: "数据请求失败，服务器返回404 Not Found",
          type: "error",
          offset: 80,
          duration: 1000,
        });
        break;
      case 400:
        if (err.response.data && err.response.data.msg) {
          ElementUI.Message({
            message: err.response.data.msg,
            type: "error",
            offset: 80,
            duration: 1000,
          });
        }
        break;
      case 403:
        if (err.response.data && err.response.data.msg) {
          ElementUI.Message({
            message: err.response.data.msg,
            type: "error",
            offset: 80,
          });
        }
        break;
      default:
        /**统一处理异常，关闭加载提示 */
        ElementUI.Message({
          message: err,
          type: "error",
          offset: 80,
          duration: 1000,
        });
        break;
    }
    if (document.getElementsByClassName("el-loading-mask is-fullscreen").length > 0) {
      var loading = ElementUI.Loading.service();
      setTimeout(() => {
        loading.close();
      }, 0);
    }
    return Promise.reject(err);
  }
);

Vue.config.productionTip = false;
Vue.prototype.$baseURL = process.env.VUE_APP_SERVER_BASE_API;

if (process.env.NODE_ENV === "serverless") {
  const { mockXHR } = require("../mock/static");
  mockXHR();
}

Vue.prototype.$axios = axios;

//注册异常处理函数
Vue.config.errorHandler=errorHandler;

var options = {
  css: false,
  whiteList: {
    a: ["target", "href", "title"],
    abbr: ["title"],
    address: [],
    area: ["shape", "coords", "href", "alt"],
    article: [],
    aside: [],
    audio: ["autoplay", "controls", "loop", "preload", "src"],
    b: [],
    bdi: ["dir"],
    bdo: ["dir"],
    big: [],
    blockquote: ["cite"],
    br: [],
    caption: [],
    center: [],
    cite: [],
    code: [],
    col: ["align", "valign", "span", "width"],
    colgroup: ["align", "valign", "span", "width"],
    dd: [],
    del: ["datetime"],
    details: ["open"],
    div: [],
    dl: [],
    dt: [],
    em: [],
    font: ["color", "size", "face"],
    footer: [],
    h1: [],
    h2: [],
    h3: [],
    h4: [],
    h5: [],
    h6: [],
    header: [],
    hr: [],
    i: [],
    img: ["src", "alt", "title", "width", "height"],
    ins: ["datetime"],
    li: [],
    mark: [],
    nav: [],
    ol: [],
    p: [],
    pre: [],
    s: [],
    section: [],
    small: [],
    span: [],
    sub: [],
    sup: [],
    strong: [],
    table: ["width", "border", "align", "valign"],
    tbody: ["align", "valign"],
    td: ["width", "rowspan", "colspan", "align", "valign"],
    tfoot: ["align", "valign"],
    th: ["width", "rowspan", "colspan", "align", "valign"],
    thead: ["align", "valign"],
    tr: ["rowspan", "align", "valign"],
    tt: [],
    u: [],
    ul: [],
    video: ["autoplay", "controls", "loop", "preload", "src", "height", "width"],
  },
}; // 自定义规则
for (var o in options.whiteList) {
  options.whiteList[o].push("style");
}
var myxss=new xss.FilterXSS(options);
Vue.prototype.$xss = myxss.process.bind(myxss);
//加载公共组件

Vue.use(common);

Vue.use(ElementUI);


new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount("#app");
