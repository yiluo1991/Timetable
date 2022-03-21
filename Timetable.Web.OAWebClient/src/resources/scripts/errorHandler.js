import store from '../../store/index';
export default function(err, vm, info){
    console.error(err);
    store.commit('addError', {err,info})
}