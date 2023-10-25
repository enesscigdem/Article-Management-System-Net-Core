import { Tooltip } from '~/PanelTheme/src/~/PanelTheme/src/~/PanelTheme/src/dist/js/bootstrap.esm.js'

window.addEventListener('load', () => {
  [].concat(...document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    .map(tooltipNode => new Tooltip(tooltipNode))
})
