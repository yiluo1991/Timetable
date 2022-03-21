import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

var saveConfig = localStorage.getItem("config");
if (!saveConfig) {
  saveConfig = '{"staticTop":true,"collapsed":false,"tabMode":true}';
}
var config = JSON.parse(saveConfig);
config.fullscreen = false;


export default new Vuex.Store({
  state: {
    config: config,
    menu:{},
    permission:{},
    tabs:["/admin"],
    errors:[]
  },
  mutations: {
    addError(state,obj){
      if(["local","development","serverless"].indexOf( process.env.NODE_ENV)>-1)  state.errors.push(obj);
    },
    clearErrors(state){
      state.errors.splice(0,state.errors.length);
    },
    
    updateStaticTopConfig(state, value) {
      state.config.staticTop = value;
      localStorage.setItem("config", JSON.stringify(state.config));
    },
    updateCollapsedConfig(state, value) {
      state.config.collapsed = value;
      localStorage.setItem("config", JSON.stringify(state.config));
    },
    updateTabModeConfig(state,value){
    
      state.config.tabMode = value;
   
      localStorage.setItem("config", JSON.stringify(state.config));
    },
    updatePermissionMenu(state,value){
        state.permission=value;
    },
    updateMenuList(state,value){
      state.menu={"/admin":"首页"};
      for(var root of value){
         if(root.Url){
          Vue.set(state.menu,root["Url"],root.Name)
         }
       
         if(root.Children.length>0){
           for(var child of root.Children){
           
            Vue.set(state.menu,child["Url"],child.Name)
           }
         }
      }
      for(var i in state.tabs){
        if(!state.menu[state.tabs[i]]){
          state.tabs.splice(i,1);
        }
      }
      
    },
    resetTabs(state){
      state.tabs=["/admin"];
    },
    addTab(state,url){
      if(state.tabs.filter(s=>s==url).length==0){
        state.tabs.push(url)
        console.log("add")
      }
    },
    removeTab(state,url){
      var index=state.tabs.findIndex(s=>s==url);
      if(index>-1){
        state.tabs.splice(index,1);
      }
    }
  },
  actions: {
  },
  modules: {
  }
})
