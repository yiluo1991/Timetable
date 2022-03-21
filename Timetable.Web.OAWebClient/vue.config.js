const WebpackBar = require('webpackbar');
const path = require("path");
const Webpack = require("webpack");

function resolve(dir) {
  return path.join(__dirname, dir);
}
function mockServer() {
  if (process.env.NODE_ENV === "local") {

    const mockServer = require("./mock/mock-server.js");
    return mockServer;
  } else {
    return undefined;
  }
}
module.exports = {
  publicPath:'/app',

  //是否生成map
  productionSourceMap:false,
  devServer: {
    open: true,
    hot: true,
    openPage: 'login',
    port: 8001,
    proxy:{
      '/api': {
        target: 'http://192.168.6.29:8888',  
        changeOrigin: true,
        timeout:700000
    },
    '/upload':{
      target: 'http://192.168.6.29:8888',  
      changeOrigin: true
    }
    },
    before: mockServer() 
  },
  chainWebpack(config) {
    config.when(process.env.NODE_ENV !== "development", (config) => {

      config.performance.set("hints", false);
      config.devtool("none");

      config.optimization.splitChunks({
        chunks: "all",
        cacheGroups: {
          libs: {
            name: "chunk-libs",
            test: /[\\/]node_modules[\\/]/,
            priority: 10,
            chunks: "initial",
          },
          elementUI: {
            name: "chunk-elementUI",
            priority: 20,
            test: /[\\/]node_modules[\\/]_?element-ui(.*)/,
          },
          commons: {
            name: "chunk-views",
            test: /[\\/]src[\\/]views[\\/]/,
            minChunks: 1,
            priority: 1110,
            minSize: 1,
            reuseExistingChunk: true,
          },
        },
      });
      config.optimization.runtimeChunk("single");
      config
        .plugin("banner")
        .use(Webpack.BannerPlugin, [
          `基于AdminUI构建\n厦门励航软件开发有限公司`+ new Date(),
        ])
        .end();
    });
  },
  configureWebpack(config) {
    var logo=`  ////////////////////////////////////////////////////////////////////  
    //                          _ooOoo_                               //  
    //                         o8888888o                              //  
    //                         88" . "88                              //  
    //                         (| ^_^ |)                              //  
    //                         O\\  =  /O                              //  
    //                      ____/'---'\\____                           //  
    //                    .'  \\\\|     |//  '.                         //  
    //                   /  \\\\|||  :  |||//  \\                        //  
    //                  /  _||||| -:- |||||-  \\                       //  
    //                  |   | \\\\\\  -  /// |   |                       //  
    //                  | \\_|  ''\\---/''  |   |                       //  
    //                  \\  .-\\__  '-'  ___/-. /                       //  
    //                ___'. .'  /--.--\\  '. . ___                     //  
    //              ."" '<  '.___\\_<|>_/___.'  >'"".                  //  
    //            | | :  '- \\'.;'\\ _ /';.'/ - ' : | |                 //  
    //            \\  \\ '-.   \\_ __\\ /__ _/   .-' /  /                 //  
    //      ========'-.____'-.___\\_____/___.-'____.-'========         //  
    //                           '=---='                              //  
    //      ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^        //  
    //            佛祖保佑       永不宕机      永无BUG              　//
    ////////////////////////////////////////////////////////////////////
     `;
    return {
      plugins: [
        new WebpackBar({
          name: logo+`adminui编译中，模式:${process.env.NODE_ENV}`,
        }),
      ],
    };
  }
}