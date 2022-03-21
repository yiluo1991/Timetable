<template>
  <el-menu
    :default-active="path"
    :collapse="config.collapsed"
    :collapse-transition="false"
    background-color="#304156"
    @select="onNavmenuSelect"
    unique-opened
    text-color="#bfcbd9"
  >
    <template v-if="menuData">
      <template v-for="main in menuData">
        <el-submenu
          :index="main.Url||main.Id.toString()"
          :key="main.Id"
          v-if="main.Children&&main.Children.length>0"
        >
          <template slot="title">
            <i :class="main.Icon"></i>
            <span>{{main.Name}}</span>
          </template>

          <el-menu-item
            v-for="sub in main.Children"
            :key="sub.Id"
            :index="sub.Url"
            :url="sub.Url"
          >{{sub.Name}}</el-menu-item>
        </el-submenu>
        <el-menu-item
          :index="main.Url||main.Id.toString()"
          :key="main.Id"
          v-if="!main.Children||(main.Children&&main.Children.length==0)"
        >
          <i :class="main.Icon"></i>
          <span slot="title">{{main.Name}}</span>
        </el-menu-item>
      </template>
    </template>
  </el-menu>
</template>
<script>
import { mapState } from "vuex";
export default {
  computed: {
    ...mapState({
      config: "config",
      permission: "permission",
    }),
    menuData() {
      return this.permission.menu;
    },
    path() {
      return this.$route.path.toLocaleLowerCase();
    },
  },

  data() {
    return {};
  },
 
  methods: {
    onNavmenuSelect: function (index, path, vnode) {
      if (index != this.$route.path.toLocaleLowerCase())
        this.$router.push(index);
    },
  },
};
</script>