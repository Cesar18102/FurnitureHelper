async function handleContinious(continious, loader, e, prevent = true) {
	if(e == null || e == undefined || !e.originalEvent.defaultPrevented) {
		loader.hidden = false;
		let result = await continious();
		
		if(e != null && e != undefined && prevent) {
			e.originalEvent.preventDefault();
			e.originalEvent.stopPropagation();
		}
		
		loader.hidden = true;
		return result;
	}
	return null;
}