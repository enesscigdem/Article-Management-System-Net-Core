/* eslint-env jquery */

import Alert from '~/PanelTheme/src/~/PanelTheme/src/src/alert'
import Button from '~/PanelTheme/src/~/PanelTheme/src/src/button'
import Carousel from '~/PanelTheme/src/~/PanelTheme/src/src/carousel'
import Collapse from '~/PanelTheme/src/~/PanelTheme/src/src/collapse'
import Dropdown from '~/PanelTheme/src/~/PanelTheme/src/src/dropdown'
import Modal from '~/PanelTheme/src/~/PanelTheme/src/src/modal'
import Offcanvas from '~/PanelTheme/src/~/PanelTheme/src/src/offcanvas'
import Popover from '~/PanelTheme/src/~/PanelTheme/src/src/popover'
import ScrollSpy from '~/PanelTheme/src/~/PanelTheme/src/src/scrollspy'
import Tab from '~/PanelTheme/src/~/PanelTheme/src/src/tab'
import Toast from '~/PanelTheme/src/~/PanelTheme/src/src/toast'
import Tooltip from '~/PanelTheme/src/~/PanelTheme/src/src/tooltip'
import { clearFixture, getFixture } from '~/PanelTheme/src/helpers/fixture'

describe('jQuery', () => {
  let fixtureEl

  beforeAll(() => {
    fixtureEl = getFixture()
  })

  afterEach(() => {
    clearFixture()
  })

  it('should add all plugins in jQuery', () => {
    expect(Alert.jQueryInterface).toEqual(jQuery.fn.alert)
    expect(Button.jQueryInterface).toEqual(jQuery.fn.button)
    expect(Carousel.jQueryInterface).toEqual(jQuery.fn.carousel)
    expect(Collapse.jQueryInterface).toEqual(jQuery.fn.collapse)
    expect(Dropdown.jQueryInterface).toEqual(jQuery.fn.dropdown)
    expect(Modal.jQueryInterface).toEqual(jQuery.fn.modal)
    expect(Offcanvas.jQueryInterface).toEqual(jQuery.fn.offcanvas)
    expect(Popover.jQueryInterface).toEqual(jQuery.fn.popover)
    expect(ScrollSpy.jQueryInterface).toEqual(jQuery.fn.scrollspy)
    expect(Tab.jQueryInterface).toEqual(jQuery.fn.tab)
    expect(Toast.jQueryInterface).toEqual(jQuery.fn.toast)
    expect(Tooltip.jQueryInterface).toEqual(jQuery.fn.tooltip)
  })

  it('should use jQuery event system', () => {
    return new Promise(resolve => {
      fixtureEl.innerHTML = [
        '<div class="alert">',
        '  <button type="button" data-bs-dismiss="alert">x</button>',
        '</div>'
      ].join('')

      $(fixtureEl).find('.alert')
        .one('closed.bs.alert', () => {
          expect($(fixtureEl).find('.alert')).toHaveSize(0)
          resolve()
        })

      $(fixtureEl).find('button').trigger('click')
    })
  })
})
