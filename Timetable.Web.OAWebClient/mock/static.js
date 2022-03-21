var Mock =require( "mockjs");
/**
 * @description 将url请求参数转为json格式,在serverless模式使用
 * @param url
 * @returns {{}|any}
 */

function param2Obj(originUrl) {

  const url =originUrl.split("?")[1];
  var theRequest = new Object();
  if (originUrl.indexOf("?") != -1) {
     
      const strs = url.split("&");
     
      for(var i = 0; i < strs.length; i ++) {

          theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1]!==undefined?decodeURIComponent(strs[i].split("=")[1]):undefined;
      }
  }
  return theRequest;

}  
const mocks = [];
 
const controllerFiles = require.context("./controller", true, /\.js$/);

controllerFiles.keys().reduce((controller, controllerPath) => {
    const value = controllerFiles(controllerPath);
    const obj = value.default;
    mocks.push(...obj);
  }, {});


export function mockXHR() {
  Mock.XHR.prototype.proxy_send = Mock.XHR.prototype.send;
  Mock.XHR.prototype.send = function () {
    if (this.custom.xhr) {
      this.custom.xhr.withCredentials = this.withCredentials || false;

      if (this.responseType) {
        this.custom.xhr.responseType = this.responseType;
      }
    }
    this.proxy_send(...arguments);
  };

  function XHR2ExpressReqWrap(respond) {
    return function (options) {
      let result = null;
      if (respond instanceof Function) {
        const { body, type, url } = options;
        result = respond({
          method: type,
          type:"static",
          body: JSON.parse(body),
          query: param2Obj(url),
        });
      } else {
        result = respond;
      }
      return Mock.mock(result);
    };
  }

  for (const i of mocks) {
     console.log("注册api路由",`${process.env.VUE_APP_SERVER_BASE_API}${i.url}`)
    Mock.mock(

      new RegExp(`${process.env.VUE_APP_SERVER_BASE_API}${i.url}`),
      i.type || "get",
      XHR2ExpressReqWrap(i.response)
    );
  }
}
