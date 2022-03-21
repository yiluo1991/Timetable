<template>
  <div :class="{ noevent: noevent }" @click.stop>
    <table class="matable">
      <colgroup>
        <col width="2" class="hidden-sm-c" />
        <col v-for="option in question.options" :key="option.oid" width="1" />
      </colgroup>
      <thead>
        <tr>
          <th class="hidden-sm"></th>
          <th v-for="option in question.options" :key="option.oid">
            {{ option["text_" + lang] }}
          </th>
        </tr>
      </thead>
      <tbody>
        <template v-for="item in question.items">
          <tr class="visiable-sm" :key="item.fid + 't'">
            <th  class="mt" :colspan="question.options.length">
              <span >{{ item["text_" + lang] }}</span>
            </th>
          </tr>
          <tr :key="item.fid + 'o'">
            <th class="hidden-sm">{{ item["text_" + lang] }}</th>
            <td v-for="o in question.options" :key="o.oid">
              <ElRadio v-model="value[item.fid]" :label="o.value"> <span></span></ElRadio>
            </td>
          </tr>
        </template>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  props: {
    lang: String,
    noevent: {
      type: Boolean,
      default: false,
    },
    question: Object,
    value: {
      type: Object,
      default: function() {
        return {};
      },
    },
  },

  mounted() {
    this.$watch(
      "value",
      () => {
        this.$emit("input", this.value);
        if (this.$parent && this.$parent.form) {
          this.$parent.validate();
        }
      },
      { deep: true }
    );
  },
};
</script>
<style lang="less"></style>
