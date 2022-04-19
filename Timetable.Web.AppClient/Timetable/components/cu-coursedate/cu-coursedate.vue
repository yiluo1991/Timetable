<template>
	<view> 
			<view class="course mini " v-for="course in viewData" :key="course.Id">
				 
				<view class="courseinfo ">
					<view class="coursetitle" >
						{{course.SubjectName}}
					</view>
					<view v-if="userType=='S'">
						<view  class="label">授课教师: </view>
						<view class="content">{{course.RealName}}</view>
					</view> 
					<view v-if="userType=='T'&&course.ClassName">
						<view  class="label">授课班级: </view>
						<view class="content">{{course.ClassName}}</view>
					</view> 
					<view >
						<view  class="label">时间地点: </view>
						<view class="content">{{course.StartIndex+1}}-{{course.StartIndex+course.Length}}节， {{course.Address}} </view>
					</view>
				</view>
			</view>
			<view class="text-gray text-center padding" v-if="courses==null">加载中</view>
			<view class="text-gray text-center padding" v-else-if="courses.length==0">今日无课程</view>
		
		
	</view>
</template>

<script>
	import {
		mapState,
		 
	} from "vuex";
	export default{
		 
		props:['courses'],
		created(){
			 
			 
		},
		computed:{
			...mapState(['userInfo','baseUrl']),
			userType(){
				if(this.userInfo.IdentityCode){
					return this.userInfo.Type;
				}else{
					return ""
				}
			},
			viewData(){
				if(this.courses){
					var data=[];
					for(var i=0;i<12;i++){
						data[i]=0;
					}
					for(var course of this.courses){
						for(var i =0;i<12;i++){
							if(course.TimeBook.indexOf(i+1)>-1){
								data[i]= {
									...course
								}
								data[i].StartIndex=i;
								data[i].Length=1;
							}
						}
					}
					for(var i=11;i>=0;i--){
						if(data[i]===0){
							data.splice(i,1);
						}else{
							if(i>0&& (data[i-1]!==0)){
								if(data[i].Id==data[i-1].Id){
									data[i-1].Length+=data[i].Length;
									data.splice(i,1);
								}
								
							}
						}
					}
					return data;
				}else{
					return [];
				}
				
			}
		},
		data(){
			return {}
		}
	}
</script>

<style lang="less">
	.course.mini{
		.courseinfo{ 
			.coursetitle{
				margin:0rpx 10rpx; 
				color:#000;
			}
			margin:0;
			
		}
		margin-bottom:10rpx;
		border:1px solid #eee;
	}
	
	
</style>
