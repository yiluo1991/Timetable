<template>
	<view>
		<cu-custom bgColor="bg-gradual-white" isBack>
			<template v-slot:content>
				 组合搜索
			</template> 
		</cu-custom>
		
		<view class="cu-form-group margin-top">
			<view class="title">关键词</view>
			<input placeholder="请输入课程名/教师名" v-model="keyword"></input>
		</view>
		<view class="cu-form-group ">
			<view class="title">上课教室</view>
			<input placeholder="请输入上课地点关键词" v-model="address"></input>
		</view>
		<view class="cu-form-group">
			<view class="title">院系范围</view>
			<picker mode="multiSelector" @change="MultiChange" range-key="Name" @columnchange="MultiColumnChange"  :value="multiIndex" :range="viewArray">
				<view class="picker">
					{{viewArray[1][multiIndex[1]].Name=='无限制'?viewArray[0][multiIndex[0]].Name : viewArray[1][multiIndex[1]].Name}}
				</view>
			</picker>
		</view>
		<view class="cu-form-group">
			<view class="title">日期选择</view>
			<picker mode="date" :value="date" :start="start" :end="end" @change="DateChange">
				<view class="picker">
					{{date}}
				</view>
			</picker>
		</view>
		<view class="bg-white padding">
			<button @click="search()" class="cu-btn block bg-blue lg"> <text class="cuIcon-search margin-right-xs" ></text> 查询</button>
		</view>
		 
		<view class="course"  v-for="course in list" :key="course.Id">
			<view class="coursetitle  ">
				{{course.SubjectName}}
			</view>
			<view class="courseinfo">
				<view v-if="course.DepartmentName||course.CollegeName">
					<view class="label">开课院系: </view>
					<view class="content">{{course.CollegeName}} {{course.DepartmentName}}</view> 
				</view> 
				<view v-if="course.ClassName">
					<view  class="label">授课班级: </view>
					<view class="content">{{course.ClassName}}</view>
				</view>
				<view>
					<view  class="label">授课教师: </view>
					<view class="content">{{course.RealName}}</view>
				</view>
				<view>
					<view  class="label">上课时间: </view>
					<view class="content">{{course.Desc}}</view>
				</view>
				<view v-if="course.Address">
					<view  class="label">上课地点: </view>
					<view class="content">{{course.Address}}</view>
				</view>
			</view>
		</view>
		<view v-if="showTip" class="text-gray padding  text-center " style="margin-top:200rpx;" >
			<view >
				<view style="font-size:80rpx;">
					<text  class="cuIcon-emoji "></text>
				</view>
			</view>
			<view>请输入查询信息</view> 
		</view>
		<cu-loading v-else :loading="loading" :noMore="noMore" ></cu-loading>
		 
		
	</view>
</template>

<script>
	import vue from 'vue'
	import {mapState} from 'vuex'
	export default {
		data() {
			return {
				viewArray:[],
				multiIndex: [0, 0],
				comboData:[],
				originArray:[],
				keyword:'',
				address:'',
				date:'无限制',
				start:'',
				end:'',
				page:0,
				rows:15,
				list:[],
				noMore:false,
				loading:false,
				postData:{},
				showTip:true
			}
		},
		computed:{
			...mapState(['baseUrl'])
			
		},
		created(){
			var now=new Date();
			 this.start= vue.filter('date')(now);
			now.setDate(now.getDate()+60);
			this.end=vue.filter('date')(now);
			this.$auth.afterLogin(()=>{
				uni.request({
					url:this.baseUrl+'/api/college/combodata',
					method:'get',
					header:{
						token:uni.getStorageSync('token').token
					},
					success:(res)=>{
						 res.data.data.unshift({
							 Id:0,
							 Name:'无限制'
						 })
						 this.originArray=res.data.data;
						 this.viewArray=[[...this.originArray],[ {Id:0,Name:'无限制'}]]
						 this.MultiColumnChange({detail:{column:0,value:this.multiIndex[0]}})
						 console.log(res)
					},
					fail:(res)=>{
						console.log(res)
					},
					 
				})
			})
		},
		methods: {
			canSearch(){
				 
				if(this.keyword){
					return true;
				}
				if(this.address){
					return true;
				}
				if(this.date!='无限制'){
					return true;
				}
				if(this.multiIndex[0]||this.multiIndex[1]){
					return true;
				}
				return false;
			
			},
			DateChange(e) {
				this.date = e.detail.value
			},
			MultiChange(e) {
				console.log('MultiChange',e)
				this.multiIndex = e.detail.value
			},
			MultiColumnChange(e) {
			
				
				var value=  e.detail.value;
				switch (e.detail.column) {
					case 0:
						if(this.originArray[value].children&&this.originArray[value].children.length>0){
							this.viewArray.splice(1,1,[
								 {Id:0,Name:'无限制'},
								 ...this.originArray[value].children
							]);
						}else{
							this.viewArray.splice(1,1,[
								 {Id:0,Name:'无限制'}
							]);
						}
					
				}
				 
			},
			search(e){
				
					this.noMore=false;
					this.page=0;
					this.list=[];
					
					if(!this.canSearch()){
						uni.showToast({
							title:'请至少输入一项查询信息',
							icon:'none'
						})
						this.showTip=true;
					}else{
						this.showTip=false;
						var data={};
						data.rows=this.rows; 
						if(this.keyword){
							data.keyword=this.keyword;
						}
						if(this.address){
							data.address=this.address
						}
						if(this.date!='无限制'){
							data.date=this.date;
						}
						if(this.multiIndex[0]){
							
							data.collegeid=this.viewArray[0][this.multiIndex[0]].Id;
						}
						if(this.multiIndex[1])
						{
							data.departmentid=this.viewArray[1][this.multiIndex[1]].Id;
						}
						this.postData=data;
						this.nextPage()
					}
				},
				nextPage(){
					if(this.canSearch()&&!this.noMore){
						this.loading=true;
						this.$auth.afterLogin(()=>{
							
							this.page++; 
							
							
							uni.request({
								url:this.baseUrl+'/api/course/search',
								method:'POST',
								data:{
									...this.postData,
									page:this.page,
									
								},
								header:{
									token:uni.getStorageSync('token').token
								},
								success:(res)=>{
									if(res.data.success){
										this.loading=false;
										if(res.data.data.length<this.rows){
											this.noMore=true;
										}
										this.list.push.apply(this.list,res.data.data);
									}else{
										uni.showToast({
											title:res.data.msg,
											icon:'none'
										})
									}
								}
							})
						})
					}
					
				}
			
		}
	}
</script>

<style>

</style>
