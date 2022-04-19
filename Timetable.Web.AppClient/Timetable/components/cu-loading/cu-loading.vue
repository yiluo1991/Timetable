<template>
	<view   class="cu-load text-gray margin-bottom-sm"  :class="!noMore?(loading?'loading':'standby'): 'over'"></view> 
</template>

<script>
	export default {
		props:{
			loading:{
				type:Boolean
			},
			noMore:{
				type:Boolean
			}
		}
	}
</script>

<style>
</style>
