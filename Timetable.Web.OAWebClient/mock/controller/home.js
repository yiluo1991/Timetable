export default [
    {
        type:"post",
        url:"/home/info",
         /**
         * 
         * @api {post} /permissiongroup/list 获取权限页列表
         * @apiName 获取权限页列表
         * @apiGroup 权限页管理
         * @apiVersion  1.0.0
         */
        response(){
            return {"success":false,"data":{"LoginLogSummary":{"Total":162,"Summary30":[{"Count":1,"Date":"2021-03-26T00:00:00"},{"Count":2,"Date":"2021-03-29T00:00:00"},{"Count":2,"Date":"2021-03-30T00:00:00"},{"Count":2,"Date":"2021-03-31T00:00:00"},{"Count":2,"Date":"2021-04-01T00:00:00"},{"Count":1,"Date":"2021-04-12T00:00:00"},{"Count":1,"Date":"2021-04-13T00:00:00"},{"Count":1,"Date":"2021-04-14T00:00:00"},{"Count":2,"Date":"2021-04-15T00:00:00"},{"Count":3,"Date":"2021-04-16T00:00:00"},{"Count":3,"Date":"2021-04-19T00:00:00"},{"Count":1,"Date":"2021-04-20T00:00:00"},{"Count":1,"Date":"2021-04-22T00:00:00"},{"Count":1,"Date":"2021-04-23T00:00:00"},{"Count":2,"Date":"2021-04-25T00:00:00"},{"Count":1,"Date":"2021-04-26T00:00:00"}]},"FeedbackSummary":{"Total":6,"Unfinished":0},"AuditingSummary":{"AuditingCount":0,"AuditedCount":5},"SubjectSummary":{"Total":10,"Runing":5,"Summary30":[{"Date":"2021-03-26T00:00:00","Count":2},{"Date":"2021-04-20T00:00:00","Count":1},{"Date":"2021-04-23T00:00:00","Count":2}]},"TemplateSummary":{"AuditedCount":2,"AuditingCount":0}},"msg":null}
        }
    }
]