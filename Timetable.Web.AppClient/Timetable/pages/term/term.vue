<template>
	<view>
		<cu-custom bgColor="bg-gradual-white"  isBack>
			<template v-slot:content>学校校历</template>
		</cu-custom>
		<view style="padding:5rpx;background: #fff;"  >
			<view v-for="obj in tableData" :key="obj.month" class="calendar">
				<view class="text-center padding-tb text-orange text-bold">
					{{obj.month}}
				</view>
				<view class="grid  col-8 text-center  text-orange"  >
					<view> </view>
					 <view> 周一</view>
					 <view>周二 </view>
					 <view> 周三</view>
					 <view> 周四</view>
					 <view> 周五</view>
					 <view> 周六</view>
					 <view> 周日</view> 
				</view>
				<view class="grid  col-8 text-center " v-for="(row,index) in obj.rows" :key="index">
					<view class="text-grey">第{{row.week}}周</view>
					<view v-for="(cell) in row.data" :key="cell">
						{{cell?cell:''}}
					</view>
			
				</view>
			</view>
		</view>
		
	</view>
</template>

<script>
	import {
		mapState,
		mapMutations
	} from 'vuex';

	export default {
		data() {
			return {
				term: undefined,
				countNum: 0,
			};
		},
		computed: {
			...mapState(['baseUrl']),
			tableData() {
				var list = [];
				if (this.term) {
					var start = new Date(this.term.FixedStart);
					var end = new Date(this.term.CustomEnd);
					var month;
					var obj;
					var row;
					var map = [6, 0, 1, 2, 3, 4, 5];
					var week = 0;
					for (start;
						(end - start) >= 0; start.setDate(start.getDate() + 1)) {
						if (start.getMonth() != month) {
							if (obj) {
								list.push(obj);
								if (row && (row.data.length > 0)) {
									obj.rows.push(row);
									row = {
										week: week,
										data: []
									};
								}
							}
							month = start.getMonth();
							obj = {
								month: start.getFullYear() + "年" + (month + 1) + '月',

								rows: []
							}
						}
						var index = map[start.getDay()];
						if (index == 0) {
							week++;
							if (row) {
								obj.rows.push(row);
							}
							row = {
								week: week,
								data: []
							};
						}
						row.data[index] = start.getDate();
					}
					if (row && (row.data.length > 0)) {
						obj.rows.push(row);
						row = {
							week: week,
							data: []
						};
					}
					if (obj) {
						list.push(obj)
					}
				}

				return list;
			},
			allRows() {
				var rows = [];
				for (var obj of this.tableData) {
					rows.push.apply(rows, obj.rows)
					
				}
				
				return rows;
			}

		},
		methods: {
			count() {
				return ++this.countNum;
			}
		},
		created() {
			uni.request({
				method: 'GET',
				url: this.baseUrl + '/api/schoolterm',
				success: (res) => {
					if (res.data) {
						this.term = res.data;
						
					}
				}
			})
		}
	}
</script>

<style lang="less">
	.calendar>.grid {

		border-collapse: collapse;

		&:nth-child(2) {
			view {
				border-top: 0.5upx solid #ccc;
			}
		}

		view {
			&:first-child{
				border-left: 0.5upx solid #ccc;
			}


			padding: 15upx 0;
			border-bottom: 0.5upx solid #ccc;
			border-right: 0.5upx solid #ccc;
		}
	}
</style>
