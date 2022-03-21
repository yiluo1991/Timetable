export default [
    {
        type:"post",
        url:"/user/getuserinfo",
         /**
         * 
         * @api {post} /user/getuserinfo 获取登录用户信息
         * @apiName 获取登录用户信息
         * @apiGroup 权限接口
         * @apiVersion  1.0.0
         */
        response(){
            return {
                success:true,
                data:{
                    Headshot:"http://up.enterdesk.com/edpic/31/c3/fd/31c3fdc63511cabedd6415d121fa2d58.jpg",
                    RealName:"哈哈管理员"
                }
            }
        }

    }
]