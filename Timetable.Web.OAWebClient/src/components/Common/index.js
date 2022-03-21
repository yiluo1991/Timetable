import Vue from 'vue';
import BasicToolbar from './BasicToolbar';
import BasicPaginationBar from './BasicPaginationBar'
import BasicSelectTree from './BasicSelectTree'

export default {

    install(){
        Vue.component('BasicToolbar',BasicToolbar)
        Vue.component('BasicPaginationBar',BasicPaginationBar)
        Vue.component('BasicSelectTree',BasicSelectTree)
        

    }
}