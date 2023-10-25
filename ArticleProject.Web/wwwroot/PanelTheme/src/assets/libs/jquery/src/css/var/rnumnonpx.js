define( [
	"~/PanelTheme/src/~/PanelTheme/src/var/pnum"
], function( pnum ) {
	"use strict";

	return new RegExp( "^(" + pnum + ")(?!px)[a-z%]+$", "i" );
} );
