<template>
	<view>
		<view>
			<view :style="transStyle.height" id="flag">
				<view class="cu-bar fixed  trans" style="transform: translateZ(0px);" :style="style+transStyle.top"
				 :class="[staticBorder?'':(noshadow?'noshadow':''),bgColor]">
					<view class="action" @tap="BackPage" :style="transStyle.textOpacity">
						<text :class="backIcon"></text>
						<slot name="title"></slot>
					</view>
					<view class="content" 
						:style="[{top: 'calc('+StatusBar + 'px'+' + 0rpx'+')',lineHeight:(CustomBar-StatusBar)+'px','height':'auto'}]">
						<slot name="content" ></slot>
					</view>
					<view class="searchbar " :style="transStyle.radius">
						<view class="searchinput" :style="isH5?'':transStyle.width">
							<text class="cuIcon-search icon"></text>
							<text class="keyword" v-if="!enableInput" @click="touchsearch">{{placeholder}}</text>
							<input class="keyword" type="text" confirm-type="search" v-model="keyword"  @confirm="search" :placeholder="placeholder" v-if="enableInput" >
						</view>
					</view>
				</view>

			</view>
		</view>
				
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				 
					StatusBar: this.StatusBar,
					CustomBar: this.CustomBar,
			 
				top:0,
				noshadow: true,
				keyword:''
			}
		},
		props:{
			enableInput:{
				type:Boolean,
				default:false
			},
			staticBorder:{
				type:Boolean,
				default:false
			},
			placeholder:{
				type:String
			},
			bgColor:{
				type:String,
				default:'bg-gradual-blue'
			}
		},
		computed: {
			backIcon() {
				return getCurrentPages().length == 1 ? 'cuIcon-home' : 'cuIcon-back'
			},
			transStyle() {
				
				var r = (1 - (-this.top / (this.CustomBar - this.StatusBar)));
				var radius = 40 * r;
				var opacity = r;
				//var width = 72 +28 * r;
				var width = r == 0 ? 72 : 100;
				return {
					height: `height:calc(${this.CustomBar}px + 90rpx );`,
					radius: `border-radius:${radius}rpx ${radius}rpx 0 0;`,
					//textOpacity: `color:rgba(255,255,255,${opacity});`,
					textOpacity: `color:rgba(0,0,0,${opacity});`,
					width: `width:${width}%;`,
					top: `transform:translateY(${this.top}px) translateZ(1px);`
				}
			},
			isH5() {
				//#ifdef H5
				return true;
				//#endif

				//#ifndef H5
				return false;
				//#endif
			},
			style() {
				var StatusBar = this.StatusBar;
				var CustomBar = this.CustomBar;
				var bgImage = this.bgImage;
				var style = `height:calc(${CustomBar}px + 90rpx );padding-top:${StatusBar}px;padding-bottom:90rpx;`;
				return style
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
			
			},
			search(){
				this.$emit('search',this.keyword);
			},
			scroll(e) {
				if (e.scrollTop > 0 && e.scrollTop <= (this.CustomBar - this.StatusBar)) {
					this.top = -e.scrollTop;
					this.noshadow = true;
				} else if (e.scrollTop == 0) {
					this.top = 0;
					this.noshadow = true;
				} else if (e.scrollTop > (this.CustomBar - this.StatusBar)) {
					this.top = -(this.CustomBar - this.StatusBar);
					this.noshadow = false
				}
			},
			touchsearch(){
				this.$emit('touchsearch');
			}
		}
	}
</script>

<style lang="less">

	
	.trans {
		transition: all linear 0.12s;
	}

	.searchbar {
		width: 100%;
		position: absolute;
		bottom: 0;
		height: 90rpx; 
		background: #fff;
		border-radius: 30rpx 30rpx 0 0;
		color: #555;
		padding: 15rpx 15rpx;

		.searchinput {
			background: rgba(0, 0, 0, 0.1);
			height: 60rpx;
			line-height: 60rpx;
			position: relative;
			border-radius: 30rpx;
			padding: 0 14rpx;
			transition: all 0.3s;


			text.icon {
				position: absolute;
				left: 25rpx;
			}

			.keyword {
				height: 60rpx;
				padding-left: 50rpx;
				padding-right:30rpx;
				line-height: 60rpx;
				font-size: inherit;
			}
		}

	}

	.noshadow {

		box-shadow: 0 1rpx 6rpx rgba(0, 0, 0, 0.0) !important;

	}
</style>
