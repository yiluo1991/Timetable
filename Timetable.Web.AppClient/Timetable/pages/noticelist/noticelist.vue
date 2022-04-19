<template>
	<view>
		<cu-custom bgColor="bg-gradual-white"  isBack>
			<template v-slot:content>通知公告</template>
		</cu-custom>
		<view>
		    <view class="notice" v-for="notice in list" :key="notice.Id">
				<view class="noticetitle  ">
					{{notice.Title}}
				</view>
				<view class="noticecontent">{{notice.Content}}</view>
			</view>
		</view> 
		<cu-loading :loading="loading" :noMore="noMore"></cu-loading>
	</view>
</template>

<script>
	import {mapState} from 'vuex'
	export default {
		onShareAppMessage(){
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			　　var shareObj = {
			　　　　title: "智慧课表-通知公告"
			　　};
			　
			　　return shareObj;
		},
		onShareTimeline(){
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			　　var shareObj = {
			　　　　title: "智慧课表-通知公告"
			　　};
			　
			　　return shareObj;
		},
		data() {
			return {
				page:0,
				list:[],
				loading:false,
				noMore:false
			};
		},
		computed:{
			...mapState(['baseUrl'])
		},
		created(){
			this.nextPage()
		},
		onReachBottom(){
			this.nextPage()
		},
		methods:{
			nextPage(){
				if(!this.noMore&&!this.loading){
					this.page++;
					this.loading=true;
					uni.request({
						url:this.baseUrl+'/api/notice/list',
						method:'POST',
						data:{
							page:this.page,
							rows:15
						},
						success:res=>{ 
							if(res.data.data.length<15){
								this.noMore=true; 
							
							}
								this.list.push.apply(this.list,res.data.data);
							
							this.$nextTick(()=>{
								this.loading=false;
							})
						}
					})
				}
				
			}
		}
	}
</script>

<style lang="less">
	.notice{
		margin:20rpx; 
		color:#666;
		background: #fff;
		padding: 20rpx;
		border-radius: 15rpx;
		.noticetitle{
			font-weight: bold;
			
			margin:20rpx 0;
			color:#000;
		}
		.noticecontent{
			 white-space: pre-wrap;
			 padding-bottom:10rpx;
		}
	}
</style>
