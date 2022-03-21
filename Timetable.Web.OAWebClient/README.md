# 支持mock
在/mock/controller中可添加各控制器代码。
/mock/static.js和/mock/mock-server.js分别实现了脚本ajax拦截响应和mock本地API两套方案，并自动根据运行模式选取方案执行。
1. mock.js在npm run serverless模式下通过拦截替换ajax底层请求，使用脚本模拟服务器响应。该模式下，由于请求是模拟的，在network中不会看到发出请求。
2. 在 num run local模式下，使用devServer托管mock，实现后台响应。该模式下不替换ajax请求，而是利用mock建立一套本地API，在network中可以看到网络请求。
3. npm run serve为传统模式，使用服务器api，需配合proxy解决跨域问题。


# 支持apidoc
使用npm run apidoc创建接口文档，通过/apidoc访问文档，不支持serverless模式下测试接口