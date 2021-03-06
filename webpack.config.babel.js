import path from 'path'

import TerserPlugin from 'terser-webpack-plugin'
import MiniCssExtractPlugin from 'mini-css-extract-plugin'
import OptimizeCSSAssetsPlugin from 'optimize-css-assets-webpack-plugin'
import autoprefixer from 'autoprefixer'
import webpack from 'webpack'

const mode = process.env.NODE_ENV === 'development' ? 'development' : 'production'

const config = {
  entry: {
    ArxMeshEditor: ['regenerator-runtime/runtime', './src/css/style.scss', './src/js/index.ts']
  },
  output: {
    path: path.resolve(__dirname, 'static'),
    filename: 'js/[name].js'
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: 'css/[name].css'
    }),
    new webpack.BannerPlugin({
      banner: 'Arx Mesh Editor - created by Lajos Meszaros <m_lajos@hotmail.com>'
    }),
    // https://github.com/webpack/webpack/issues/2537#issuecomment-263630802
    new webpack.EnvironmentPlugin(['NODE_ENV'])
  ],
  mode,
  module: {
    rules: [
      {
        test: /\.(ts|js)$/,
        loader: 'babel-loader',
        query: {
          compact: false
        }
      },
      {
        test: /\.s?css$/,
        use: [
          MiniCssExtractPlugin.loader,
          {
            loader: 'css-loader'
          },
          {
            loader: 'postcss-loader',
            options: {
              plugins: () => [autoprefixer]
            }
          },
          'sass-loader'
        ]
      }
    ]
  },
  resolve: {
    extensions: ['.js', '.ts', '.json', '.scss', '.css']
  },
  optimization: {
    minimizer: [
      new TerserPlugin({
        cache: false,
        parallel: true,
        sourceMap: true
      }),
      new OptimizeCSSAssetsPlugin({})
    ]
  },
  devtool: mode === 'production' ? 'none' : 'cheap-module-eval-source-map',
  devServer: {
    contentBase: './static'
  }
}

export default config
