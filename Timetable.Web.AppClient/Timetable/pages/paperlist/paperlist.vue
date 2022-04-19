<template>
	<view>
		<cu-custom bgColor="bg-gradual-white"  isBack>
			<template v-slot:content>问卷调查</template>
		</cu-custom>
		<view>
		    <view class="paper" v-for="notice in list" :key="notice.Id" @click="goto(notice)">
				<view class="papertitle  ">
					{{notice.Title}}
				</view>
				 
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
			　　　　title: "智慧课表-问卷调查"
			　　};
			　
			　　return shareObj;
		},
		onShareTimeline(){
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			　　var shareObj = {
			　　　　title: "智慧课表-问卷调查"
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
			goto(paper){
				uni.navigateTo({
					url:'../web/web?url='+encodeURIComponent(paper.Url),
					
				})
			},
			nextPage(){
				if(!this.noMore&&!this.loading){
					this.page++;
					this.loading=true;
					uni.request({
						url:this.baseUrl+'/api/paper/list',
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
	.paper{
		margin:20rpx; 
		color:#666;
		background: #fff;
		padding: 20rpx;
		border-radius: 15rpx;
		align-items:center ;
		align-content: center;
		display: flex;
		.papertitle{ 
			margin:20rpx 0;
			flex-grow: 1;
		}
		&::after{
			content: '\e6a3';
			font-family: "cuIcon";
			font-size: inherit;
			font-style: normal;
			
		}
		 
	}
</style>
