<template>
	<view>
		<cu-custom bgColor="bg-white"> 
			<template v-slot:content>
				 智慧课表
			</template> 
		</cu-custom>
		<view class="content">
			<view class="bg-white" style="width:100%;">
				<view class="grid text-center col-4">
					<view class="padding " @click="$url('/pages/week2/week2')">
						<text class=" cuIcon-form text-green" style="font-size:70rpx;"></text>
						<view class="text-grey">本周课表</view>
					</view>
					<view class="padding" @click="$url('/pages/term/term')">
						<text class=" cuIcon-calendar text-orange" style="font-size:70rpx;"></text>
						<view class="text-grey">学校校历</view>
					</view>
					<view class="padding" @click="$url('/pages/combosearch/combosearch')">
						<text class=" cuIcon-searchlist text-blue" style="font-size:70rpx;"></text>
						<view class="text-grey">课程查询</view>
					</view>
					<view class="padding" @click="$url('/pages/noticelist/noticelist')">
						<text class=" cuIcon-new text-red" style="font-size:70rpx;"></text>
						<view class="text-grey">通知公告</view>
					</view>
				</view>
			</view>
			<view style="width:100%">
				<view class="cu-bar bg-white solid-bottom " >
					<view class="action">
						<text class="cuIcon-time text-orange"></text> 今日课表
					</view>
				</view>
				<view class="bg-white solid-bottom padding  text-center" style="width:100%">
					 
					<cu-coursedate :courses="courseDate"></cu-coursedate>
				</view>
				<view class="bg-white solid-bottom padding-sm text-center " style="width:100%">
					<button class="cu-btn line-cyan " @click="$url('/pages/week/week')">查看本周课程</button>
				</view>
			</view>
			<view class="bg-white margin-top" style="width:100%;">
				<view class="cu-bar bg-white solid-bottom " >
					<view class="action">
						<text class="cuIcon-file text-cyan"></text> 教学管理
					</view>
				</view>
				<view class="grid cu-list text-center col-2">
					<view class="padding cu-item" @click="$url('/pages/mycourse/mycourse')" >
						<text class=" cuIcon-read text-grey padding-tb-xs" style="font-size:50rpx;"></text>
						<view class="text-grey">我的课程</view>
					</view>
					 
					<view class="padding cu-item" @click="$url('/pages/week/week')" >
						<text class=" cuIcon-list text-green padding-tb-xs" style="font-size:50rpx;"></text>
						<view class="text-grey">本周课程</view>
					</view>
					
				</view>
			</view>
			<view class="bg-white margin-top" style="width:100%;">
				<view class="cu-bar bg-white solid-bottom " >
					<view class="action">
						<text class="cuIcon-attention text-green"></text> 全校课程
					</view>
				</view>
				<view class="grid cu-list text-center col-2">
					<view class="padding cu-item" @click="$url('/pages/search/search?type=course')" >
						<text class=" cuIcon-search text-grey padding-tb-xs" style="font-size:50rpx;"></text>
						<view class="text-grey">搜索课程</view>
					</view>
					<view class="padding cu-item" @click="$url('/pages/search/search?type=address')" >
						<text class=" cuIcon-location text-grey padding-tb-xs" style="font-size:50rpx;"></text>
						<view class="text-grey">按教室查</view>
					</view>
					<view class="padding cu-item" @click="$url('/pages/search/search?type=teacher')" >
						<text class=" cuIcon-friendfill text-grey padding-tb-xs" style="font-size:50rpx;"></text>
						<view class="text-grey">按教师查</view>
					</view>
					<view class="padding cu-item" @click="$url('/pages/combosearch/combosearch')">
						<text class=" cuIcon-same text-grey padding-tb-xs" style="font-size:50rpx;"></text>
						<view class="text-grey">组合查询</view>
					</view>
				</view>
				
			</view>
			<view class="bg-white margin-top" style="width:100%;">
				<view class="cu-bar bg-white solid-bottom " >
					<view class="action">
						<text class="cuIcon-selection text-yellow"></text> 辅助功能
					</view>
				</view>
				<view class="grid cu-list text-center col-2">
					<view class="padding cu-item"  @click="$url('/pages/paperlist/paperlist')">
						<text class=" cuIcon-emoji text-grey padding-tb-xs" style="font-size:50rpx;"></text>
						<view class="text-grey">问卷调查</view>
					</view>
				</view>
				
			</view>
			<view style="width:100%;" class="padding text-center">
				<text class="  text-sm" style="color:#ccc">励航软件 &copy; 2022 智慧课表 V1.0</text>
			</view>
		
		</view>
	</view>
</template>

<script>
	import vue from 'vue';
	import {mapState} from 'vuex'
	export default {
		onShareAppMessage(){
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			　　var shareObj = {
			　　　　title: "智慧课表"
			　　};
			　
			　　return shareObj;
		},
		onShareTimeline(){
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			　　var shareObj = {
			　　　　title: "智慧课表"
			　　};
			　
			　　return shareObj;
		},
		data() {
			return {
				title: 'Hello',
				courseDate:undefined,
				today:vue.filter('sortdate')(new Date())
			}
		},
		 computed:{
		 	...mapState(['baseUrl','userInfo']),
		 },
		onLoad() {
			
		},
		onShow() {
			this.$auth.afterLogin(()=>{ 
				if(!this.userInfo.IdentityCode){
					this.$auth.getUserInfo()
				}
				uni.request({
					url:this.baseUrl+'/api/course/minedate',
					method:'GET',
					data:{date:this.today},
					header:{
						token:uni.getStorageSync('token').token
					},
					success:(res)=>{
						if(res.data.success){
							res.data.data.forEach(s=>{
								s.TimeBook= JSON.parse(s.TimeBook)
							})
							this.courseDate=res.data.data
						}else{
							uni.showToast({
								title:res.data.msg,
								icon:'none',
								mask:true
							})
						}
					}
				})
				
			})
		},
		methods: {

		}
	} 
</script>

<style>
	.content {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
	}

	.logo {
		height: 200rpx;
		width: 200rpx;
		margin-top: 200rpx;
		margin-left: auto;
		margin-right: auto;
		margin-bottom: 50rpx;
	}

	.text-area {
		display: flex;
		justify-content: center;
	}

	.title {
		font-size: 36rpx;
		color: #8f8f94;
	}
</style>
