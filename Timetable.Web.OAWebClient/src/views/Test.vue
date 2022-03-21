<template>
  <div class="admin-page">
    <h1>文件上传测试</h1>
    {{src}}
    <br>
    {{fileList}}
    <ElUpload
      class="upload-demo avatar-uploader articleimg"
      accept=""
      action="/api/employee/uploadheadshot"
      list-type="picture-card"
      :on-remove="handleRemove"
      :on-change="clearList"
      :on-success="handleSuccess"
      :file-list="fileList"
      ref="uploader"
    >
      <div style="font-size:12px">点击上传</div>
      <div slot="tip" style="font-size:12px;line-height:12px; margin-top:10px;">
        只能上传图片文件，建议宽高比1:1
      </div>
    </ElUpload>
  </div>
</template>
<script>
export default {
  data() {
    return {
      src:'/upload/2020-08/061447170974b3a3c6b.png',
      fileList: [{ name:'', url: '/upload/2020-08/061447170974b3a3c6b.png' }],
    };
  },
  methods: {
    clearList: function(file, fileList) {
      if (this.$refs.uploader.$refs["upload-inner"].fileList.length == 2) {
        this.$refs.uploader.$refs["upload-inner"].fileList.splice(0, 1);
      }
      // app.$refs.uploader.clearFiles()
    },
    handleRemove(file, fileList) {
      this.src=''
    },
   
    beforeRemove(file, fileList) {
      return this.$confirm(`确定移除 ${file.name}？`);
    },
    handleSuccess(response, file, fileLis) {
      if (response.success == false) {
        if (this.$refs.uploader.$refs["upload-inner"].fileList.length == 1) {
          this.$refs.uploader.$refs["upload-inner"].fileList.splice(0, 1);
        }
        this.$message({
          type:"error",
          message:response.msg
        })
      }
       

      this.src=response.data;
    },
  },
};
</script>
