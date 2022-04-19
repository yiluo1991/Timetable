<template>

	<web-view :src='url' v-if="url"></web-view>
</template>

<script>
	export default {
		data(){
			return {
				url:'',
			}
		},
		onLoad(option){
			console.log(option)
			this.url= decodeURIComponent( option.url);
		}
	}
</script>

<style>
</style>
