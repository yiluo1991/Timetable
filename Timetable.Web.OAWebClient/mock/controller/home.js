export default [
    {
        type:"post",
        url:"/LogManager/GetIpLogs",
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
                data: [{Code: "CN", name: "China", Name_CN: "中国", VisitorCount: 19715}
               , {Code: "CN", name: "福建", Name_CN: "福建", VisitorCount: 8232}
               , {Code: "CN", name: "北京", Name_CN: "北京", VisitorCount: 5055}
               , {Code: "CN", name: "广东", Name_CN: "广东", VisitorCount: 1299}
               , {Code: "CN", name: "浙江", Name_CN: "浙江", VisitorCount: 644}
               , {Code: "US", name: "United States", Name_CN: "美国", VisitorCount: 620}
               , {Code: "CN", name: "上海", Name_CN: "上海", VisitorCount: 494}
               , {Code: "CN", name: "河北", Name_CN: "河北", VisitorCount: 472}
               , {Code: "CN", name: "江苏", Name_CN: "江苏", VisitorCount: 229}
               , {Code: "CN", name: "台湾", Name_CN: "台湾", VisitorCount: 147}
               , {Code: "CN", name: "河南", Name_CN: "河南", VisitorCount: 112}
               , {Code: "CN", name: "湖南", Name_CN: "湖南", VisitorCount: 111}
               , {Code: "CN", name: "江西", Name_CN: "江西", VisitorCount: 103}
               , {Code: "CN", name: "陕西", Name_CN: "陕西", VisitorCount: 92}
               , {Code: "CN", name: "山东", Name_CN: "山东", VisitorCount: 80}
               , {Code: "CN", name: "四川", Name_CN: "四川", VisitorCount: 73}
               , {Code: "CN", name: "天津", Name_CN: "天津", VisitorCount: 73}
               , {Code: "CN", name: "湖北", Name_CN: "湖北", VisitorCount: 70}
               , {Code: "CN", name: "安徽", Name_CN: "安徽", VisitorCount: 65}
               , {Code: "CN", name: "重庆", Name_CN: "重庆", VisitorCount: 60}
               , {Code: "CN", name: "吉林", Name_CN: "吉林", VisitorCount: 56}
               , {Code: "CN", name: "香港", Name_CN: "香港", VisitorCount: 49}
               , {Code: "CN", name: "山西", Name_CN: "山西", VisitorCount: 48}
               , {Code: "CN", name: "贵州", Name_CN: "贵州", VisitorCount: 36}
               , {Code: "CN", name: "广西", Name_CN: "广西", VisitorCount: 34}
               , {Code: "CN", name: "黑龙江", Name_CN: "黑龙江", VisitorCount: 31}
               , {Code: "CN", name: "甘肃", Name_CN: "甘肃", VisitorCount: 27}
               , {Code: "CN", name: "辽宁", Name_CN: "辽宁", VisitorCount: 25}
               , {Code: "CN", name: "内蒙古", Name_CN: "内蒙古", VisitorCount: 24}
               , {Code: "CN", name: "新疆", Name_CN: "新疆", VisitorCount: 24}
               , {Code: "CN", name: "云南", Name_CN: "云南", VisitorCount: 22}
               , {Code: "CN", name: "宁夏", Name_CN: "宁夏", VisitorCount: 20}
               , {Code: "JP", name: "Japan", Name_CN: "日本", VisitorCount: 11}
               , {Code: "CN", name: "海南", Name_CN: "海南", VisitorCount: 7}
               , {Code: "RU", name: "Russia", Name_CN: "俄罗斯", VisitorCount: 7}
               , {Code: "DE", name: "Germany", Name_CN: "德国", VisitorCount: 4}
               , {Code: "NL", name: "Netherlands", Name_CN: "荷兰", VisitorCount: 4}
               , {Code: "FR", name: "France", Name_CN: "法国", VisitorCount: 3}
               , {Code: "IT", name: "Italy", Name_CN: "意大利", VisitorCount: 3}
               , {Code: "CN", name: "青海", Name_CN: "青海", VisitorCount: 3}
               , {Code: "CN", name: "澳门", Name_CN: "澳门", VisitorCount: 3}
               , {Code: "CN", name: "西藏", Name_CN: "西藏", VisitorCount: 3}
               , {Code: "AU", name: "Australia", Name_CN: "澳大利亚", VisitorCount: 2}
               , {Code: "CA", name: "Canada", Name_CN: "加拿大", VisitorCount: 2}
               , {Code: "GB", name: "United Kingdom", Name_CN: "大不列颠联合王国", VisitorCount: 2}
               , {Code: "BR", name: "Brazil", Name_CN: "巴西", VisitorCount: 2}
               , {Code: "BE", name: "Belgium", Name_CN: "比利时", VisitorCount: 1}
               , {Code: "CH", name: "Switzerland", Name_CN: "瑞士", VisitorCount: 1}
               , {Code: "CH", name: "Switzerland", Name_CN: "瑞士", VisitorCount: 1}
               , {Code: "FI", name: "Finland", Name_CN: "芬兰", VisitorCount: 1}
               , {Code: "NO", name: "Norway", Name_CN: "挪威", VisitorCount: 1}
               , {Code: "NP", name: "Nepal", Name_CN: "尼泊尔", VisitorCount: 1}
               , {Code: "NZ", name: "New Zealand", Name_CN: "新西兰", VisitorCount: 1}
               , {Code: "KR", name: "Korea", Name_CN: "韩国", VisitorCount: 1}
               , {Code: "SA", name: "Saudi Arabia", Name_CN: "沙特阿拉伯", VisitorCount: 1}
               , {Code: "TH", name: "Thailand", Name_CN: "泰国", VisitorCount: 1}
               , {Code: "MY", name: "Malaysia", Name_CN: "马来西亚", VisitorCount: 1}
               , {Code: "ZA", name: "South Africa", Name_CN: "南非", VisitorCount: 1}
               , {Code: "ZM", name: "Zambia", Name_CN: "赞比亚", VisitorCount: 0}
               , {Code: "ZW", name: "Zimbabwe", Name_CN: "津巴布韦", VisitorCount: 0}
               , {Code: null, name: "austria", Name_CN: "奥地利", VisitorCount: 0}
               , {Code: "NA", name: "Namibia", Name_CN: "纳米比亚", VisitorCount: 0}
               , {Code: "NC", name: "New Caledonia", Name_CN: "新喀里多尼亚", VisitorCount: 0}
               , {Code: "NE", name: "Niger", Name_CN: "尼日尔", VisitorCount: 0}
               , {Code: "NG", name: "Nigeria", Name_CN: "尼日利亚", VisitorCount: 0}
               , {Code: "NI", name: "Nicaragua", Name_CN: "尼加拉瓜", VisitorCount: 0}
               , {Code: "RW", name: "Rwanda", Name_CN: "卢旺达", VisitorCount: 0}
               , {Code: null, name: "W. Sahara", Name_CN: "西撒哈拉", VisitorCount: 0}
               , {Code: "TJ", name: "Tajikistan", Name_CN: "塔吉克斯坦", VisitorCount: 0}
               , {Code: "TM", name: "Turkmenistan", Name_CN: "土库曼斯坦", VisitorCount: 0}
               , {Code: "TL", name: "Timor-Leste", Name_CN: "东帝汶", VisitorCount: 0}
               , {Code: "TT", name: "Trinidad and Tobago", Name_CN: "特立尼达和多巴哥", VisitorCount: 0}
               , {Code: "TN", name: "Tunisia", Name_CN: "突尼斯", VisitorCount: 0}
               , {Code: "TR", name: "Turkey", Name_CN: "土耳其", VisitorCount: 0}
               , {Code: "TZ", name: "Tanzania", Name_CN: "坦桑尼亚联合共和国", VisitorCount: 0}
               , {Code: "UG", name: "Uganda", Name_CN: "乌干达", VisitorCount: 0}]
            }
        }
    }
]