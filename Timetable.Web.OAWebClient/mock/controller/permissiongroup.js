export default [
    {
        type:"post",
        url:"/permissiongroup/list",
         /**
         * 
         * @api {post} /permissiongroup/list 获取权限页列表
         * @apiName 获取权限页列表
         * @apiGroup 权限页管理
         * @apiVersion  1.0.0
         */
        response(req,res){
            return {
                success:true,
                data:[{"Id":1,"DisplayName":"首页","ParentId":null,"Url":"/Home/Index","Tag":null,"Headshot":"el-icon-s-home","SN":0,"children":[]},{"Id":7,"DisplayName":"基础设置","ParentId":null,"Url":null,"Tag":null,"Headshot":"el-icon-receiving","SN":0,"children":[{"Id":8,"DisplayName":"走马灯管理","ParentId":7,"Url":"/Banner/Index","Tag":null,"Headshot":null,"SN":3,"children":[]},{"Id":9,"DisplayName":"友情链接管理","ParentId":7,"Url":"/FriendlyLink/Index","Tag":null,"Headshot":null,"SN":4,"children":[]},{"Id":10,"DisplayName":"登录日志","ParentId":7,"Url":"/LogManager/Index","Tag":null,"Headshot":null,"SN":5,"children":[]}]},{"Id":14,"DisplayName":"BSSP设置","ParentId":null,"Url":null,"Tag":null,"Headshot":"el-icon-setting","SN":2,"children":[{"Id":15,"DisplayName":"运算服务节点管理","ParentId":14,"Url":"/ServicePoint/Index","Tag":null,"Headshot":null,"SN":1,"children":[]}]},{"Id":2,"DisplayName":"账号和权限管理","ParentId":null,"Url":null,"Tag":null,"Headshot":"el-icon-user","SN":3,"children":[{"Id":3,"DisplayName":"权限组管理","ParentId":2,"Url":"/PermissionGroup/Index","Tag":null,"Headshot":null,"SN":1,"children":[]},{"Id":4,"DisplayName":"权限项管理","ParentId":2,"Url":"/PermissionLine/Index","Tag":null,"Headshot":null,"SN":2,"children":[]},{"Id":5,"DisplayName":"角色权限","ParentId":2,"Url":"/Role/Index","Tag":null,"Headshot":null,"SN":3,"children":[]},{"Id":6,"DisplayName":"账号管理","ParentId":2,"Url":"/Employee/Index","Tag":null,"Headshot":null,"SN":4,"children":[]}]},{"Id":16,"DisplayName":"问卷管理","ParentId":null,"Url":null,"Tag":null,"Headshot":"el-icon-document-checked","SN":4,"children":[{"Id":18,"DisplayName":"问卷列表","ParentId":16,"Url":"/Paper/Index","Tag":null,"Headshot":null,"SN":1,"children":[]},{"Id":19,"DisplayName":"问卷题目管理","ParentId":16,"Url":"/Question/Index","Tag":null,"Headshot":null,"SN":2,"children":[]},{"Id":20,"DisplayName":"问卷查阅","ParentId":16,"Url":"/Paper/History","Tag":null,"Headshot":null,"SN":3,"children":[]},{"Id":17,"DisplayName":"题目分类管理","ParentId":16,"Url":"/PaperQuestionGroup/Index","Tag":null,"Headshot":null,"SN":4,"children":[]},{"Id":21,"DisplayName":"问卷分类管理","ParentId":16,"Url":"/PaperCategory/Index","Tag":null,"Headshot":null,"SN":100,"children":[]}]},{"Id":11,"DisplayName":"频道和文章管理","ParentId":null,"Url":null,"Tag":null,"Headshot":"el-icon-notebook-1","SN":6,"children":[{"Id":12,"DisplayName":"频道分类管理","ParentId":11,"Url":"/Channel/Index","Tag":null,"Headshot":null,"SN":1,"children":[]},{"Id":13,"DisplayName":"文章管理","ParentId":11,"Url":"/Article/Index","Tag":null,"Headshot":null,"SN":2,"children":[]}]}]
            }
        }
    },{
        type:"post",
        url:"/permissiongroup/add",
          /**
         * 
         * @api {post} /permissiongroup/add 添加权限页
         * @apiName 添加权限页
         * @apiGroup 权限页管理
         * @apiVersion  1.0.0
         */
        response(req,res){
            return {
                success:true,
                data:[],
                msg:'添加成功'
            }
        }
    },{
        type:"post",
        url:"/permissiongroup/edit",
          /**
         * 
         * @api {post} /permissiongroup/add 添加权限页
         * @apiName 添加权限页
         * @apiGroup 权限页管理
         * @apiVersion  1.0.0
         */
        response(req,res){
            return {
                success:true,
                data:[],
                msg:'修改成功'
            }
        }
    }
]