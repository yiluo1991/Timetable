import Vue from 'vue';
import BasicToolbar from './BasicToolbar';
import BasicPaginationBar from './BasicPaginationBar'
import BasicSelectTree from './BasicSelectTree'

export default {

    install(){
        Vue.component('BasicToolbar',BasicToolbar)
        Vue.component('BasicPaginationBar',BasicPaginationBar)
        Vue.component('BasicSelectTree',BasicSelectTree)
        
        Vue.filter("date", function(value) {
            var value1 = new Date(value);
            let y = value1.getFullYear();
            let MM = value1.getMonth() + 1;
            MM = MM < 10 ? "0" + MM : MM;
            let d = value1.getDate();
            d = d < 10 ? "0" + d : d;
            return y + "-" + MM + "-" + d;
          });
          Vue.filter("datetime", function(value) {
            var value1 = new Date(value);
            let y = value1.getFullYear();
            let MM = value1.getMonth() + 1;
            MM = MM < 10 ? "0" + MM : MM;
            let d = value1.getDate();
            d = d < 10 ? "0" + d : d;
            let h=value1.getHours();
            h= h<10?'0'+h:h;
            let m=value1.getMinutes();
            m=m<10?'0'+m:m;
            let s=value1.getSeconds();
            s=s<10?'0'+s:s;
            return y + "-" + MM + "-" + d +" "+h+":"+m+":"+s;
          });
    }
}