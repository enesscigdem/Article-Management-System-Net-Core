define( [
	"~/PanelTheme/src/core",
	"~/PanelTheme/src/selector",
	"~/PanelTheme/src/effects"
], function( jQuery ) {

"use strict";

jQuery.expr.pseudos.animated = function( elem ) {
	return jQuery.grep( jQuery.timers, function( fn ) {
		return elem === fn.elem;
	} ).length;
};

} );
