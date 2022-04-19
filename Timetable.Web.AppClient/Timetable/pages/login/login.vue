<template>
	<view>
		<cu-custom bgColor="bg-gradual-white" isBack>
			<template v-slot:content>登录课表平台</template>
		</cu-custom>
		<view>
			<view class="cu-bar bg-white">
				<view class="action">
					<text class="cuIcon-titles text-green"></text>
					<text class="text-gray">登录遇到问题请咨询：0592 xxxxxxxx</text>
				</view>
			</view>
			<view class="bg-white padding">
				<view class="cu-steps">
					<view class="cu-item" :class="index>num?'':'text-blue'" v-for="(item,index) in numList"
						:key="index">
						<text class="num" :data-index="index + 1"></text> {{item.name}}
					</view>
				</view>
			</view>
			<view>
				<radio-group v-if="n==0" class="block " @change="RadioChange">
					<view class="cu-form-group  margin-top">
						<view class="title"><text class="text-cyan cuIcon-my" style="margin-right:5px"></text>
							使用账号密码登录通道</view>
						<radio class="radio blue" :class="radio=='B'?'checked':''" :checked="radio=='B'?true:false"
							value="B"></radio>
					</view>
					<view class="cu-form-group">
						<view class="title">
							<text class="text-green  cuIcon-profile" style="margin-right:5px"></text>
							校园统一身份认证用户登录平台
						</view>
						<radio class="radio blue" :class="radio=='A'?'checked':''" :checked="radio=='A'?true:false"
							value="A"></radio>
					</view>

				</radio-group>
				<view v-if="n==1">
					<view class="cu-form-group margin-top">
						<view class="title">用户账号</view>
						<input placeholder="请输入提供的账号(工号/学号)" v-model="form.username"></input>
					</view>
					<view class="cu-form-group">
						<view class="title">用户密码</view>
						<input placeholder="请输入登录密码" type="password" v-model="form.password"></input>
					</view>
					<view class="cu-form-group">

						<view class="title">账号类型</view>
						<view>
							<radio-group class="block " @change="typeChange">
								<radio class='blue' :class="form.type=='T'?'checked':''"
									:checked="form.type=='T'?true:false" value="T" name="t"></radio> <text
									class="margin-left-sm">教职工</text>
								<radio class='blue  margin-left-sm' :class="form.type=='S'?'checked':''"
									:checked="form.type=='S'?true:false" value="S" name="t"></radio> <text
									class="margin-left-sm">学生</text>
							</radio-group>
						</view>

					</view>
				</view>
				<view v-if="n==2"  class="padding bg-white">
					<view class="text-center">
						<icon   type="success" size="60"></icon>
						<view class="padding">
							登录成功
						</view>
					</view>
					
				</view>
			</view>
			<view class="padding flex flex-direction">
				<button class="cu-btn bg-green lg" @click="next">{{n==2?'完成':'下一步'}}</button>
			</view>
		 


			<view style="margin-top:200rpx; " class="text-center">
				<text class="  text-sm" style="color:#ccc">励航软件 &copy; 2022 智慧课表 V1.0</text>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		mapState,
		mapMutations
	} from 'vuex'
	import auth from '../../commonjs/auth.js'

	export default {
		computed: {
			...mapState(["baseUrl"])
		},
		data() {
			return {

				numList: [{
					name: '选择类型'
				}, {
					name: '身份验证'
				}, {
					name: '完成'
				}],
				num: 0,
				radio: 'B',
				n: 0,
				form: {
					type: 'S'
				}
			};
		},
		methods: {
		
			RadioChange(e) {
				this.radio = e.detail.value
			},
			typeChange(e) {
				this.form.type = e.detail.value
			},
			next() {
				if (this.n == 0) {
					if (this.radio == "B") {
						this.n = 1;
						this.num = 1;
					} else {
						uni.showToast({
							mask: true,
							icon: "none",
							title: '暂未开放统一身份登录，请使用用户名密码登录'
						})
					}
				} else if(this.n==1){
					if (this.form.username && this.form.password) {
						uni.showLoading({
							mask: true,
							title: '登陆中'
						})
						uni.request({
							url: this.baseUrl + "/api/login/loginbyusername",
							method: 'GET',
							data: this.form,
							success: (res) => {
								uni.hideLoading()
								if (res.data.success) {
									uni.login({
										success: (lres) => {
											console.log("开始绑定账号")

											uni.request({
												url: this.baseUrl +
													"/api/login/BindWechat",
												method: "GET",
												header:{
													token:res.data.data.Token
												},
												data: {
													code: lres.code
												},
												success: (serverRes) => {
													if (serverRes.data.success) { 
														var now = new Date() - 0;
														uni.setStorageSync('token', {
															token: res.data.data.Token,
															expires: now +
																1000 * 60 *
																60 * 24,
															id: res.data.data
																.Id
														})
														this.n=2;
														this.num++;
														
														auth.getUserInfo();
														
													} else {
														uni.showModal({
															content: '账号绑定失败',
															title: '登录失败',
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

								} else {
									uni.showModal({
										content: '用户名和密码有误',
										title: '登录失败',
										showCancel: false
									})
								}
							}
						})
					} else {
						uni.showModal({
							content: '用户名和密码必填',
							title: '错误',
							showCancel: false
						})
					}
					
				}else{
					if( getCurrentPages().length == 1){
						uni.switchTab({
							url:'../index/index'
						})
					}else{
						uni.navigateBack();
						//不执行，弹出登录的，需重新触发操作
						// if (typeof(auth.afterLoginFN) == "function") {
						// 	auth.afterLoginFN();
						// 	auth.afterLoginFN = undefined; //执行完后，重置为undefined，避免多次执行
						// }
					}
					
				}

			},
		}
	}
</script>

<style lang="less">

</style>
