import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);

const store = new Vuex.Store({
	strict: true,
	state: {
		location: {
			title: "正在定位..."
		},
		token:'``',
		baseUrl: '',
		userInfo:{
			AvatarUrl: null,
			Gender: 1,
			IdentityCode: "",
			Mobile: "",
			RealName: "",
			Type:''
		}
	},
	mutations: {
		configBaseUrl(state,baseUrl){
			state.baseUrl=baseUrl;
		},
		set_location(state, value) {
			state.location = value
		},
		set_userInfo(state,value){
			console.log("userinfo已更新")
			state.userInfo=value;
		}
	}
})

export default store;
