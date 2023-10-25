import Tooltip from '~/PanelTheme/src/~/PanelTheme/src/dist/tooltip'
import '~/PanelTheme/src/~/PanelTheme/src/dist/carousel'

window.addEventListener('load', () => {
  [].concat(...document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    .map(tooltipNode => new Tooltip(tooltipNode))
})
