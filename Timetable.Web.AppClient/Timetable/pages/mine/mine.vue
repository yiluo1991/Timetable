<template>
	<view>
		<cu-custom bgColor="bg-white">
			<template v-slot:content>用户中心</template>
		</cu-custom>
		<view class="cu-card   bg-white margin-top padding userinfo">
			<view class="cu-avatar radius  xl head"></view>
			<view class="info">
				<view class="username text-lg  padding-tb-xs">{{userInfo.RealName}}</view>
				<view class="text-gray">{{userInfo.Type=="T"?"(教师)":userInfo.Type=="S"?"(学生)":""}}</view>
			</view>
			<view class="idcode text-gray text-sm">
				{{userInfo.Type=="T"?"职工号：":userInfo.Type=="S"?"学号：":""}}{{userInfo.IdentityCode}}
			</view>
		</view>
		<view class="cu-list  bg-white  menu margin-top ">
			<view class="cu-item arrow" v-if="userInfo.Type" @click="$url('/pages/term/term')">
				<view class="content">
					<text class="cuIcon-calendar text-orange"></text>
					<text class="text-grey">学校校历</text>

				</view>
			</view>
			<view class="cu-item arrow" v-if="userInfo.Type" @click="$url('/pages/mycourse/mycourse')">
				<view class="content">
					<text class="cuIcon-read text-yellow"></text>
					<text class="text-grey">我的课程</text>
				</view>
			</view>
		</view>

		<view class="cu-list  bg-white  menu margin-top ">
			<view class="cu-item arrow" v-if="userInfo.Type" @click="$url('/pages/password/password')">
				<view class="content">
					<text class="cuIcon-lock text-green"></text>
					<text class="text-grey">修改密码</text>
				</view>
			</view>
			<view class="cu-item arrow" v-if="userInfo.Type" @click="$url('/pages/login/login')">
				<view class="content">
					<text class="cuIcon-exit text-red"></text>
					<text class="text-grey">重新登录</text>
					<view class="tagtip">若登录的账号不正确</view>
				</view>
			</view>

		</view>
		<button class="cu-btn bg-red margin block lg" v-if="userInfo.Type" @click="logout">解除绑定并退出登陆</button>

	</view>
</template>

<script>
	import {
		mapState,
		mapMutations
	} from "vuex";

	export default {
		onShareTimeline(){
				
			return {
				　title: "智慧课表-用户中心"
			}
		},
		onShareAppMessage(){
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			　　var shareObj = {
			　　　　title: "智慧课表-用户中心"
			　　};
			　
			　　return shareObj;
		},
		computed: {
			...mapState(['baseUrl', 'userInfo'])
		},
		data() {
			return {

			}
		},
		onShow() {
			
			this.$auth.afterLogin(() => {
				this.$auth.getUserInfo()
			})
		},
		methods: {
			...mapMutations(['set_userInfo']),
			logout() {
				this.$auth.afterLogin(() => {
					uni.showModal({
						title: "确认解绑",
						content: "确认要解除账号和微信号的绑定并退出登陆？解除绑定后下次您需要手动登陆。",
						success: (e) => {
							if (e.confirm) {
								console.log("取消绑定并退出")
								uni.login({
									success: (lres) => {
										console.log("开始绑定账号")

										uni.request({
											url: this.baseUrl +
												"/api/login/UnBindWechat",
											method: "GET",
											header: {
												token: uni.getStorageSync('token')
													.token
											},
											data: {
												code: lres.code
											},
											success: (serverRes) => {
												if (serverRes.data.success) {
													uni.removeStorageSync(
														'token');
													uni.navigateTo({
														url: '../login/login'
													});
													this.set_userInfo({
														AvatarUrl: null,
														Gender: 1,
														IdentityCode: "",
														Mobile: "",
														RealName: "",
													})
												} else {
													uni.showModal({
														content: serverRes
															.data.msg,
														title: '失败',
														showCancel: false
													})
												}
											}
										})
									},
									error() {
										console.log("调用login失败，可能网络不通畅")
									}
								})

							}
						}
					})

				})
			}
		}
	}
</script>

<style lang="less">
	.tagtip {
		padding: 10rpx 10rpx;
		border: 1px solid #f15045;
		color: #f15045;
		border-radius: 25rpx;
		margin-left: 5rpx;
		display: inline-block;
		font-size: 20rpx;
		line-height: 20rpx;
		vertical-align: middle;
	}

	.head {
		background-image: url(../../colorui/default.jpg);
		background-color: #000;
		filter: brightness(0.8) sepia(50%);

	}

	.userinfo {
		display: flex;
		align-items: center;

		.info {
			flex-grow: 1;
			padding-left: 20rpx;

		}

		.idcode {}
	}
</style>
