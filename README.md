# arx-mesh-editor

An editor for the level files of Arx Fatalis

[![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg)](http://commitizen.github.io/cz-cli/)

## file formats of Arx

source: https://wiki.arx-libertatis.org/Filetypes

`dlf` - level

`fts` - compiled scene

`llf` - level lighting

`DanaeLoadLevel` - https://github.com/arx/ArxLibertatis/blob/master/src/scene/LoadLevel.cpp#L225

## goals for this project

[ ] load the dlf file

[ ] parse it

[ ] render vertexes to canvas with webgl

## low priority todos

WebpackDevServer is not live reloading html files

## controls to implement - preferrably how blender does it

3 button setup:
  rotate - MMB (middle mouse button) + moving mouse
  pan - shift + MMB + moving mouse
  zoom - scroll mouse wheel

2 button setup:
  rotate - alt + LMB
  pan - shift + alt + LMB
  zoom - ctrl + alt + LMB

