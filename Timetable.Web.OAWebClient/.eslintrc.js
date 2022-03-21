module.exports = {
  root: true,
  env: {
    node: true
  },
  'extends': [
    'plugin:vue/essential',
    'eslint:recommended'
  ],
  parserOptions: {
    parser: 'babel-eslint'
  },
  rules: {
    'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'no-redeclare':'off',
    'no-undef':'off',
    'no-inner-declarations':'off',
    'no-unused-vars':'off',
    'no-extra-semi':'off',
    'no-case-declarations':'off',
    "vue/no-dupe-keys":'off'
  }
}
