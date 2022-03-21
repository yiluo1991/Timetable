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

    response: () => {
      return {
        success: true,
        data: {
          logined: true,
          menu: [
            { Url: "/admin", Name: "首页", Icon: "el-icon-s-home", Id: 2, Children: [] },
          
            {
              Url: "",
              Name: "后台权限管理",
              Icon: "el-icon-user",
              Id: 3,
              Children: [
                { Url: "/admin/permissiongroup", Name: "权限页管理", Icon: null, Id: 6 },
                { Url: "/admin/permissionline", Name: "页详细权限项管理", Icon: null, Id: 7 },
                { Url: "/admin/role", Name: "角色权限", Icon: null, Id: 8 },
                { Url: "/admin/employee", Name: "管理员账号", Icon: null, Id: 9 },
              ],
            },
            {
                Url: "",
                Name: "其他",
                Icon: "el-icon-s-order",
                Id: 4,
                Children: [
                  { Url: "/admin/404", Name: "404错误页", Icon: null, Id: 10 },
                  { Url: "/admin/catch", Name: "异常捕获", Icon: null, Id: 11 }
                ],
              }
            
          
          ],
          pages: [
            "/admin",
            "/admin/permissiongroup",
            "/admin/permissionline",
            "/admin/role",
            "/admin/employee"
          ],
          permissionline: [
            "PG_G",
            "EL_G",
            "E_RG",
            "E_R",
            "E_G",
            "R_D",
            "R_M",
            "R_A",
            "R_G",
            "PL_PGG",
            "PL_D",
            "PL_M",
            "PL_A",
            "PL_G",
            "PG_D",
            "PG_M",
            "PG_A",
            "R_PLG",
            "SC_G",
            "SC_D",
            "SC_M",
            "SC_A",
            "SC_V",
            "SC_GP",
            "AS_G",
            "AS_I",
            "AS_A",
            "SV_G",
            "SV_I",
            "SV_D",
            "U_RA",
            "SV_MA",
            "SV_MR",
            "T_D",
            "TO_F",
            "TO_G",
            "TO_I",
            "TP_G",
            "C_P",
            "H",
            "TP_I",
            "E_I",
            "E_A",
            "TP_M",
            "E_M",
            "E_V",
            "E_D",
            "U_G",
            "U_A",
            "U_M",
            "U_R",
            "U_V",
            "UL_G",
            "T_G",
            "T_M",
            "T_A",
            "T_V",
            "TP_D",
            "TP_V",
            "TP_GS",
            "TP_QA",
            "SP_G",
            "SP_A",
            "SP_M",
            "SP_D",
            "UG_G",
            "UG_A",
            "UG_M",
            "UG_D",
            "PR_G",
            "PR_C",
            "PR_M",
            "PR_U",
            "PR_GM",
            "U_UGG",
            "SV_SPL",
          ],
        },
        msg: "请求成功",
      };
    },
  },
];
