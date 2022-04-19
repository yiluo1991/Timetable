<template>
	<view>
		 <cu-custom bgColor="bg-gradual-white"  isBack>
		 	<template v-slot:content>修改密码</template>
		 </cu-custom>
		<view class="cu-form-group margin-top">
			<view class="title">旧密码</view>
			<input placeholder="请输入旧登录密码" type="password" v-model="form.o"></input>
		</view>
		<view class="cu-form-group">
			<view class="title">新密码</view>
			<input placeholder="请输入新登录密码" type="password" v-model="form.n"></input>
		</view>
		<view class="cu-form-group">
			<view class="title">验证密码</view>
			<input placeholder="请再次输入新登录密码" type="password" v-model="form.r"></input>
		</view>
		<button class="cu-btn block lg bg-green margin " @click="change">确认修改</button>
	</view>
</template>

<script>
	import {mapState,mapMutations} from "vuex";
	
	export default {
		computed:{
			...mapState(['baseUrl','userInfo' ])
		},
		data() {
			return {
				form:{
					o:'',
					n:'',
					r:''
				}
			}
		},
		methods: {
			change(){
				var err=[];
				if(!this.form.o){
					err.push(err.length+1+".请输入旧密码；")
				}
				if(!this.form.n){
					err.push(err.length+1+".请输入新密码；")
				}
				if(!this.form.r){
					err.push(err.length+1+".请输入验证密码；")
				}
				if(this.form.n&&this.form.r&&(this.form.n!=this.form.r)){
					err.push(err.length+1+".两次输入的新密码不一致；")
				}
				if(err.length==0){
					uni.showLoading({
						mask:true,
						title:'处理中'
					})
					this.$auth.afterLogin(()=>{
						uni.request({
							url:this.baseUrl+'/api/member/password',
							method:'POST',
							data:{
								old:this.form.o,
								new:this.form.n
							},
							header:{
								token:uni.getStorageSync('token').token
							},
							success:(res)=>{
								uni.hideLoading();
								if(res.data.success){
									uni.showModal({
										title:'成功',
										content:"密码修改成功",
										showCancel:false,
										complete:()=>{
											if(getCurrentPages().length>1){
												
												uni.navigateBack()
											}else{
												uni.switchTab({
													url:'../index/index'
												})
											}
										}
									})
									
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
				else{
					uni.showModal({
						mask:true,
						showCancel:false,
						title:'错误',
						content:err.join('\n')
					})
				}
			}
		}
	}
</script>

<style>

</style>
