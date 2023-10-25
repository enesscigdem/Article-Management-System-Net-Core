import Events from '~/PanelTheme/src/Events'
import Localization from './Localization'
import Animations from '~/PanelTheme/src/Animations'
import Axes from '~/PanelTheme/src/axes/Axes'
import Config from '~/PanelTheme/src/settings/Config'
import CoreUtils from '~/PanelTheme/src/CoreUtils'
import Crosshairs from '~/PanelTheme/src/Crosshairs'
import Grid from '~/PanelTheme/src/axes/Grid'
import Graphics from '~/PanelTheme/src/Graphics'
import Exports from '~/PanelTheme/src/Exports'
import Options from '~/PanelTheme/src/settings/Options'
import Responsive from '~/PanelTheme/src/Responsive'
import Series from '~/PanelTheme/src/Series'
import Theme from '~/PanelTheme/src/Theme'
import Formatters from '~/PanelTheme/src/Formatters'
import TitleSubtitle from '~/PanelTheme/src/TitleSubtitle'
import Legend from '~/PanelTheme/src/legend/Legend'
import Toolbar from '~/PanelTheme/src/Toolbar'
import Dimensions from '~/PanelTheme/src/dimensions/Dimensions'
import ZoomPanSelection from '~/PanelTheme/src/ZoomPanSelection'
import Tooltip from '~/PanelTheme/src/tooltip/Tooltip'
import Core from '~/PanelTheme/src/Core'
import Data from '~/PanelTheme/src/Data'
import UpdateHelpers from './UpdateHelpers'

import '~/PanelTheme/src/~/PanelTheme/src/svgjs/svg.js'
import 'svg.filter.js'
import 'svg.pathmorphing.js'
import 'svg.draggable.js'
import 'svg.select.js'
import 'svg.resize.js'

// global Apex object which user can use to override chart's defaults globally
if (typeof window.Apex === 'undefined') {
  window.Apex = {}
}

export default class InitCtxVariables {
  constructor(ctx) {
    this.ctx = ctx
    this.w = ctx.w
  }

  initModules() {
    this.ctx.publicMethods = [
      'updateOptions',
      'updateSeries',
      'appendData',
      'appendSeries',
      'toggleSeries',
      'showSeries',
      'hideSeries',
      'setLocale',
      'resetSeries',
      'zoomX',
      'toggleDataPointSelection',
      'dataURI',
      'addXaxisAnnotation',
      'addYaxisAnnotation',
      'addPointAnnotation',
      'clearAnnotations',
      'removeAnnotation',
      'paper',
      'destroy'
    ]

    this.ctx.eventList = [
      'click',
      'mousedown',
      'mousemove',
      'mouseleave',
      'touchstart',
      'touchmove',
      'touchleave',
      'mouseup',
      'touchend'
    ]

    this.ctx.animations = new Animations(this.ctx)
    this.ctx.axes = new Axes(this.ctx)
    this.ctx.core = new Core(this.ctx.el, this.ctx)
    this.ctx.config = new Config({})
    this.ctx.data = new Data(this.ctx)
    this.ctx.grid = new Grid(this.ctx)
    this.ctx.graphics = new Graphics(this.ctx)
    this.ctx.coreUtils = new CoreUtils(this.ctx)
    this.ctx.crosshairs = new Crosshairs(this.ctx)
    this.ctx.events = new Events(this.ctx)
    this.ctx.exports = new Exports(this.ctx)
    this.ctx.localization = new Localization(this.ctx)
    this.ctx.options = new Options()
    this.ctx.responsive = new Responsive(this.ctx)
    this.ctx.series = new Series(this.ctx)
    this.ctx.theme = new Theme(this.ctx)
    this.ctx.formatters = new Formatters(this.ctx)
    this.ctx.titleSubtitle = new TitleSubtitle(this.ctx)
    this.ctx.legend = new Legend(this.ctx)
    this.ctx.toolbar = new Toolbar(this.ctx)
    this.ctx.dimensions = new Dimensions(this.ctx)
    this.ctx.updateHelpers = new UpdateHelpers(this.ctx)
    this.ctx.zoomPanSelection = new ZoomPanSelection(this.ctx)
    this.ctx.w.globals.tooltip = new Tooltip(this.ctx)
  }
}
