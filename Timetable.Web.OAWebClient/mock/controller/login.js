
export default [
    {  
      url: "/login/userlogin",
      type: "post",  
      /**
       * 
       * @api {post} /login/userlogin 登录
       * @apiName 登录
       * @apiGroup 登录
       * @apiVersion  1.0.0
       * 
       * 
       * @apiParam  {String} username 用户名
       * @apiParam  {String} password 密码
       * 
       * @apiSuccess (200) {json} Success 服务器响应
       * 
       * @apiParamExample  {json} Request-Example:
       * {
       *     "username" : "admin",
       *     "password" : "123456"
       * }
       * 
       * 
       * @apiSuccessExample {type} Success-Response:
       * {
       *     "success" : true,
       *     "msg" : "登录成功",
       *     "data" : {
       *          "uid": 123       
       *      }
       * }
       */       
      response: () => {
        return {msg: "登录成功",success:true, data:{uid:"@integer(100, 2000)"} }; 
      },  
    }
  ];
  