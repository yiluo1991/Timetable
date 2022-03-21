<template>
  <el-dialog :title="dialogTitle" :visible.sync="dialogVisible" width="600px" close :close-on-click-modal="false">
    <div>
      <el-form action="asdf" :rules="rules" ref="form" v-bind:model="editRow" label-width="100px" @submit.native.prevent>
        <el-form-item label="节点名称" prop="Name">
          <el-input v-model="editRow.Name"></el-input>
        </el-form-item>
        <el-form-item label="节点地址" prop="Host">
          <el-input v-model="editRow.Host"></el-input>
        </el-form-item>
        <el-form-item label="在线" prop="Online">
          <el-select  v-model="editRow.Online" style="width:100%;"   placeholder="请选择">
            <el-option label="在线" :value="true"></el-option>
            <el-option label="离线" :value="false"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="健康程度" prop="Healthy">
          <el-select v-model="editRow.Healthy" style="width:100%;"  placeholder="请选择">
            <el-option label="健康" :value="true"></el-option>
            <el-option label="不健康" :value="false"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="权重" prop="Weight">
          <el-input-number   :controls="false" :min="1" :max="99999" v-model="editRow.Weight"></el-input-number>
        </el-form-item>
      </el-form>
    </div>
    <span slot="footer" class="dialog-footer">
      <el-button @click="dialogVisible = false" icon="el-icon-close">取 消</el-button>
      <el-button type="primary" @click="save()" icon="el-icon-check">确 定</el-button>
    </span>
  </el-dialog>
</template>
<script>
export default { 
    inject: ["hasPermission"],
  computed: {
    hasEditPermission() {
      return this.hasPermission("SP_M");
    },
       hasAddPermission() {
      return this.hasPermission("SP_A");
    },
    
  },
  data() {
    return {
         dialogTitle: '添加服务节点',
      dialogVisible: false,
      action:'',
      editRow: {},
      rules: {
        Name: [
          { required: true, message: "请输入名称", trigger: "change" },
          { min: 1, max: 64, message: "输入的内容的长度应在1~64之间", trigger: "change" },
        ],
        Host: [
          { required: true, message: "请输入地址", trigger: "change" },
           {  max:22, message: "最长22位", trigger: "change" },
          {
            pattern: /^((((25[0-5])|(2[0-4]\d)|(1\d\d)|([1-9]\d)|\d)(\.((25[0-5])|(2[0-4]\d)|(1\d\d)|([1-9]\d)|\d)){3} )|([a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+\.?))(:\d{1,5})?$/,
            message: "请输入节点地址，如：192.168.1.1，192.168.1.1:91",
            trigger: "change",
          },
        ],
        Online: [{ required: true, message: "请选择节点在线情况", trigger: "change" }],
        Healthy: [{ required: true, message: "请选择节点健康状况", trigger: "change" }],
        Weight: [
          { required: true, message: "请输入权重", trigger: "change" }, 
        ],
      },
    };
  },
  methods:{
         showAdd : function () {
             if(!this.hasAddPermission)return;
                this.action = "add";
                this.editRow = { Name: '', Host: '', Online: '', Healthy: '', Weight: 1 }
                this.dialogVisible = true;
               this.$nextTick(()=>{
                    this.$refs["form"].clearValidate();
                }, 10)

            }, showEdit : function (row) {
                if(!this.hasEditPermission)return;
                this.action = 'edit'
                var e = JSON.parse(JSON.stringify(row));
             
                this.editRow = e;
                this.dialogVisible = true;
            },
            save(){
                   this.$refs["form"].validate((result) => {
                    if (result) {
                        var url;
                        if (this.action == "add") {
                            if(!this.hasAddPermission)return;
                            url = '/api/ServicePoint/Add'
                        } else {
                            if(!this.hasEditPermission)return;
                            url = '/api/ServicePoint/Edit'
                        }
                        this.$axios.post(url, this.editRow).then( (res ) => { 
                            this.dialogVisible = false;
                            if (res.data.success == true) {
                                this.$message({
                                    type: 'success',
                                    message: (this.action == "add"?'添加':'修改')+'成功!'
                                });
                            } else {
                                this.$message({
                                    type: 'error',
                                    message: data.Message
                                });
                            }
                            this.$emit('reload');
                        })
                    } else {
                        return false;
                    }
                })
            },
  }
};
</script>
