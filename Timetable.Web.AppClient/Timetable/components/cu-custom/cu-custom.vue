<template>
	<view>
		<view  :style="[{height:'calc('+ CustomBar + 'px'+' + '+ (hasRound?'10rpx':'0rpx')+')' }]"></view>
		<view style="position: fixed;width:100%; z-index:9999;top:0" > 
		<view class="cu-custom " :class="hasShadow?'shadow':''"  >
			<view class="cu-bar fixed noevent" :style="style"
				:class="[bgImage!=''?'none-bg text-white bg-img':'',bgColor,hasRound?'hasRound':'']">
				<view class="action" @tap="BackPage" v-if="isBack">
					<text :class="backIcon"></text>
					<slot name="backText"></slot>
				</view>
				<view class="content"
					:style="[{top: 'calc('+StatusBar + 'px'+' + 0rpx'+')',lineHeight:(CustomBar-StatusBar)+'px'}]">
					<slot name="content"></slot>
				</view>
				<slot name="right"></slot>
			</view>
			<view class="mask"></view>
		</view>
		</view>
		
	</view>
</template>

<script>
	export default {
		data() {
			return {
				StatusBar: this.StatusBar,
				CustomBar: this.CustomBar
			};
		},
		name: 'cu-custom',
		computed: {
			backIcon() {
				return getCurrentPages().length == 1 ? 'cuIcon-home' : 'cuIcon-back'
			},
			style() {
				var StatusBar = this.StatusBar;
				var CustomBar = this.CustomBar;
				var bgImage = this.bgImage;
				var style = `height:calc(${CustomBar}px + ${this.hasRound?'40':'0'}rpx);padding-top:${StatusBar}px;`;
				if (this.bgImage) {
					style = `${style}background-image:url(${bgImage});`;
				}
				return style
			}
		},
		props: {
			bgColor: {
				type: String,
				default: ''
			},
			isBack: {
				type: [Boolean, String],
				default: false
			},
			bgImage: {
				type: String,
				default: ''
			},
			hasRound: {
				type: Boolean,
				default: false
			},
			hasShadow: {
				type: Boolean,
				default: false
			}
		},
		methods: {
			BackPage() {
				if (getCurrentPages().length < 2) {
					if ('undefined' !== typeof __wxConfig) {
						let url = '/' + __wxConfig.pages[0]
						uni.switchTab({
							url
						})
					} else {
						uni.switchTab({
							url:'/pages/index/index'
         				})
					}
				} else {
					uni.navigateBack({
						delta: 1
					});

				}

			}
		}
	}
</script>

<style lang="less">
	.cu-custom.shadow{
		    filter:drop-shadow(0 0 10rpx rgba(0,0,0,0.2));
	}
	.cu-bar.hasRound {
		-webkit-mask-image: url(../../static/mask.png);
		-webkit-mask-position: bottom;
		-webkit-mask-size: 100%;
		-webkit-mask-repeat: no-repeat;
		-webkit-mask-positon: bottom;
		position: fixed;
		bottom: -40rpx;
		left: 0;
		right: 0;
		padding-bottom: 40rpx;
     
		.content {

			height: 100%;
			margin-top: 0rpx;
			padding-bottom: 40rpx;
		}

		box-shadow: 1px 1px 10rpx black;
	}

	.cu-bar {
		.content {
			height: auto !important;
		}

	}

	.noevent {
		pointer-events: none;

		.action {
			pointer-events: auto !important;
		}
	}
</style>
