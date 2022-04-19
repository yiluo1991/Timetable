import Vue from 'vue'
import App from './App'



Vue.config.productionTip = false

//全局存储
import store from './store/store.js'
//载入的地图API，可支持baiduMapApi、aMapApi、tencentMapApi
import {
	baiduMapApi as mapApi
} from 'commonjs/map_util.js'
Vue.prototype.$mapApi = mapApi;



Vue.filter("date", function(value) {
	var value1 = new Date(value);
	let y = value1.getFullYear();
	let MM = value1.getMonth() + 1;
	MM = MM < 10 ? "0" + MM : MM;
	let d = value1.getDate();
	d = d < 10 ? "0" + d : d;
	return y + "-" + MM + "-" + d;
});
Vue.filter("sortdate", function(value) {
	var value1 = new Date(value);
	let y = value1.getFullYear();
	let MM = value1.getMonth() + 1;
	let d = value1.getDate(); 
	return y + "-" + MM + "-" + d;
});
Vue.filter("money", function(value) {
	if (value) {

		value = parseFloat(value);
	} else {
		value = 0;
	}
	return value.toFixed(2);
});
Vue.filter("datetime", function(value) {
	var value1 = new Date(value);
	let y = value1.getFullYear();
	let MM = value1.getMonth() + 1;
	MM = MM < 10 ? "0" + MM : MM;
	let d = value1.getDate();
	d = d < 10 ? "0" + d : d;
	let h = value1.getHours();
	h = h < 10 ? '0' + h : h;
	let m = value1.getMinutes();
	m = m < 10 ? '0' + m : m;
	let s = value1.getSeconds();
	s = s < 10 ? '0' + s : s;
	return y + "-" + MM + "-" + d + " " + h + ":" + m + ":" + s;
});

Vue.filter("days", function(value) {
	if(value){
		value=Math.abs(value);
		var days= Math.floor( value/1000/60/60/24);
		var hours= Math.floor( (value%(1000*60*60*24))/1000/60/60);
		var mins=Math.floor( (value%(1000*60*60)) / 1000/60);
		var secs= Math.floor( (value%(1000*60)) / 1000); 
		
		return `${days==0?'':days+'天'}${hours==0?'':hours+'小时'}${mins  }分${secs}秒`
	}else{
		return undefined;
	}
});



import Auth from 'commonjs/auth.js'
Vue.prototype.$auth = Auth;
Vue.prototype.$url = function navTo(src) {
	console.log(src)
	if (src && !src.startsWith('#')) {
		uni.navigateTo({
			url: src,
			fail: (a) => {
				uni.showToast({
					icon: 'none',
					title: '页面未找到'
				})
			}
		})
	}
};
App.mpType = 'app'

const app = new Vue({

	...App,
	store
})
app.$mount()
