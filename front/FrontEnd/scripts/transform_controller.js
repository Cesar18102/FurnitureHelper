let TRANSFORM_CONTROLLER = {
	translation : {
		x : 0,
		y : 0,
		z : 0
	},
	absTranslation : {
		x : 0,
		y : 0,
		z : 0
	},
	zoom : 1,
	absZoom : 1,
	makeZoom : function(zoom) {
		this.zoom = zoom;
		this.absZoom *= zoom;
	},
	makeTranslation : function(x, y, z) {
		this.translation.x = x;
		this.translation.y = y;
		this.translation.z = z;
		
		this.absTranslation.x += x;
		this.absTranslation.y += y;
		this.absTranslation.z += z;
	},
	update : function() {
		this.makeZoom(1);
		this.makeTranslation(0, 0, 0);
	}
}

const ZOOM_DELTA_UP = 1.1;
const ZOOM_DELTA_DOWN = 0.9;

const TRANSLATION_DELTA_UP = 5;
const TRANSLATION_DELTA_DOWN = -5;

window.onkeydown = event => {
	switch(event.code) {
		case "NumpadAdd" : 
			TRANSFORM_CONTROLLER.makeZoom(ZOOM_DELTA_UP); 
			break;
		case "NumpadSubtract" : 
			TRANSFORM_CONTROLLER.makeZoom(ZOOM_DELTA_DOWN); 
			break;
		case "Numpad4" : 
			TRANSFORM_CONTROLLER.makeTranslation(TRANSLATION_DELTA_UP, 0); 
			break;
		case "Numpad6" : 
			TRANSFORM_CONTROLLER.makeTranslation(TRANSLATION_DELTA_DOWN, 0); 
			break;
		case "Numpad8" : 
			TRANSFORM_CONTROLLER.makeTranslation(0, TRANSLATION_DELTA_UP); 
			break;
		case "Numpad2" : 
			TRANSFORM_CONTROLLER.makeTranslation(0, TRANSLATION_DELTA_DOWN); 
			break;
	}
};