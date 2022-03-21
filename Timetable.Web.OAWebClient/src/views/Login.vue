<template>
  <div id="login-page">
    <div id="particles" v-if="bg"></div>
    <div id="bg"></div>
    <div id="login-box-outer">
      <div id="boxbg">
        <div id="blurbg"></div>
      </div>
      <div id="imgbox">
        <img src="../resources/images/user.jpg" alt="Alternate Text" />
        <div>
          <h1>欢迎登录数据中心</h1>
          <h2>welcom to login</h2>
        </div>
      </div>

      <div id="login-box-inner">
        <ElForm ref="form" label-width="100px" label-position="top" :rules="rules" v-bind:model="info">
          <ElFormItem label prop="loginname">
            <ElInput
              type="text"
              autocomplete="off"
              style="width:100%;"
              placeholder="请输入用户名"
              v-model="info.loginname"
              v-on:keyup.native.13="login"
            >
              <i slot="prepend" class="el-icon-user"></i>
            </ElInput>
          </ElFormItem>
          <ElFormItem label prop="password">
            <ElInput
              type="password"
              autocomplete="off"
              placeholder="请输入密码"
              v-model="info.password"
              v-on:keyup.native.13="login"
            >
              <i slot="prepend" class="el-icon-lock"></i>
            </ElInput>
          </ElFormItem>

          <ElFormItem style="text-align:center;">
            <ElButton
              type="primary"
              :icon="logining ? 'el-icon-loading' : 'el-icon-arrow-right'"
              round
              @click="login"
              :disabled="logining"
              >{{ logining ? "登陆中..." : "登录" }}</ElButton
            >
          </ElFormItem>
        </ElForm>
      </div>

      <div id="copyright">powered by &copy; lihangsoft.com 励航软件 2016-2020</div>
    </div>
  </div>
</template>
<script>
import ".../../../public/particles.min.js";

export default {
  data() {
    return {
      logining: false,
      bg: true,
      info: {
        password: "",
        loginname: "",
      },
      rules: {
        loginname: [{ required: true, message: "请输入用户名" }],
        password: [{ required: true, message: "请输入密码" }],
      },
    };
  },
  mounted() {
    window.pJSDom = [];
    window.particlesJS("particles", {
      particles: {
        number: {
          value: 90,
          density: {
            enable: true,
            value_area: 800,
          },
        },
        color: {
          value: "#ffffff",
        },
        shape: {
          type: "circle",
          stroke: {
            width: 0,
            color: "#000000",
          },
          polygon: {
            nb_sides: 5,
          },
          image: {
            src: "img/github.svg",
            width: 100,
            height: 100,
          },
        },
        opacity: {
          value: 0.5,
          random: false,
          anim: {
            enable: false,
            speed: 1,
            opacity_min: 0.1,
            sync: false,
          },
        },
        size: {
          value: 5,
          random: true,
          anim: {
            enable: false,
            speed: 40,
            size_min: 0.1,
            sync: false,
          },
        },
        line_linked: {
          enable: true,
          distance: 250,
          color: "#ffffff",
          opacity: 0.4,
          width: 1,
        },
        move: {
          enable: true,
          speed: 3,
          direction: "none",
          random: true,
          straight: false,
          out_mode: "bounce",
          bounce: false,
          attract: {
            enable: false,
            rotateX: 600,
            rotateY: 1200,
          },
        },
      },
      interactivity: {
        detect_on: "canvas",
        events: {
          onhover: {
            enable: true,
            mode: ["bubble"],
          },
          onclick: {
            enable: true,
            mode: "repulse",
          },
          resize: true,
        },
        modes: {
          grab: {
            distance: 400,
            line_linked: {
              opacity: 1,
            },
          },
          bubble: {
            distance: 250,
            size: 15,
            duration: 10,
            opacity: 8,
            speed: 3,
          },
          repulse: {
            distance: 200,
          },
          push: {
            particles_nb: 4,
          },
          remove: {
            particles_nb: 2,
          },
        },
      },
      retina_detect: true,
      config_demo: {
        hide_card: false,
        background_color: "#b61924",
        background_image: "",
        background_position: "50% 50%",
        background_repeat: "no-repeat",
        background_size: "cover",
      },
    });
 
 },
  beforeDestroy() {
    window.pJSDom = undefined;
  },
  methods: {
    login() {
      this.$refs.form.validate((isValid) => {
        if (isValid) {
          this.logining = true;
        
          this.$axios
            .post(this.$baseURL + "/login/userlogin", {
              username: this.info.loginname,
              password: this.info.password,
            })
            .then((res) => {
              if (res.data.success) {
                if (this.$route.query.returnUrl) {
                  this.$router.push(this.$route.query.returnUrl);
                } else {
                  this.$router.push("/admin");
                }
              } else {
                this.$message({
                  message: res.data.msg,
                  type: "error",
                });
                this.logining = false;
              }
            });
        }
      });
    },
  },
};
</script>
<style lang="less">
#login-page {
  #bg {
    background: url("../resources/images/loginbg.jpg") no-repeat fixed;
    background-size: cover;
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    z-index: -2;
  }

  height: 100%;
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  display: flex;
  align-items: center;
  z-index: -1;
  justify-content: center;

  #particles {
    position: fixed;
    width: 100%;
    height: 100%;
    top: 0;
    left: 0;
  }

  #login-box-outer {
    #boxbg {
      position: absolute;
      top: -50px;
      bottom: -50px;
      left: -50px;
      right: -50px;
      opacity: 0.95;
      z-index: 0;
      background: rgba(255, 255, 255, 0.8);

      #blurbg {
        opacity: 0.8;
        background: url("../resources/images/loginbg.jpg") no-repeat fixed;
        -webkit-filter: blur(10px);
        background-size: cover;
        width: 100%;
        height: 100%;
      }
    }

    border-radius: 15px;
    overflow: hidden;
    position: relative;
    z-index: 1;
    width: 430px;
    display: block;
    height: 380px;
    box-shadow: 0px 0px 55px 1px rgba(0, 0, 0, 0.3);

    #imgbox {
      background: #3782cf;
      background: -webkit-gradient(
        linear,
        80% 39%,
        35% 80%,
        from(rgba(39, 91, 145, 0.8)),
        to(rgba(55, 130, 207, 0.8)),
        color-stop(1, rgba(55, 130, 207, 0.8))
      );
      padding: 20px 0 30px 0;
      display: flex;
      align-items: center;
      position: relative;
      z-index: 0;

      img {
        margin: 10px 20px;
        width: 80px;
        height: 80px;
        border-radius: 50%;
      }

      div {
        flex: 1;
        text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.6);

        h1 {
          color: #fff;
          font-size: 30px;
          font-weight: normal;
          margin: 0;
        }

        h2 {
          margin: 0;
          color: #fff;
          font-size: 20px;
          font-weight: normal;
        }
      }
    }

    #login-box-inner {
      position: relative;
      z-index: 1;
      border-radius: 5px;
      background: rgba(255, 255, 255, 0.8);
      width: 90%;
      margin: 0 auto;
      height: 210px;
      box-sizing: border-box;
      padding: 25px 10px 25px 10px;
      box-shadow: 0px 0px 20px rgba(0, 0, 0, 0.2);
      margin-top: -20px;
    }

    #copyright {
      position: absolute;
      bottom: 0;
      text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.6);
      text-align: center;
      color: rgba(255, 255, 255, 0.6);
      line-height: 40px;
      width: 100%;
      font-size: 12px;
    }
  }
}
</style>
