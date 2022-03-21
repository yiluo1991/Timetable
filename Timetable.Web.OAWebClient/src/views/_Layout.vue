<template v-if="false">
  <div id="layout" :class="{ collapsed: config.collapsed, tabmode: config.tabMode }">
    <div id="layout_menu">
      <div class="mainlogo">
        <img src="../resources/images/lhlogo.png" alt />
      </div>
      <ElScrollbar id="catchscroll">
        <BasicMenu></BasicMenu>
      </ElScrollbar>
    </div>
    <div id="layout_main" :class="{ 'static-top': config.staticTop, collapsed: config.collapsed }">
      <div id="layout_main_top" :style="!config.tabMode ? 'box-shadow:5px 0px 8px #ddd' : ''">
        <a id="menu_collapse" @click="collapsedChange()">
          <i :class="{ 'el-icon-s-fold': !config.collapsed, 'el-icon-s-unfold': config.collapsed }"></i>
        </a>
        <ElBreadcrumb id="breadcrumb" separator="/" style="margin-left: 10px;">
          <ElBreadcrumb-item>
            <RouterLink to="/admin">首页</RouterLink>
          </ElBreadcrumb-item>
          <ElBreadcrumb-item v-if="$route.path.toLocaleLowerCase() != '/admin'">
            <a>{{ menu[$route.path.toLocaleLowerCase()] }}</a>
          </ElBreadcrumb-item>
        </ElBreadcrumb>
        <div id="layout_tools">
          <ElTooltip :content="config.tabMode ? '关闭多标签页模式' : '启用多标签页模式'">
            <a @click="tabModeChange()">
              <i :class="config.tabMode ? 'el-icon-more' : 'el-icon-more-outline'"></i>
            </a>
          </ElTooltip>
          <ElTooltip :content="config.staticTop ? '取消固定顶部' : '固定顶部'">
            <a @click="staticTopChange()">
              <i :class="config.staticTop ? 'el-icon-arrow-up' : 'el-icon-sort-down'"></i>
            </a>
          </ElTooltip>
          <ElTooltip :content="config.fullscreen ? '退出全屏' : '进入全屏'">
            <a id="fullscreen" @click="fullscreenChange()">
              <i
                :class="{ 'fa fa-expand': !config.fullscreen, 'fa fa-compress': config.fullscreen }"
                aria-hidden="true"
              ></i>
            </a>
          </ElTooltip>

          <ElDropdown id="user" @command="handleCommand">
            <span class="ElDropdown-link">
              <img :src="userinfo.Headshot" class="userface" />
              <!-- <el-avatar style="margin-top: 5px;" :src="userimg"></el-avatar> -->
              <i class="el-icon-arrow-down el-icon--right"></i>
            </span>
            <ElDropdownMenu slot="dropdown">
              <ElDropdownItem command="pwd"> <i class="el-icon-lock"></i>修改密码 </ElDropdownItem>
              <ElDropdownItem divided command="exit"> <i class="el-icon-switch-button"></i>退出 </ElDropdownItem>
            </ElDropdownMenu>
          </ElDropdown>
        </div>
      </div>
      <div id="layout_main_tabs" v-if="config && config.tabMode">
        <div class="tabs">
          <div id="tabs_wrap">
            <ElTabs
              :value="$route.path.toLocaleLowerCase()"
              @tab-remove="tabRemove"
              @tab-click="clickTab"
              type="card"
              size="small"
              v-if="tabs"
            >
              <ElTabPane :closable="tab != '/admin'" :name="tab" v-for="tab in tabs" :key="tab">
                <span slot="label">{{ menu[tab] }}</span>
              </ElTabPane>
            </ElTabs>
          </div>

          <div>
            <ElDropdown size="medium" @command="tabCommand">
              <span class="ElDropdown-link">
                更多操作
                <i class="el-icon-arrow-down el-icon--right"></i>
              </span>
              <ElDropdownMenu slot="dropdown">
                <ElDropdownItem icon="el-icon-caret-left" command="closeleft">关闭左侧</ElDropdownItem>
                <ElDropdownItem icon="el-icon-caret-right" command="closeright">关闭右侧</ElDropdownItem>
                <ElDropdownItem icon="el-icon-remove" command="closeother">关闭其它</ElDropdownItem>
                <ElDropdownItem icon="el-icon-circle-close" command="closeall">全部关闭</ElDropdownItem>
              </ElDropdownMenu>
            </ElDropdown>
          </div>
        </div>
      </div>
      <div id="layout_main_center">
        <transition name="slidein">
          <KeepAlive :include="keep_alives">
            <RouterView />
          </KeepAlive>
        </transition>
      </div>
    </div>

    <el-dialog
      title="修改密码"
      :lock-scroll="false"
      :visible.sync="pwdDialogVisiable"
      width="600px"
      close
      :close-on-click-modal="false"
    >
      <div>
        <el-form ref="pwdForm" label-width="100px" @submit.native.prevent :model="pwd" :rules="pwdRules">
          <el-form-item label="旧密码" prop="old">
            <el-input type="password" v-model="pwd.old" placeholder="请输入旧密码" autocomplete="off">></el-input>
          </el-form-item>
          <el-form-item label="密码" prop="pass">
            <el-input type="password" v-model="pwd.pass" placeholder="请输入新密码" autocomplete="off"></el-input>
          </el-form-item>
          <el-form-item label="确认密码" prop="checkPass">
            <el-input
              type="password"
              v-model="pwd.checkPass"
              placeholder="请再次输入新密码"
              autocomplete="off"
            ></el-input>
          </el-form-item>
        </el-form>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button @click="pwdDialogVisiable = false" icon="el-icon-close">取 消</el-button>
        <el-button type="primary" @click="savePWD()" icon="el-icon-check">确 定</el-button>
      </span>
    </el-dialog>
  </div>
