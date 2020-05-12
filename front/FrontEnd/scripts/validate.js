function addValidation(form, field, invalidMessage, validPredicate) {	
	field.addEventListener("input", () => {
		if(validPredicate(field)) {
			field.setCustomValidity("");
		}
	});
	
	form.addEventListener("change", () => {
		if(!validPredicate(field)) {
			field.setCustomValidity(invalidMessage);
		} else {
			field.setCustomValidity("");
		}
	});
	
	$(form).submit(e => {
		if(!validPredicate(field)) {
			field.setCustomValidity(invalidMessage);
			e.originalEvent.preventDefault();
			e.originalEvent.stopPropagation();
		} else {
			field.setCustomValidity("");
		}
	});
}