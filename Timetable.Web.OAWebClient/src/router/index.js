import Vue from "vue";
import VueRouter from "vue-router";
import NProgress from "nprogress";
NProgress.configure({ showSpinner: false, minimum: 0.6, trickleRate: 0.01, trickleSpeed: 100 });
Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    redirect: "/admin",
  },
  {
    path: "/login",
    component: () => import("../views/Login.vue"),
  },
  {
    path: "/admin",
    component: () => import("../views/_Layout.vue"),
    children: [
      {
        path: "",
        meta: {
          auth: true,
        },
        component: () => import(/*webpackChunkName:"home"*/"../views/Home.vue"),
      },
      {
        path: "permissiongroup",
        meta: {
          auth: true,
        },
        component: () => import("../views/PermissionGroup.vue"),
      },
      {
        path: "permissionline",
        meta: {
          auth: true,
        },
        component: () => import("../views/PermissionLine.vue"),
      },
      {
        path: "role",
        meta: {
          auth: true,
        },
        component: () => import("../views/Role.vue"),
      },
      {
        path: "employee",
        meta: {
          auth: true,
        },
        component: () => import("../views/Employee.vue"),
      },
      {
        path: "employeeloginlog",
        meta: {
          auth: true,
        },
        component: () => import("../views/EmployeeLoginLog.vue"),
      },
      {
        path: "user",
        meta: {
          auth: true,
        },
        component: () => import("../views/User.vue"),
      },
      {
        path: "usergroup",
        meta: {
          auth: true,
        },
        component: () => import("../views/UserGroup.vue"),
      },
      {
        path: "userregister",
        meta: {
          auth: true,
        },
        component: () => import("../views/UserRegister.vue"),
      },
      {
        path: "userloginlog",
        meta: {
          auth: true,
        },
        component: () => import("../views/UserLoginLog.vue"),
      },
      {
        path: "school",
        meta: {
          auth: true,
        },
        component: () => import("../views/School.vue"),
      },
      {
        path: "servicepoint",
        meta: {
          auth: true,
        },
        component: () => import("../views/ServicePoint.vue"),
      },
      {
        path: "auditsurvey",
        meta: {
          auth: true,
        },
        component: () => import("../views/AuditSurvey.vue"),
      },
      {
        path: "survey",
        meta: {
          auth: true,
        },
        component: () => import("../views/Survey.vue"),
      },
      {
        path: "tipoff",
        meta: {
          auth: true,
        },
        component: () => import("../views/TipOff.vue"),
      },
      {
        path: "tag",
        meta: {
          auth: true,
        },
        component: () => import("../views/Tag.vue"),
      },
      {
        path: "template",
        meta: {
          auth: true,
        },
        component: () => import("../views/SubjectTemplate.vue"),
      },
      {
        path: "proreport",
        meta: {
          auth: true,
        },
        component: () => import("../views/ProReport.vue"),
      },
      {
        path: "proreportpackage",
        meta: {
          auth: true,
        },
        component: () => import("../views/ProReportPackage.vue"),
      },
      { path: "test", meta: { auth: true }, component: () => import("../views/Test") },
    ],
  },
  {
    path: "/*",
    component: () => import("../views/error_pages/ERROR_404.vue"),
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

import store from "../store/index";
import axios from "axios";
function pass(next, to) {
  if (to.path.toLocaleLowerCase().indexOf("/admin") > -1 && store.state.config.tabMode) {
    console.log("tab设置" + to.path);

    store.commit("addTab", to.path.toLocaleLowerCase());
  }

  next();
}
function deny(next, to) {
  next({ path: "/login", query: { returnUrl: to.path } });
}

router.beforeEach(async (to, from, next) => {
  NProgress.start();
  if (to.path.toLocaleLowerCase() == "/login") {
    next();

    return;
  }
  if (to.path.length > 1 && to.path.lastIndexOf("/") == to.path.length - 1) {
    next(to.path.substr(0, to.path.length - 1));
  } else {
    var userinfo = (await getUserInfo()).data.data;
    if (userinfo) {
      store.commit("updateMenuList", userinfo.menu);
      store.commit("updatePermissionMenu", userinfo);
      if (to.meta && to.meta.auth) {
        if (userinfo.logined) {
          if (to.path) {
            if (userinfo.pages.indexOf(to.path.toLocaleLowerCase()) > -1) {
              console.log("路由进入");
              pass(next, to);
            } else {
              deny(next, to);
            }
          } else {
            pass(next, to);
          }
        } else {
          deny(next, to);
        }
      } else {
        pass(next, to);
      }
    } else {
      deny(next, to);
    }
  }
});
router.afterEach(() => {
  NProgress.done();
});
async function getUserInfo() {
  return await axios.post(process.env.VUE_APP_SERVER_BASE_API + "/auth/getPermissionMenu");
}
export default router;
