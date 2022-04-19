<template>
	<view>
		<cu-custom bgColor="bg-gradual-white" isBack>
			<template v-slot:content>本周课表</template>
		</cu-custom>

		<view class="weektable">
			<view style="position: relative;" v-if="rotate">
				<view :class="{changebutton:true,r:rotate}" @click="changeRotate()">
					<button class="cu-btn  bg-green round  sm">
						<text class="cuIcon-camerarotate text-white text-lg"></text>
					</button>
				</view>
			</view>
			<scroll-view v-if="!rotate" scroll-x class="bg-white">
				<mp-html class="normal" :content="html" :tag-style="style"></mp-html>
			</scroll-view>
			<view class="padding" v-if="!rotate">
				<button class="cu-btn  bg-green round  block "  @click="changeRotate()">
					<text class="cuIcon-camerarotate  text-lg margin-right-xs"></text> 旋转表格
				</button>
			</view>



			<scroll-view v-if="rotate" scroll-x class="bg-white" :style="{height:availableWidth+'px'}">
				<view style="position:relative;overflow-x:hidden;overflow-y: visible;"
					:style="{width:height+'px',height:availableWidth+'px'}">
					<view class="rotate" :style="{height:availableWidth+'px',transform: 'rotateZ(-90deg) translateX(-'+availableWidth+'px)',
		'transform-origin':'0rpx 0rpx'}">
						<mp-html :content="html" :tag-style="style"></mp-html>
					</view>
				</view>
			</scroll-view>
		</view>
	</view>
	</view>
</template>

<script>
	import {
		mapState
	} from 'vuex';
	import vue from 'vue'
	export default {
		onShareAppMessage() {
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			var shareObj = {
				title: "智慧课表-本周课程"
			};

			return shareObj;
		},
		onShareTimeline() {
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			var shareObj = {
				title: "智慧课表-本周课程"
			};

			return shareObj;
		},
		computed: {
			...mapState(['baseUrl']),
			style() {
				return {
					table: "border:1rpx solid #ccc;border-collapse:collapse;table-layout:fixed;width:" + this
						.availableWidth + "px;font-size:22rpx;",
					td: "height:50rpx;border:1rpx solid #ccc;text-align: center;vertical-align: middle;",
					th: "height:50rpx;border:1rpx solid #ccc;text-align: center;vertical-align: middle;background:#f1f1f1",
					span:'color:#aaa'
				}
			},
			dateList() {
				var list = [];
				var first = this.getFirstDay();
				for (var i = 0; i < 7; i++) {
					list.push(vue.filter('sortdate')(first))
					first.setDate(first.getDate() + 1);
				}
				return list;
			},
			html() {
				if (this.tableData) {
					var rows = [];
					for (var i = 0; i < 12; i++) {
						var r = `
							<th  >第${i+1}节</th>
						`
						this.dateList.forEach(date => {
							var dateData = this.tableData[date];
							if (this.unNeedCellIndex[date].indexOf(i) == -1) {
								var target = dateData.find(s => s.StartIndex == i);
								if (target) {
									r += `<td  ${target.Length>1?'rowspan="'+target.Length+'"':''}>
									${target.SubjectName}
									${target.Address?'<br/><span>'+target.Address:'</span>'}
									</td>`
								}
							} else {
								r += "<td></td>"
							}

						})
						rows.push("<tr>" + r + "</tr>")
					}
					var str = `
					<table>
						<tr>
							<th></th><th>周一</th><th>周二</th><th>周三</th><th>周四</th><th>周五</th><th>周六</th><th>周日</th>
						</tr>
						${rows.join('')}
					</table>
					`

					return str;
				} else {
					return ""
				}
			},
			courseByDay() {
				if (this.weekDate) {
					var obj = {};
					this.dateList.forEach(s => {
						obj[s] = this.weekDate.filter(t => t.TimeBook[s] && t.TimeBook[s].length > 0).map(t => {
							var copy = {
								...t
							}
							copy.TimeBook = t.TimeBook[s]
							return copy
						})
					})
					return obj;
				} else {
					return undefined;
				}
			},
			unNeedCellIndex() {
				if (this.tableData) {
					var obj = {};
					var all = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
					this.dateList.forEach(s => {
						var tableColData = this.tableData[s];
						var has = [];
						tableColData.forEach(c => {
							for (var i = c.StartIndex; i < c.StartIndex + c.Length; i++) {
								has.push(i)
							}
						})
						obj[s] = all.filter(n => has.indexOf(n) == -1);
					})
					return obj;
				}
			},
			tableData() {
				if (this.courseByDay && this.weekDate) {
					var obj = {};
					this.dateList.forEach(s => {
						var courses = this.courseByDay[s];
						if (courses) {
							var data = [];
							for (var i = 0; i < 12; i++) {
								data[i] = 0;
							}
							for (var course of courses) {
								for (var i = 0; i < 12; i++) {
									if (course.TimeBook.indexOf(i + 1) > -1) {
										data[i] = {
											...course
										}
										data[i].StartIndex = i;
										data[i].Length = 1;
									}
								}
							}
							for (var i = 11; i >= 0; i--) {
								if (data[i] === 0) {
									data.splice(i, 1);
								} else {
									if (i > 0 && (data[i - 1] !== 0)) {
										if (data[i].Id == data[i - 1].Id) {
											data[i - 1].Length += data[i].Length;
											data.splice(i, 1);
										}

									}
								}
							}
							obj[s] = data;
						}
					})
					return obj;


				} else {
					return undefined;
				}
			}
		},
		onShow() {
			if (this.fistShow) {
				this.fistShow = false;
			} else {
				this.load()
			}
		},
		data() {
			return {
				rotate: false,
				weekDate: undefined,
				index: 0,
				availableWidth: 0,
				fistShow: true,
				height: 0,
				map: ['周一', '周二', '周三', '周四', '周五', '周六', '周日'],

			}
		},
		created() {
			this.load();
		},
		methods: {
			changeRotate() {
				if (this.rotate == false) {
					wx.createSelectorQuery().select('.normal').boundingClientRect(rect => {
						this.height = rect.height;
						this.rotate = true;

					}).exec()
				}else{
					this.rotate=false;
				}
			},
			load() {
				let custom = wx.getMenuButtonBoundingClientRect();
				wx.getSystemInfo({
					success: (res) => {
						this.availableWidth = res.screenHeight - custom.top - custom.bottom + res
							.statusBarHeight
					}
				});

				this.$auth.afterLogin(() => {
					uni.request({
						url: this.baseUrl + '/api/course/mineweek',
						method: 'GET',
						data: {},
						header: {
							token: uni.getStorageSync('token').token
						},
						success: (res) => {
							if (res.data.success) {
								res.data.data.forEach(s => {
									s.TimeBook = JSON.parse(s.TimeBook)
								})
								this.weekDate = res.data.data
							} else {
								uni.showToast({
									title: res.data.msg,
									icon: 'none',
									mask: true
								})

							}
						}
					})
				})

			},
			getFirstDay() {
				var now = new Date(vue.filter('sortdate')(new Date()) + ' 00:00:00');
				while (now.getDay() != 1) {
					now.setDate(now.getDate() - 1)
				}

				return now;
			},
		}
	}
</script>

<style lang="less">
	.weektable .rotate {
		position: absolute;
	}

	.changebutton.r {
		position: absolute;
		top: 10rpx;
		left: 2rpx;
	
		z-index: 99999;
		text-align: center;
	
			transform:rotateZ(-90deg)
		
	}
</style>
