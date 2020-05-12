async function handleContinious(e, loader, continious, prevent = true) {
	if(!e.originalEvent.defaultPrevented) {
		loader.hidden = false;
		let result = await continious();
		
		if(prevent) {
			e.originalEvent.preventDefault();
			e.originalEvent.stopPropagation();
		}
		
		loader.hidden = true;
		return result;
	}
	return null;
}