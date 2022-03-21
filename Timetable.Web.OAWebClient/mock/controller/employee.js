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
        response(req,res){
            return {
                success:true,
                data:[],
                msg:'添加成功'
            }
        }
    },{
        type:"post",
        url:"/employee/edit",
        response(req,res){
            return {
                success:true,
                data:[],
                msg:'修改成功'
            }
        }
    },{
        type:"post",
        url:"/employee/resetPassword",
        response(req,res){
            return {
                success:true,
                data:[],
                msg:'重置成功'
            }
        }
    },{
        type:"post",
        url:"/employee/checkLoginName",
        response(req,res){
            return {
                success:true,
                data:[],
                msg:''
            }
        }
    }
]