<template>
	<view>
		<cu-search @search="search" :placeholder="placeholder"  staticBorder bgColor="bg-white" isBack enableInput>
			<template v-slot:content>
				 课程查询
			</template> 
		</cu-search>
		<view >
			<view v-if="!keyword" class="text-gray padding  text-center " style="margin-top:200rpx;" >
				<view >
					<view style="font-size:80rpx;">
						<text  class="cuIcon-emoji "></text>
					</view>
				</view>
				<view>请输入查询关键词进行查询</view> 
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
			<cu-loading v-if="keyword" :loading="loading" :noMore="noMore"></cu-loading>
		</view>
	</view>
</template>

<script>
	import {mapState} from 'vuex';
	
	export default {
		data() {
			return {
					type:'',
					keyword:'',
					page:0,
					rows:15,
					list:[],
					noMore:false,
					loading:false
			};
		},
		onLoad(o){
			switch(o.type){
				case 'course':
				 this.type='course';
				 break;
				case 'address':
				 this.type='address';
				  break;
				case 'teacher':
				 this.type='teacher';
				  break;
				default:
				 this.type='course';
				  break;
			}
		},
		computed:{
			...mapState(['baseUrl']),
			placeholder(){
					switch(this.type){
						case 'course':
						return '请输入课程名进行查询';
						case 'address':
						return '请输入上课教室或地点进行查询';
						case 'teacher':
						return '请输入任课教师姓名进行查询';
						default:
						return ''
					}
			
			}
		},
		onReachBottom(){
			this.nextPage()
		},
		methods:{
			
			search(e){
				this.noMore=false;
				this.keyword=e;
				this.page=0;
				this.list=[];
				this.nextPage()
				if(!this.keyword){
					uni.showToast({
						title:this.placeholder,
						icon:'none'
					})
				}
			},
			nextPage(){
				if(this.keyword&&!this.noMore){
					this.loading=true;
					this.$auth.afterLogin(()=>{
						var data={};
						this.page++; 
						data.rows=this.rows;
						data[this.type]=this.keyword;
						data.page=this.page;
						uni.request({
							url:this.baseUrl+'/api/course/search',
							method:'POST',
							data:data,
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

<style lang="less">

</style>
