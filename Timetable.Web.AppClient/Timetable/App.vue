<script>
	import Vue from 'vue';
	import store from './store/store.js'
	Vue.prototype.$baseUrl = 'https://timetable.xmlihang.com';
	//Vue.prototype.$baseUrl = 'http://192.168.6.29:63573';
	store.commit('configBaseUrl', Vue.prototype.$baseUrl);
	export default {

		onLaunch: function() {
			uni.getSystemInfo({
				success: function(e) {

					// #ifndef MP 

					Vue.prototype.StatusBar = e.statusBarHeight;
					if (e.platform == 'android') {
						Vue.prototype.CustomBar = e.statusBarHeight + 50;
					} else {
						Vue.prototype.CustomBar = e.statusBarHeight + 45;
					};
					// #endif
					// #ifdef MP-WEIXIN 
					Vue.prototype.StatusBar = e.statusBarHeight;
					let custom = wx.getMenuButtonBoundingClientRect();
					Vue.prototype.Custom = custom;
					Vue.prototype.CustomBar = custom.bottom + custom.top - e.statusBarHeight;

					// #endif		
					// #ifdef MP-ALIPAY
					Vue.prototype.StatusBar = e.statusBarHeight;
					Vue.prototype.CustomBar = e.statusBarHeight + e.titleBarHeight;
					// #endif
				}
			})

			Vue.prototype.ColorList = [{
					title: '嫣红',
					name: 'red',
					color: '#e54d42'
				},
				{
					title: '桔橙',
					name: 'orange',
					color: '#f37b1d'
				},
				{
					title: '明黄',
					name: 'yellow',
					color: '#fbbd08'
				},
				{
					title: '橄榄',
					name: 'olive',
					color: '#8dc63f'
				},
				{
					title: '森绿',
					name: 'green',
					color: '#39b54a'
				},
				{
					title: '天青',
					name: 'cyan',
					color: '#1cbbb4'
				},
				{
					title: '海蓝',
					name: 'blue',
					color: '#0081ff'
				},
				{
					title: '姹紫',
					name: 'purple',
					color: '#6739b6'
				},
				{
					title: '木槿',
					name: 'mauve',
					color: '#9c26b0'
				},
				{
					title: '桃粉',
					name: 'pink',
					color: '#e03997'
				},
				{
					title: '棕褐',
					name: 'brown',
					color: '#a5673f'
				},
				{
					title: '玄灰',
					name: 'grey',
					color: '#8799a3'
				},
				{
					title: '草灰',
					name: 'gray',
					color: '#aaaaaa'
				},
				{
					title: '墨黑',
					name: 'black',
					color: '#333333'
				},
				{
					title: '雅白',
					name: 'white',
					color: '#ffffff'
				},
			]


			//this.$auth.login(true);
			
		},
		 
		onShow: function() {
			console.log('App Show')
		},
		onHide: function() {
			console.log('App Hide')
		}
	}
</script>

<style lang="less">
	/*每个页面公共css */
	@import "colorui/main.css";
	@import "colorui/icon.css";
	@import "colorui/animation.css";

	.radius-card {
		border-radius: 30rpx;
		background: #fff;
		overflow: hidden;
	}

	.itemhover:active {
		color: #000;
		background-color: rgba(255, 255, 255, 0.7) !important;
		transition: 0.1s all;
	}

	.half-radius-card {
		border-radius: 0 0 30rpx 30rpx;
	}

	.subjectlist-card {
        &::after{
			content:'';
			display:block;
			clear: both;
		}
		.subjectitem {
			&:nth-child(1n){
				border-right:.5px solid rgba(0, 0, 0, 0.05);
				
			}
			border-bottom:.5px solid rgba(0,0,0,0.05);
			width: 50%;
			float: left;
			background-color: #fff;
			height:430rpx;
			image {
				margin: 15rpx;
				width: 325rpx;
				height: 195rpx;
				border-radius: 20rpx;
			}

			.content {
				display: flex;
				flex-direction: column;
				padding:0 15rpx 15rpx 15rpx;

				.subject-title { 
					width: 100%;
					-webkit-line-clamp: 2;
					line-clamp: 2;
					height:59rpx;
					overflow: hidden;
					line-height: 30rpx;
					margin-bottom: 5rpx;
					-webkit-box-orient: vertical;
					display: -webkit-box;
					text-overflow: ellipsis;
					overflow: hidden;
				}

				.flex {
					.left {
						.category {}

						flex:100rpx;
						flex-grow: 100;
						flex-shrink: 0;
					}

					.right {
						align-items: right;
						flex: 100rpx;
						flex-grow: 50;

						view {
							text-align: right;
						}

						.old {
							text-decoration: line-through;

						}
					}
				}

				.tags {
					overflow: hidden;

					.tag {
						background: lighten(#0081FF, 15);
						border: 1px solid lighten(#0081FF, 5);
						border-radius: 5rpx;
						padding: 0 5rpx;
						white-space: nowrap;
						margin: 5rpx;
						float: left;
						color: #fff;
						line-height: 28rpx;
					}
				}

			}
		}

	

	}
	.noscrollbar{
		::-webkit-scrollbar{
		
		 width: 0;
		
		 height: 0;
		
		 color: transparent
		
		}
	}
	.course{
			margin:20rpx; 
			color:#666;
			background: #fff;
			padding: 20rpx;
			border-radius: 15rpx;
			.coursetitle{
				margin:20rpx 0; 
				color:#000;
			}
			.courseinfo{  
				>view{
					
					color:#666;
					display: flex;
					padding:10rpx 0;
					.label{
						vertical-align: middle;
						width:140rpx;
						 text-justify: inter-character;	
						flex-shrink: 0;  
						line-height: 30rpx;
					}
					.content{
							vertical-align: middle;	
						padding-left:20rpx;
						
						line-height: 30rpx;
						flex-grow: 1; 
					}
				}
			}
		}
</style>
