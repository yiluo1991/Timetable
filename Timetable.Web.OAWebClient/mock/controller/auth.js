export default [
    {
        type: "post",
        url: "/auth/getPermissionMenu",
         /**
         * 
         * @api {post} /auth/getPermissionMenu 获取权限菜单
         * @apiName 获取权限菜单
         * @apiGroup 权限接口
         * @apiVersion  1.0.0
         */
        response: (req, res) => {
            return {
                success: true,
                data: {
                    logined:true,
                  
                    menu: [
                        {
                            Url: "/admin",
                            Name: "首页",
                            Icon: "el-icon-s-home",
                            Id: 0,
                            Children: [  ]
                        },
                        {
                            Url: "",
                            Name: "权限和账号管理",
                            Icon: "el-icon-user",
                            Id: 1,
                            Children: [
                                {
                                    Url: "/admin/permissiongroup",
                                    Name: "权限页管理",
                                    Icon: null,
                                    Id: 2,
                                },
                                {
                                    Url: "/admin/permissionline",
                                    Name: "页详细权限项管理",
                                    Icon: null,
                                    Id: 3,
                                },
                                {
                                    Url: "/admin/role",
                                    Name: "角色管理",
                                    Icon: null,
                                    Id: 4,
                                },
                                {
                                    Url: "/admin/employee",
                                    Name: "管理员账号",
                                    Icon: null,
                                    Id: 5,
                                }
                            ]
                        }
                    ],
                    pages: [
                        "/admin",
                      
                        "/admin/employee",
                        "/admin/permissiongroup",
                        "/admin/permissionline",
                        "/admin/role"
                    ],
                    permissionline: [

                    ]
                }
            }
        }
    }
]