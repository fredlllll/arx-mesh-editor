const autoprefixer = require('autoprefixer')

module.exports = config => {
  // https://github.com/electron-userland/electron-webpack/blob/master/docs/en/modifying-webpack-configurations.md#use-case-1
  const styleRules = config.module.rules.filter(rule => rule.test.toString().match(/css|less|s\(\[ac\]\)ss/))

  styleRules.forEach(rule => {
    const cssLoaderIdx = rule.use.findIndex(use => use.loader === 'css-loader')
    const cssLoader = rule.use[cssLoaderIdx]
    cssLoader.options.modules = {
      localIdentName: '[local]_[hash:base64:5]'
    }

    rule.use.splice(cssLoaderIdx, 0, 'css-modules-typescript-loader')
    rule.use.splice(cssLoaderIdx + 2, 0, {
      loader: 'postcss-loader',
      options: {
        plugins: () => [autoprefixer]
      }
    })
  })

  return config
}