</template>
<style lang="less">
@import url(../less/layout.less);
</style>
<script>
import { mapState, mapMutations } from "vuex";
import BasicMenu from "../components/Layout/BasicMenu.vue";
export default {
  components: {
    BasicMenu,
  },
  mounted() {
    this.$axios.post(this.$baseURL + "/employee/getemployeeinfo").then((res) => {
      this.userinfo = res.data.data;
    });
  },
  //提供依赖注入方法
  provide: function() {
    return {
      hasPermission: (permissionKey) => {
        if (this.permission && this.permission.permissionline && this.permission.permissionline instanceof Array) {
          return this.permission.permissionline.indexOf(permissionKey) > -1;
        } else {
          return false;
        }
      },
    };
  },
  methods: {
    ...mapMutations(["updateStaticTopConfig", "updateCollapsedConfig", "updateTabModeConfig", "addTab", "removeTab"]),
    handleCommand: function(cmd) {
      if (cmd == "exit") {
        this.$axios.post(this.$baseURL + '/login/logout').then(res=>{
           location.href = "/app/login";
        })
       
      } else if (cmd == "pwd") {
        this.pwdDialogVisiable = true;
        this.pwd = { pass: "", checkPass: "", old: "" };
        this.$nextTick(() => {
          this.$refs.pwdForm.clearValidate();
        });
      }
    },
    tabModeChange: function() {
      this.$store.commit("updateTabModeConfig", !this.config.tabMode);
      if (this.config.tabMode) {
        //this.$store.commit("resetTabs");
        if (this.$route.path.toLocaleLowerCase() != "/admin") {
          this.addTab(this.$route.path);
        }
      } else {
        this.$store.commit("resetTabs");
      }
    },
    staticTopChange: function() {
      this.$store.commit("updateStaticTopConfig", !this.config.staticTop);
    },
    collapsedChange: function() {
      this.$store.commit("updateCollapsedConfig", !this.config.collapsed);
    },
    fullscreenChange: function() {
      /*判断是否全屏*/
      var isFullscreen =
        document.fullScreenElement || //W3C
        document.msFullscreenElement || //IE
        document.mozFullScreenElement || //火狐
        document.webkitFullscreenElement || //谷歌
        false;
      if (!isFullscreen) {
        this.config.fullscreen = true;
        var el = document.documentElement;
        if (el.requestFullscreen) {
          el.requestFullscreen();
        } else if (el.mozRequestFullScreen) {
          el.mozRequestFullScreen();
        } else if (el.webkitRequestFullscreen) {
          el.webkitRequestFullscreen();
        } else if (el.msRequestFullscreen) {
          el.msRequestFullscreen();
        }
      } else {
        this.config.fullscreen = false;
        if (document.exitFullscreen) {
          document.exitFullscreen();
        } else if (document.msExitFullscreen) {
          document.msExitFullscreen();
        } else if (document.mozCancelFullScreen) {
          document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
          document.webkitCancelFullScreen();
        }
      }
    },
    clickTab(e) {
      if (e.name != this.$route.path.toLocaleLowerCase()) this.$router.push(e.name);
    },
    tabRemove(e) {
      var index;
      if (e == this.$route.path.toLocaleLowerCase()) {
        var index = this.tabs.findIndex((s) => s == e);
      }
      this.removeTab(e);
      if (index !== undefined) {
        if (index < this.tabs.length) {
          this.$router.push(this.tabs[index]);
        } else {
          this.$router.push(this.tabs[index - 1]);
        }
      }
    },
    tabCommand(e) {
      var index = this.tabs.findIndex((s) => s == this.$route.path.toLocaleLowerCase());
      switch (e) {
        case "closeleft":
          for (var i = index - 1; i > 0; i--) {
            this.removeTab(this.tabs[i]);
          }

          //关闭左侧
          break;
        case "closeright":
          //关闭右侧
          for (var i = this.tabs.length - 1; i > index; i--) {
            this.removeTab(this.tabs[i]);
          }
          break;
        case "closeother":
          //关闭其他
          for (var i = this.tabs.length - 1; i > index; i--) {
            this.removeTab(this.tabs[i]);
          }

          for (var i = index - 1; i > 0; i--) {
            this.removeTab(this.tabs[i]);
          }

          break;
        case "closeall":
          for (var i = this.tabs.length - 1; i > 0; i--) {
            this.removeTab(this.tabs[i]);
          }
          this.$router.push(this.tabs[0]);
        //关闭全部
      }
    },

    savePWD: function() {
      this.$refs.pwdForm.validate((result) => {
        if (result) {
          this.$axios.post('/api/employee/changepassword',`old=${encodeURIComponent(this.pwd.old)}&pass=${encodeURIComponent(this.pwd.pass)}`).then(res=>{
            
            if(res.data.success){
              this.pwdDialogVisiable=false;
            }
            this.$message({
              message:res.data.msg,
              type:res.data.success?'success':"warning"
            })
          })
        }
      });
    },
  },
  computed: {
    ...mapState(["config", "menu", "tabs", "permission"]),
    keep_alives() {
      return this.tabs.map((s) => s.replace(/\//g, ""));
    },
  },
  data() {
    var that = this;
    return {
      userinfo: {},
      pwd: {},
      pwdDialogVisiable: false,
      pwdRules: {
        old: [
          {
            validator: (rule, value, callback) => {
              if (value === "") {
                callback(new Error("请输入密码"));
              } else {
                callback();
              }
            },
          },
        ],
        pass: [
          { min: 6, max: 16, message: "密码长度在6~16个字符" },
          {
            validator: function(rule, value, callback) {
              if (value === "") {
                callback(new Error("请输入密码"));
              } else {
                if (that.pwd.checkPass !== "") {
                  that.$refs.pwdForm.validateField("checkPass");
                }
                callback();
              }
            },
          },
        ],
        checkPass: [
          {
            validator: (rule, value, callback) => {
              if (value === "") {
                callback(new Error("请再次输入密码"));
              } else if (value !== that.pwd.pass) {
                callback(new Error("两次输入密码不一致!"));
              } else {
                callback();
              }
            },
            trigger: "blur",
          },
        ],
      },
    };
  },
};
</script>
