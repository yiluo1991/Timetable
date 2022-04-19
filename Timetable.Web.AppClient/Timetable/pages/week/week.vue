<template>
	<view>
		<cu-custom bgColor="bg-white" isBack>
			<template v-slot:content>本周课程</template>
		</cu-custom>
		<scroll-view scroll-x class="bg-white nav noscrollbar"  scroll-with-animation  :scroll-left="scrollLeft">
		<view class="cu-item" :class="index==TabCur?'text-green cur':''" v-for="(item,index) in dateList" :key="index" @tap="tabSelect" :data-id="index">
			{{item.s}} {{map[index]}}
		</view>
		</scroll-view>
		<view class=" padding-sm  text-center" style="width:100%"> 
			<cu-coursedate :courses="courseDate"></cu-coursedate>
		</view>
		 
	</view>
</template>

<script>
	import vue from 'vue'
	import {mapState} from 'vuex'
	export default {
		onShareAppMessage(){
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			　　var shareObj = {
			　　　　title: "智慧课表-本周课程"
			　　};
			　
			　　return shareObj;
		},
		onShareTimeline(){
			// 设置菜单中的转发按钮触发转发事件时的转发内容
			　　var shareObj = {
			　　　　title: "智慧课表-本周课程"
			　　};
			　
			　　return shareObj;
		},
		data() {
			return {
				TabCur:undefined,
				scrollLeft:0,
				courseDate:undefined,
				index:0,
				fistShow:true,
				map:['周一','周二','周三','周四','周五','周六','周日']
			}
		},
		computed:{
			...mapState(['baseUrl']),
			dateList(){
				var date=this.getFirstDay();
				var list=[]; 
				for(var i=0;i<7;i++){
					var str=vue.filter('sortdate')(date);
					list.push({
						f:str,
						s:str.replace(date.getFullYear()+'-','')
					})
					date.setDate(date.getDate()+1);
				}
				return list
			}
		},
		created(){
			var todaystr=vue.filter('sortdate')(new Date());
			this.index=this.dateList.findIndex(s=>s.f==todaystr);
			
			if(this.index==-1){
				uni.showToast({
					title:'日期错误，为您选择周一',
					icon:"none"
				})
				this.index=0;
			}
			
			this.changeSelect()
		},
		onShow(){
			if(this.fistShow){
				this.fistShow=false;
			}else{
				this.changeSelect()
			}
		},
		methods: {
			tabSelect(e){
				this.index=e.currentTarget.dataset.id;
				this.changeSelect()
			},
			changeSelect(){
				this.scrollLeft = ( this.index- 1) * 70; 
				this.TabCur = this.index;
				this.load(this.index);
				
			},
			getFirstDay(){
				var now=new Date( vue.filter('sortdate')(new Date())+' 00:00:00');
				while(now.getDay()!=1){
					now.setDate(now.getDate()-1)
				}
				
				return now; 
			},
			load(){
				this.$auth.afterLogin(()=>{
					this.courseDate=undefined; 
					uni.request({
						url:this.baseUrl+'/api/course/minedate',
						method:'GET',
						data:{date:this.dateList[this.index].f},
						header:{
							token:uni.getStorageSync('token').token
						},
						success:(res)=>{
							if(res.data.success){
								res.data.data.forEach(s=>{
									s.TimeBook= JSON.parse(s.TimeBook)
								})
								this.courseDate=res.data.data
							}else{
								uni.showToast({
									title:res.data.msg,
									icon:'none',
									mask:true
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

</style>
