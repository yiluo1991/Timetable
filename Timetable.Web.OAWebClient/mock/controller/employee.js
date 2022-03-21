export default [
    {
        type:"post",
        url:"/employee/list",
        
        response(){
            return {
                success:true,
                data:[{
                    Address: null,
                    CreateTime: "/Date(1593566964467)/",
                    Creator: "管理员",
                    Email: "a@123.com",
                    EmployeeRoleIds: [1],
                    EmployeeRoleNames: ["管理员"],
                    Enable: true,
                    Headshot: null,
                    Id: 1,
                    LoginName: "admin",
                    Mobile: null,
                    RealName: "管理员"
                }],
                total:1
            }
        }

    },{
        type:"post",
        url:"/employee/add",
        response(){
            return {
                success:true,
                data:[],
                msg:'添加成功'
            }
        }
    },{
        type:"post",
        url:"/employee/edit",
        response(){
            return {
                success:true,
                data:[],
                msg:'修改成功'
            }
        }
    },{
        type:"post",
        url:"/employee/resetPassword",
        response(){
            return {
                success:true,
                data:[],
                msg:'重置成功'
            }
        }
    },{
        type:"post",
        url:"/employee/checkLoginName",
        response(){
            return {
                success:true,
                data:[],
                msg:''
            }
        }
    },{
        type:'post', 
        url:'/employee/getemployeeinfo',
        response(){
            return {"success":true,"data":{"Headshot":"http://up.enterdesk.com/edpic/31/c3/fd/31c3fdc63511cabedd6415d121fa2d58.jpg","Mobile":"18559819573","Email":"sgliyang@126.com","Address":null,"RealName":"李小阳","CreateTime":"2020-09-27T13:28:03.993886","Roles":["超级管理员"]},"msg":null}
        }
    }
]