import store from '../store/store.js'
console.log("auth.js加载")
export default {
	baseurl: store.state.baseUrl,
	afterLoginFN: undefined,
	/**
	 * 登录后执行
	 * @param {Function} fn 登录完毕后的回调函数，注意同一时间程序只能有一个登录回调函数
	 */
	afterLogin(fn) {
		this.afterLoginFN = fn;
		this.ifLogin({
			yes: () => {
				this.afterLoginFN();
				this.afterLoginFN = undefined; //执行完后，重置为undefined，避免多次执行
			},
			no: () => {
				this.login();
			}
		})
	},
	/**
	 * 执行登录
	 * @param {Boolean} onlyTry 是否为尝试登录，不跳到登录页
	 */
	login(onlyTry) {
		uni.getSetting({
			success: (e) => {
				if (e.authSetting["scope.userInfo"]) {
					uni.login({
						success: (res) => {
						 
							console.log('开始登录流程')
							 
							uni.request({
								url: this.baseurl + "/api/login/LoginByWeChatCode",
								method: "GET",
								data: {
									code: res.code
								},
								success: (serverRes) => {
									if (serverRes.data.success) {
										 
									   var now=new Date()-0;
										uni.setStorageSync('token', {
											token:serverRes.data.data.Token,
											expires:now+1000*60*60*24,
											id:serverRes.data.data.Id
										})
										this.getUserInfo();
										if (typeof(this.afterLoginFN) == "function") {
											this.afterLoginFN();
											this.afterLoginFN = undefined; //执行完后，重置为undefined，避免多次执行
										}
									 
										console.log('登录完毕')
									}else{
										console.log("微信账号自动登录失败，跳转到登录页")
										if (!onlyTry) {
											uni.hideLoading();
											console.log("跳转")
											uni.navigateTo({
												url: "/pages/login/login"
											})
										}
									}
								}
							})
						},
						error() {
							console.log("调用login失败，可能网络不通畅")
						}
					})
				} else {
					if (!onlyTry) {
						console.log("跳转")
						uni.navigateTo({
							url: "/pages/login/login"
						})
					}

				}
			}
		})


	},
	/**
	 * 获取用户信息
	 */
	getUserInfo(){
		uni.request({
			url: this.baseurl + '/api/member',
			header: {
				token: uni.getStorageSync('token').token
			},
			 
			method: 'GET',
			success: (res) => {
				console.log(res.data.data)
				store.commit('set_userInfo', res.data.data);
			}
		})
	},
	
	/**
	 * 判断是否登录，可分别指定登录成功和不成功的回调函数
	 * @param {Object} options           登录状态下的回调函数
	 * @param {Function} options.yes    登录执行
	 * @param {Function} options.no     未登录执行
	 */
	ifLogin(options) {
		
		var token = uni.getStorageSync('token');
		console.log(new Date(token.expires) - new Date())
		if (token && (new Date(token.expires) - new Date()) > 60 * 1000) {
			if (typeof(options.yes) == "function") {
				options.yes()
			}
		} else {
			if (typeof(options.no) == "function") {
				options.no()
			}
		}
	}
}
