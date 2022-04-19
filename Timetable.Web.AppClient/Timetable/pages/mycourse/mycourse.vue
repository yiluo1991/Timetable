<template>
	<view>
		<cu-custom bgColor="bg-white" isBack>
			<template v-slot:content>本学期课程</template>
		</cu-custom>
		<view class="course" v-for="course in list" :key="course.Id">
			<view class="coursetitle  ">
				{{course.SubjectName}}
			</view>
			<view class="courseinfo">
				<view v-if="course.DepartmentName||course.CollegeName">
					<view class="label">开课院系: </view>
					<view class="content">{{course.CollegeName}} {{course.DepartmentName}}</view> 
				</view> 
				<view v-if="course.ClassName">
					<view  class="label">授课班级: </view>
					<view class="content">{{course.ClassName}}</view>
				</view>
				<view>
					<view  class="label">授课教师: </view>
					<view class="content">{{course.RealName}}</view>
				</view>
				<view>
					<view  class="label">上课时间: </view>
					<view class="content">{{course.TimeBook.Desc}}</view>
				</view>
				<view v-if="course.Address">
					<view  class="label">上课地点: </view>
					<view class="content">{{course.Address}}</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import {mapState} from 'vuex'
	export default {
		data() {
			return {
				firstShow:true,
				list:[]
			};
		},
		created(){ 
			this.load()
		},
		computed:{
			...mapState(['baseUrl'])
		},
		onShow(){
			if(!this.firstShow){ 
				this.load()
			}else{
				this.firstShow=false;
			}
		},
		methods:{
			load(){
				this.$auth.afterLogin(()=>{
					 uni.request({
					 	url:this.baseUrl+'/api/course/mine',
						method:'POST',
						header:{
							token:uni.getStorageSync('token').token
						},
						success:(res)=>{
							if(res.data.success){
								res.data.data.forEach(s=>{
									s.TimeBook= JSON.parse(s.TimeBook)
								})
								this.list=res.data.data
							}else{
								uni.showToast({
									title:res.data.msg,
									icon:"none",
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
