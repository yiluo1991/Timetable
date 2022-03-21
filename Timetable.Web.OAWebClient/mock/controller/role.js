export default [
    {
        type:"post",
        url:"/role/list",
        
        response(){
            return {
                success:true,
                data:[{Id: 1, Name: "管理员", Remark: null, LineIds: [1, 6, 7, 8, 12, 2, 3, 4, 5, 14, 15, 16, 13, 17, 9, 10]}]
            }
        }

    },{
        type:"post",
        url:"/role/add",
        response(req,res){
            return {
                success:true,
                data:[],
                msg:'添加成功'
            }
        }
    },{
        type:"post",
        url:"/role/edit",
        response(req,res){
            return {
                success:true,
                data:[],
                msg:'修改成功'
            }
        }
    }
]